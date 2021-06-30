using MediatR;
using System.Collections.Generic;

namespace GloboTicket.TicketManagement.Application.Features.Events
{
    // This class is showing how the mediator pattern should work by using the library MediatR
    public class GetEventsListQuery : IRequest<List<EventListVm>>
    {
    }
}
