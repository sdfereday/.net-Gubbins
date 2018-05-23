using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using FluentValidation;
using ClientList.Common.Data;
using ClientList.Features.Client.Models;

namespace ClientList.Features.Client
{
    /// <summary>
    /// Gets a single client by its ID and returns related data, allows for edit on post.
    /// </summary>
    public class Edit
    {
        // With a validator added, this will be included in the various requests
        // before anything further can happen and define the model state going forward
        // for the controller to observe.
        public class EditClientValidator : AbstractValidator<EditClientViewModel>
        {
            public EditClientValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("A name is required.");
                RuleFor(x => x.Name).Matches("^[a-zA-Z]+$").WithMessage("Must only contain letters.");
            }
        }

        // Query object passed from controller on request as type of GetClientViewModel ->
        // - You can really pass anything here, for it's what is returned from the query.
        // - In some instances you won't even need a query (such as 'creating').
        // - More to that, you may not even need a post type if it's just to return a list.
        // This is all specced out in the handlers anyway, so you add what you need.
        public class EditClientQuery : IRequest<EditClientViewModel>
        {
            public Guid Id { get; set; }
        }

        // Returned view model object after query complete, again returns as type EditClientViewModel
        // so try not to get confused here, it's just specifying the return type. <-
        public class EditClientViewModel : IRequest<EditClientViewModel>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        // The handler responsible for the getting the requested data from and to the controller <-->
        // First type is the query passed on request, the second is the result returned once done.
        // <Received Type, Return Type>
        public class GetClientQueryHandler : IRequestHandler<EditClientQuery, EditClientViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public GetClientQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<EditClientViewModel> Handle(EditClientQuery editClientQuery, CancellationToken cancellationToken)
            {
                var client = await this._context.FindAsync<ClientModel>(editClientQuery.Id);
                return this._mapper.Map<ClientModel, EditClientViewModel>(client);
            }
        }

        public class EditClientQueryHandler : IRequestHandler<EditClientViewModel, EditClientViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public EditClientQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<EditClientViewModel> Handle(EditClientViewModel editClientViewModel, CancellationToken cancellationToken)
            {
                var modelMapping = this._mapper.Map<ClientModel>(editClientViewModel);

                _context.Clients.Update(modelMapping);
                await _context.SaveChangesAsync();

                return this._mapper.Map<ClientModel, EditClientViewModel>(modelMapping);
            }
        }
    }
}
