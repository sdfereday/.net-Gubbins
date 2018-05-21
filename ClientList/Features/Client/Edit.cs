using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ClientList.Common.Data;
using ClientList.Features.Client.Models;

namespace ClientList.Features.Client
{
    /// <summary>
    /// Gets a single client by its ID and returns related data, allows for edit on post.
    /// </summary>
    public class Edit
    {
        // Query object passed from controller on request as type of GetClientViewModel ->
        public class GetClientQuery : IRequest<GetClientViewModel>
        {
            public Guid Id { get; set; }
        }

        // Returned view model object after query complete, again returns as type GetClientViewModel <-
        public class GetClientViewModel : IRequest<GetClientViewModel>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        // The handler responsible for the getting the requested data from and to the controller <-->
        // First type is the query passed on request, the second is the result returned once done.
        public class GetClientQueryHandler : IRequestHandler<GetClientQuery, GetClientViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public GetClientQueryHandler(DataContext context, IMapper mapper)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                if (mapper == null)
                {
                    throw new ArgumentNullException("mapper");
                }

                this._context = context;
                this._mapper = mapper;
            }

            // Should I be using this type on the razor page?
            public async Task<GetClientViewModel> Handle(GetClientQuery request, CancellationToken cancellationToken)
            {
                var client = await this._context.FindAsync<ClientModel>(request.Id);
                return this._mapper.Map<ClientModel, GetClientViewModel>(client);
            }
        }
    }
}
