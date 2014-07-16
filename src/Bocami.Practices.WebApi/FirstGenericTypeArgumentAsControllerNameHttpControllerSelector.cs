using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Bocami.Practices.WebApi
{
    public class FirstGenericTypeArgumentAsControllerNameHttpControllerSelector : IHttpControllerSelector
    {
        private readonly HttpConfiguration configuration;
        private readonly Dictionary<string, Type> controllerTypeCache;

        public FirstGenericTypeArgumentAsControllerNameHttpControllerSelector(HttpConfiguration configuration)
        {
            this.configuration = configuration;

            var assembliesResolver = configuration.Services.GetAssembliesResolver();
            var httpControllerTypeResolver = configuration.Services.GetHttpControllerTypeResolver();
            controllerTypeCache = CreateControllerTypeCache(httpControllerTypeResolver.GetControllerTypes(assembliesResolver));
        }

        private Dictionary<string, Type> CreateControllerTypeCache(IEnumerable<Type> controllerTypes)
        {
            return controllerTypes
                        .Where(o => o.GenericTypeArguments.Any())
                        .ToDictionary(o => o.GenericTypeArguments.Select(t => t.Name).First(), o => o, StringComparer.InvariantCultureIgnoreCase);
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return null;
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var controllerName = GetControllerName(request);

            if (controllerName == null)
                return null;

            Type controllerType;

            controllerTypeCache.TryGetValue(controllerName, out controllerType);
            
            if (controllerType == null)
                return null;

            return new HttpControllerDescriptor(configuration, controllerName, controllerType);
        }

        private string GetControllerName(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var routeData = request.GetRouteData();

            if (routeData == null)
                return null;

            object controllerName;
            routeData.Values.TryGetValue("controller", out controllerName);

            return controllerName as string;
        }
    }
}
