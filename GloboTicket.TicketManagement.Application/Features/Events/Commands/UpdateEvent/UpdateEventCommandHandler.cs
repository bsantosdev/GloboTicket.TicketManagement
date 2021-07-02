using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler 
        : IRequestHandler<UpdateEventCommand>
    {
        private readonly IEventRepository EventRepository;
        private readonly IMapper Mapper;

        public UpdateEventCommandHandler(
            IMapper mapper,
            IEventRepository eventRepository)
        {
            Mapper = mapper;
            EventRepository = eventRepository;
        }

        public async Task<Unit> Handle(
            UpdateEventCommand request, 
            CancellationToken cancellationToken)
        {
            Event itemToUpdate = await EventRepository.GetByIdAsync(request.EventId);

            Mapper.Map(request, itemToUpdate, typeof(UpdateEventCommand), typeof(Event));

            await EventRepository.UpdateAsync(itemToUpdate);

            return Unit.Value;
        }
    }
}
