using AutoMapper;
using FluentValidation.Results;
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
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public CreateEventCommandHandler(
            IMapper mapper, 
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<Guid> Handle(
            CreateEventCommand request, 
            CancellationToken cancellationToken)
        {
            ValidationResult validatorResult = await new CreateEventCommandValidator(_eventRepository)
                .ValidateAsync(request);    

            if (validatorResult.Errors.Count > 0)
            {
                throw new Exceptions.ValidationException(validatorResult);
            }

            Event itemToAdd = _mapper.Map<Event>(request);

            itemToAdd = await _eventRepository.AddAsync(itemToAdd);

            return itemToAdd.EventId;
        }
    }
}
