﻿using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQueryHandler
        : IRequestHandler<GetEventsListQuery, List<EventListVm>>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public GetEventsListQueryHandler(
            IMapper mapper,
            IAsyncRepository<Event> eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<List<EventListVm>> Handle(
            GetEventsListQuery request,
            CancellationToken cancellationToken)
        {
            IOrderedEnumerable<Event> allEvents = (await _eventRepository
                .ListAllAsync()).OrderBy(x => x.Date);

            return _mapper.Map<List<EventListVm>>(allEvents);
        }
    }
}
