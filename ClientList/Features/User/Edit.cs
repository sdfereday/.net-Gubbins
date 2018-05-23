using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using FluentValidation;
using ClientList.Common.Data;
using ClientList.Features.User.Models;

namespace UserList.Features.User
{
    /// <summary>
    /// Gets a single User by its ID and returns related data, allows for edit on post.
    /// </summary>
    public class Edit
    {
        // With a validator added, this will be included in the various requests
        // before anything further can happen and define the model state going forward
        // for the controller to observe.
        public class EditUserValidator : AbstractValidator<EditUserViewModel>
        {
            public EditUserValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("A name is required.");
                RuleFor(x => x.FirstName).Matches("^[a-zA-Z]+$").WithMessage("Must only contain letters.");
            }
        }

        // Query object passed from controller on request as type of GetUserViewModel ->
        // - You can really pass anything here, for it's what is returned from the query.
        // - In some instances you won't even need a query (such as 'creating').
        // - More to that, you may not even need a post type if it's just to return a list.
        // This is all specced out in the handlers anyway, so you add what you need.
        public class EditUserQuery : IRequest<EditUserViewModel>
        {
            public Guid Id { get; set; }
        }

        // Returned view model object after query complete, again returns as type EditUserViewModel
        // so try not to get confused here, it's just specifying the return type. <-
        public class EditUserViewModel : IRequest<EditUserViewModel>
        {
            //public Guid ClientId { get; set; }
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        // The handler responsible for the getting the requested data from and to the controller <-->
        // First type is the query passed on request, the second is the result returned once done.
        // <Received Type, Return Type>
        public class GetUserQueryHandler : IRequestHandler<EditUserQuery, EditUserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public GetUserQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<EditUserViewModel> Handle(EditUserQuery editUserQuery, CancellationToken cancellationToken)
            {
                var User = await this._context.FindAsync<UserModel>(editUserQuery.Id);
                return this._mapper.Map<UserModel, EditUserViewModel>(User);
            }
        }

        public class EditUserQueryHandler : IRequestHandler<EditUserViewModel, EditUserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public EditUserQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<EditUserViewModel> Handle(EditUserViewModel editUserViewModel, CancellationToken cancellationToken)
            {
                var modelMapping = this._mapper.Map<UserModel>(editUserViewModel);

                _context.Users.Update(modelMapping);
                await _context.SaveChangesAsync();

                return this._mapper.Map<UserModel, EditUserViewModel>(modelMapping);
            }
        }
    }
}
