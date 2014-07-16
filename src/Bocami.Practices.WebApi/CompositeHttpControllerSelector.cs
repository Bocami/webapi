using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Bocami.Practices.WebApi
{
    public class CompositeHttpControllerSelector : IHttpControllerSelector
    {
        private readonly IHttpControllerSelector[] httpControllerSelectors;

        public CompositeHttpControllerSelector(params IHttpControllerSelector[] httpControllerSelectors)
        {
            this.httpControllerSelectors = httpControllerSelectors;
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return this.httpControllerSelectors
                .Select(a => a.GetControllerMapping())
                .Where(a => a != null)
                .SelectMany(a => a)
                .GroupBy(a => a.Key, (a, b) => b.First())
                .ToDictionary(a => a.Key, a => a.Value);
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            return this.httpControllerSelectors
                .Select(o => o.SelectController(request))
                .FirstOrDefault(o => o != null);
        }
    }
}