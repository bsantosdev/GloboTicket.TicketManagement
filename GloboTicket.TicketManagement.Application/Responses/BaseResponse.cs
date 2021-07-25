using System.Collections.Generic;

namespace GloboTicket.TicketManagement.Application.Responses
{
    // A great approach is to always return the controller responses inheriting from the base response
    // so the client could handle an error in a standard way.
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = true;
        }

        public BaseResponse(string message = null)
        {
            Success = true;
            Message = message;
        }

        public BaseResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}
