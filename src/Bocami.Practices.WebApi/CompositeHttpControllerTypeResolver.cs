using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dispatcher;

namespace Bocami.Practices.WebApi
{
    public class CompositeHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        private readonly IHttpControllerTypeResolver[] httpControllerTypeResolvers;

        public CompositeHttpControllerTypeResolver(params IHttpControllerTypeResolver[] httpControllerTypeResolvers)
        {
            if (httpControllerTypeResolvers == null)
                throw new ArgumentNullException("httpControllerTypeResolvers");

            this.httpControllerTypeResolvers = httpControllerTypeResolvers;
        }

        public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            return this.httpControllerTypeResolvers
                .SelectMany(a => a.GetControllerTypes(assembliesResolver))
                .Distinct()
                .ToList();
        }
    }
}
