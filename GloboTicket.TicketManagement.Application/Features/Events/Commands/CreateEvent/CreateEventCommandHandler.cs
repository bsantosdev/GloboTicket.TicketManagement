using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
    {
        private readonly IEventRepository EventRepository;
        private readonly IMapper Mapper;

        public CreateEventCommandHandler(
            IMapper mapper, 
            IEventRepository eventRepository)
        {
            Mapper = mapper;
            EventRepository = eventRepository;
        }

        public async Task<Guid> Handle(
            CreateEventCommand request, 
            CancellationToken cancellationToken)
        {
            Event itemToAdd = Mapper.Map<Event>(request);

            itemToAdd = await EventRepository.AddAsync(itemToAdd);

            return itemToAdd.EventId;
        }
    }
}
