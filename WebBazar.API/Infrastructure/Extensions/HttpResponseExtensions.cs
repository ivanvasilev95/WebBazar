using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebBazar.API.Infrastructure;

namespace WebBazar.API.Infrastructure.Extensions
{
    public static class HttpResponseExtensions
    {
        // public static void AddApplicationError(this HttpResponse response, string message)
        // {
        //     response.Headers.Add("Application-Error", message);
        //     response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
        //     response.Headers.Add("Access-Control-Allow-Origin", "*");
        // }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems) 
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems);

            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}