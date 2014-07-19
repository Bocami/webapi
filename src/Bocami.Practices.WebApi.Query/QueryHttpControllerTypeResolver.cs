using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dispatcher;
using Bocami.Practices.Query;

namespace Bocami.Practices.WebApi.Query
{
    public class QueryHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        private const string QueryTypePostfix = "Query";
        private const string QueryResultTypePostfix = "QueryResult";

        private class QueryTypeIdentifier
        {
            public QueryTypeIdentifier(Type type, Type @interface)
            {
                Type = type;
                Interface = @interface;
            }

            public QueryTypeIdentifier(string key, QueryTypeIdentifier baseIdentifier)
            {
                Key = key;
                Type = baseIdentifier.Type;
                Interface = baseIdentifier.Interface;
            }

            public string Key { get; private set; }
            public Type Type { get; private set; }
            public Type Interface { get; private set; }
        }

        public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            var allTypes = assembliesResolver.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .SelectMany(type => type.GetInterfaces(), (type, @interface) => new QueryTypeIdentifier(type, @interface))
                .ToList();

            var queryTypes = allTypes
                .Where(a => a.Interface == typeof(IQuery))
                .Select(a => new QueryTypeIdentifier(a.Type.Name.Remove(a.Type.Name.LastIndexOf(QueryTypePostfix, StringComparison.InvariantCulture)), a));

            var queryResultTypes = allTypes
                .Where(a => a.Interface == typeof(IQueryResult))
                .Select(a => new QueryTypeIdentifier(a.Type.Name.Remove(a.Type.Name.LastIndexOf(QueryResultTypePostfix, StringComparison.InvariantCulture)), a));

            return queryTypes
                .Join(
                    queryResultTypes,
                    query => query.Key,
                    result => result.Key,
                    (query, result) => typeof(QueryController<,>).MakeGenericType(query.Type, result.Type))
                .ToList();
        }
    }
}