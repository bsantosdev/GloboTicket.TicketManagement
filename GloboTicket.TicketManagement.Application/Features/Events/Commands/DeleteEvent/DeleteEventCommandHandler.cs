using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IAsyncRepository<Event> EventRepository;
        //private readonly IMapper Mapper; // It should be there?

        public DeleteEventCommandHandler(
               IMapper mapper,
               IEventRepository eventRepository)
        {
            //Mapper = mapper;
            EventRepository = eventRepository;
        }

        public async Task<Unit> Handle(
            DeleteEventCommand request, 
            CancellationToken cancellationToken)
        {
            Event itemToDelete = await EventRepository.GetByIdAsync(request.EventId);

            await EventRepository.DeleteAsync(itemToDelete);

            return Unit.Value;
        }
    }
}
