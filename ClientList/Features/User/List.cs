using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using ClientList.Common.Data;

namespace UserList.Features.User
{
    /// <summary>
    /// Returns a list of Users from DB context
    /// </summary>
    public class List
    {
        public class ListUserQuery : IRequest<ListUserResultViewModel>
        {
        }

        public class ListUserResultViewModel
        {
            public IEnumerable<ListUserViewModel> Users { get; set; }
        }

        public class ListUserViewModel
        {
            //public Guid ClientId { get; set; }
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class ListUserQueryHandler : IRequestHandler<ListUserQuery, ListUserResultViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public ListUserQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<ListUserResultViewModel> Handle(ListUserQuery UserQuery, CancellationToken cancellationToken)
            {
                // I wouldn't normally do this, but I figured since the request is CPU-bound, I just run the task manually.
                var UserViewModels = await Task.Run(() => this._mapper.Map<IEnumerable<ListUserViewModel>>(_context.Users));
                var result = new ListUserResultViewModel()
                {
                    Users = UserViewModels
                };

                return result;
            }
        }
    }
}
