using RoadEye_Service.Dtos;
using RoadEye_Service.Types;
using System.Dynamic;
using static RoadEye_Service.Services.ConstractPaginationHeader.ConstractPaginationHeaderService;

namespace RoadEye_Service.Services.ConstractPaginationHeader
{
    public interface IConstractPaginationHeaderService
    {
        PaginationHeader ConstractPaginationHeader<T>(DefaultIndexParameters defaultIndexParameters,
            PagedList<T> pagedList,
            UrlHelperLinkGenerator urlHelperLinkGenerator,
            ExpandoObject additionalQueryParameters);
    }
}
