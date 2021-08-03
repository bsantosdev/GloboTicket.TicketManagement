using AutoMapper;
using FluentValidation.Results;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler 
        : IRequestHandler<UpdateEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public UpdateEventCommandHandler(
            IMapper mapper,
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<Unit> Handle(
            UpdateEventCommand request, 
            CancellationToken cancellationToken)
        {
            Event eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId);

            if (eventToUpdate == null)
            {
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            UpdateEventCommandValidator validator = new UpdateEventCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult);

            _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));

            await _eventRepository.UpdateAsync(eventToUpdate);

            return Unit.Value;
        }
    }
}
