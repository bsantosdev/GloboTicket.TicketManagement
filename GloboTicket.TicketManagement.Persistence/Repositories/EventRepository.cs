﻿using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(GloboTicketDbContext dbContext) 
            : base(dbContext) { }

        public Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
        {
            bool matches = _dbContext.Events
                .Any(e=> e.Name.Equals(name) && e.Date.Date.Equals(eventDate.Date));

            return Task.FromResult(matches);
        }
    }
}