using AutoMapper;
using FluentValidation.Results;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateEventCommandHandler> _logger;

        public CreateEventCommandHandler(
            IMapper mapper,
            IEventRepository eventRepository,
            IEmailService emailService,
            ILogger<CreateEventCommandHandler> logger)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _emailService = emailService;
            _logger = logger;
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

            Email email = new Email() 
            { 
                To = "brunosantos.dev@outlook.com", 
                Body = $"A new event was created: {request}", 
                Subject = "A new event was created" 
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                // this shoudn't stop the API from doing else so this can be logged
                _logger.LogError($"Mailing about event {itemToAdd.EventId} failed due to an error with the mail service: {ex.Message}");
            }

            return itemToAdd.EventId;
        }
    }
}
