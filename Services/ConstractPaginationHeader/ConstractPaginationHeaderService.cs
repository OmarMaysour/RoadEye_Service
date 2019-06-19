using Microsoft.AspNetCore.Mvc;
using RoadEye_Service.Dtos;
using RoadEye_Service.Types;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace RoadEye_Service.Services.ConstractPaginationHeader
{
    public class ConstractPaginationHeaderService : IConstractPaginationHeaderService
    {
        private readonly IUrlHelper _urlHelper;

        public delegate string UrlHelperLinkGenerator(ExpandoObject additionalQueryParameters);

        public ConstractPaginationHeaderService(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        public PaginationHeader ConstractPaginationHeader<T>(
            DefaultIndexParameters defaultIndexParameters, 
            PagedList<T> pagedList,
            UrlHelperLinkGenerator urlHelperLinkGenerator,
            ExpandoObject additionalQueryParameters = null) {
            if(additionalQueryParameters == null) {
                additionalQueryParameters = new ExpandoObject();
            }

            string previousPageLink = pagedList.HasPrevious ?
               CreatePaginationResourceUri(defaultIndexParameters, PaginationResourceUriType.PreviousPage, urlHelperLinkGenerator, additionalQueryParameters) : null;

            string nextPageLink = pagedList.HasNext ?
                CreatePaginationResourceUri(defaultIndexParameters, PaginationResourceUriType.NextPage, urlHelperLinkGenerator, additionalQueryParameters) : null;

            return new PaginationHeader {
                TotalCount = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                PreviousPageLink = previousPageLink,
                NextPageLink = nextPageLink
            };
        }

        private string CreatePaginationResourceUri(
           DefaultIndexParameters defaultIndexParameters,
           PaginationResourceUriType paginationResourceUriType,
           UrlHelperLinkGenerator urlHelperLinkGenerator,
           ExpandoObject additionalQueryParameters) {
            switch (paginationResourceUriType) {
                case PaginationResourceUriType.PreviousPage:
                    (additionalQueryParameters as IDictionary<string, object>).Add("PageNumber", defaultIndexParameters.PageNumber - 1);
                    (additionalQueryParameters as IDictionary<string, object>).Add("PageSize", defaultIndexParameters.PageSize);
                    return urlHelperLinkGenerator(additionalQueryParameters);
                case PaginationResourceUriType.NextPage:
                    (additionalQueryParameters as IDictionary<string, object>).Add("PageNumber", defaultIndexParameters.PageNumber + 1);
                    (additionalQueryParameters as IDictionary<string, object>).Add("PageSize", defaultIndexParameters.PageSize);
                    return urlHelperLinkGenerator(additionalQueryParameters);
                default:
                    (additionalQueryParameters as IDictionary<string, object>).Add("PageNumber", defaultIndexParameters.PageNumber);
                    (additionalQueryParameters as IDictionary<string, object>).Add("PageSize", defaultIndexParameters.PageSize);
                    return urlHelperLinkGenerator(additionalQueryParameters);
            }
        }
    }
}
