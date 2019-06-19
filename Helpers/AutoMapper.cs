using AutoMapper;
using Microsoft.AspNetCore.Http;
using RoadEye_Service.Dtos.AnomalyDtos;
using RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteRequestDtos;
using RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteResponseDtos;
using RoadEye_Service.Models;
using RoadEye_Service.Repositories;
using RoadEye_Service.Types;

namespace RoadEye_Service.Helpers
{
    public class AutoMapper : Profile
    {
        private readonly IHttpContextAccessor _context;
        private readonly IRoadRepository _roadRepository;

        public AutoMapper(IHttpContextAccessor context, IRoadRepository roadRepository) {
            _context = context;
            _roadRepository = roadRepository;
        }

        public AutoMapper() {
            CreateMap<CreateAnomalyDto, Anomaly>()
                .ForMember(dest => dest.TrueCreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.AnomalyTypeId, opt => opt.MapFrom<AnomalyTypeStringToIdResolver>());
            CreateMap<Anomaly, AnomalyForListDto>()
                .ForPath(dest => dest.Geolocation.Lat, opt => { opt.MapFrom(src => src.Lat); })
                .ForPath(dest => dest.Geolocation.Lng, opt => { opt.MapFrom(src => src.Lng); });
            CreateMap<SuggestRouteRequestDto, RoadIdsList>().ConvertUsing<SuggestRouteRoadsIdsToListConverter>();
            CreateMap<SuggestRouteRequestDto, SuggestRouteResponseDto>().ConvertUsing<SuggestRouteRequestToResponseConverter>();
        }

        private class AnomalyTypeStringToIdResolver : IValueResolver<CreateAnomalyDto, Anomaly, int>
        {
            private readonly IAnomalyTypeRepository _anomalyTypeRepository;
            public AnomalyTypeStringToIdResolver(IAnomalyTypeRepository anomalyTypeRepository) {
                _anomalyTypeRepository = anomalyTypeRepository;
            }

            public int Resolve(CreateAnomalyDto source, Anomaly destination, int destMember, ResolutionContext context) {
                return _anomalyTypeRepository.GetByTypeNameSync(source.Type).Id;
            }
        }

        private class SuggestRouteRoadsIdsToListConverter : ITypeConverter<SuggestRouteRequestDto, RoadIdsList>
        {
            public RoadIdsList Convert(SuggestRouteRequestDto source, RoadIdsList destination, ResolutionContext context) {
                destination = new RoadIdsList();
                foreach(SuggestRouteRequestRouteDto route in source.Routes) {
                    destination.RoadIds.AddRange(route.RoadIds);
                }
                return destination;
            }
        }

        private class SuggestRouteRequestToResponseConverter : ITypeConverter<SuggestRouteRequestDto, SuggestRouteResponseDto>
        {
            private readonly IRoadRepository _roadRepository;
            public SuggestRouteRequestToResponseConverter(IRoadRepository roadRepository) {
                _roadRepository = roadRepository;
            }

            public SuggestRouteResponseDto Convert(SuggestRouteRequestDto source, SuggestRouteResponseDto destination, ResolutionContext context) {
                destination = new SuggestRouteResponseDto();

                foreach(SuggestRouteRequestRouteDto requestRoute in source.Routes) {
                    SuggestRouteResponseRouteDto responseRoute = new SuggestRouteResponseRouteDto {
                        OriginalIndex = requestRoute.OriginalIndex
                    };
                    foreach(string requestRoadId in requestRoute.RoadIds) {
                        responseRoute.Roads.Add(_roadRepository.GetByApiIdSync(requestRoadId));
                    }

                    destination.Routes.Add(responseRoute);
                }
                return destination;
            }
        }
    }
}