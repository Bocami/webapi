using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bocami.Practices.Query;

namespace Bocami.Practices.WebApi.Query
{
    public class QueryController<TQuery, TQueryResult> : ApiController
        where TQuery : IQuery
        where TQueryResult : IQueryResult
    {
        private readonly IQueryHandler<TQuery, TQueryResult> queryHandler;

        public QueryController(IQueryHandler<TQuery, TQueryResult> queryHandler)
        {
            if (queryHandler == null)
                throw new ArgumentNullException("queryHandler");

            this.queryHandler = queryHandler;
        }

        public HttpResponseMessage Get([FromUri(SuppressPrefixCheck = true)]TQuery model)
        {
            try
            {
                var queryResult = queryHandler.Handle(model);

                return Request.CreateResponse(queryResult);
            }
            catch (InvalidOperationException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            catch (UnauthorizedAccessException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
    }
}