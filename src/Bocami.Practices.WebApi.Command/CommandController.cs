using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bocami.Practices.Command;

namespace Bocami.Practices.WebApi.Command
{
    public class CommandController<TCommand> : ApiController
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;

        public CommandController(ICommandHandler<TCommand> commandHandler)
        {
            if (commandHandler == null)
                throw new ArgumentNullException("commandHandler");

            this.commandHandler = commandHandler;
        }

        public HttpResponseMessage Post(TCommand command)
        {
            try
            {
                commandHandler.Handle(command);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (UnauthorizedAccessException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            catch (InvalidOperationException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
    }
}