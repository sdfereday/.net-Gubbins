using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using ClientList.Common.Data;
using ClientList.Features.User.ViewModels;

namespace ClientList.Features.Client
{
    /// <summary>
    /// Returns a list of clients from DB context
    /// </summary>
    public class List
    {
        public class ListClientQuery : IRequest<ListClientResultViewModel>
        {
        }

        public class ListClientResultViewModel
        {
            public IEnumerable<ListClientViewModel> Clients { get; set; }
        }

        public class ListClientViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class ListClientQueryHandler : IRequestHandler<ListClientQuery, ListClientResultViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public ListClientQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<ListClientResultViewModel> Handle(ListClientQuery clientQuery, CancellationToken cancellationToken)
            {
                // I wouldn't normally do this, but I figured since the request is CPU-bound, I just run the task manually.
                var clientViewModels = await Task.Run(() => this._mapper.Map<IEnumerable<ListClientViewModel>>(_context.Clients));
                var result = new ListClientResultViewModel()
                {
                    Clients = clientViewModels
                };

                return result;
            }
        }
    }
}
