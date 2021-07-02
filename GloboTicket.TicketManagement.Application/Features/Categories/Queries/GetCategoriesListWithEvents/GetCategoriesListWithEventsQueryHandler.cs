using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents
{
    public class GetCategoriesListWithEventsQueryHandler 
        : IRequestHandler<GetCategoriesListWithEventsQuery, List<CategoryEventListVm>>
    {
        private readonly IMapper Mapper;
        private readonly ICategoryRepository CategoryRepository;

        public GetCategoriesListWithEventsQueryHandler(
            IMapper mapper,
            ICategoryRepository categoryRepository)
        {
            Mapper = mapper;
            CategoryRepository = categoryRepository;
        }

        public async Task<List<CategoryEventListVm>> Handle(
            GetCategoriesListWithEventsQuery request, 
            CancellationToken cancellationToken)
        {
            List<Category> list = await CategoryRepository
                .GetCategoriesWithEvents(request.IncludeHistory);

            return Mapper.Map<List<CategoryEventListVm>>(list);
        }
    }
}
