using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dispatcher;
using Bocami.Practices.Command;

namespace Bocami.Practices.WebApi.Command
{
    public class CommandHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            return assembliesResolver.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .SelectMany(type => type.GetInterfaces(), (type, @interface) => new { type, @interface })
                .Where(a => a.@interface == typeof(ICommand))
                .Select(a => typeof(CommandController<>).MakeGenericType(a.type))
                .ToList();
        }
    }
}