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
    public class Create
    {
        public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
        {
            public CreateUserValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("A name is required.");
                RuleFor(x => x.FirstName).Matches("^[a-zA-Z]+$").WithMessage("Must only contain letters.");
            }
        }

        public class CreateUserViewModel : IRequest<CreateUserViewModel>
        {
            //public Guid ClientId { get; set; }
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class CreateUserQueryHandler : IRequestHandler<CreateUserViewModel, CreateUserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public CreateUserQueryHandler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<CreateUserViewModel> Handle(CreateUserViewModel createUserViewModel, CancellationToken cancellationToken)
            {
                var modelMapping = this._mapper.Map<UserModel>(createUserViewModel);

                _context.Users.Add(modelMapping);
                await _context.SaveChangesAsync();

                return this._mapper.Map<CreateUserViewModel>(modelMapping);
            }
        }
    }
}