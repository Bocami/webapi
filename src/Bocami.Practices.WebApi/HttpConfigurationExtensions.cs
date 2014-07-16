using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Bocami.Practices.WebApi
{
    public static class HttpConfigurationExtensions 
    {
        public static void RegisterCompositeHttpControllerTypeResolver(this HttpConfiguration configuration, params IHttpControllerTypeResolver[] httpControllerTypeResolvers)
        {
            configuration.Services.Replace(typeof(IHttpControllerTypeResolver), new CompositeHttpControllerTypeResolver(httpControllerTypeResolvers));
        }

        public static void RegisterCompositeHttpControllerSelector(this HttpConfiguration configuration, params IHttpControllerSelector[] httpControllerSelectors)
        {
            configuration.Services.Replace(typeof(IHttpControllerSelector), new CompositeHttpControllerSelector(httpControllerSelectors));
        }
    }
}