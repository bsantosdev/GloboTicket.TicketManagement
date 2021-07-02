using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListQueryHandler
        : IRequestHandler<GetCategoriesListQuery, List<CategoryListVm>>
    {
        private readonly IAsyncRepository<Category> CategoryRepository;
        private readonly IMapper Mapper;

        public GetCategoriesListQueryHandler(
            IMapper mapper,
            IAsyncRepository<Category> categoryRepository)
        {
            Mapper = mapper;
            CategoryRepository = categoryRepository;
        }

        public async Task<List<CategoryListVm>> Handle(
            GetCategoriesListQuery request,
            CancellationToken cancellationToken)
        {
            IOrderedEnumerable<Category> allCategories = (await CategoryRepository
                .ListAllAsync()).OrderBy(x => x.Name);

            return Mapper.Map<List<CategoryListVm>>(allCategories);
        }
    }
}
