using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using ClientList.Common.Data;
using ClientList.Features.Client.Models;

namespace ClientList.Features.Client
{
    /// <summary>
    /// Returns a list of clients from DB context
    /// </summary>
    public class Create
    {
        public class CreateClientViewModel : IRequest<CreateClientViewModel>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class CreateClientQueryHandler : IRequestHandler<CreateClientViewModel, CreateClientViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public CreateClientQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<CreateClientViewModel> Handle(CreateClientViewModel createClientViewModel, CancellationToken cancellationToken)
            {
                var modelMapping = this._mapper.Map<ClientModel>(createClientViewModel);

                _context.Clients.Add(modelMapping);
                await _context.SaveChangesAsync();

                return this._mapper.Map<CreateClientViewModel>(modelMapping);
            }
        }
    }
}
