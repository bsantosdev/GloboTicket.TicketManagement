using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQueryHandler
        : IRequestHandler<GetEventDetailQuery, EventDetailVm>
    {
        private readonly IAsyncRepository<Event> EventRepository;
        private readonly IAsyncRepository<Category> CategoryRepository;
        private readonly IMapper Mapper;

        public GetEventDetailQueryHandler(
            IMapper mapper,
            IAsyncRepository<Event> eventRepository,
            IAsyncRepository<Category> categoryRepository)
        {
            Mapper = mapper;
            EventRepository = eventRepository;
            CategoryRepository = categoryRepository;
        }

        public async Task<EventDetailVm> Handle(
            GetEventDetailQuery request,
            CancellationToken cancellationToken)
        {
            Event item = await EventRepository.GetByIdAsync(request.Id);
            EventDetailVm eventDetailDto = Mapper.Map<EventDetailVm>(item);

            Category category = await CategoryRepository.GetByIdAsync(item.CategoryId);
            eventDetailDto.Category = Mapper.Map<CategoryDto>(category);

            return eventDetailDto;
        }
    }
}
