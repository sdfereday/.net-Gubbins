using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;

namespace UserList.Features.User.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.CreateUserViewModel createUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserViewModel);
            }

            var User = await this._mediator.Send(createUserViewModel);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Edit.EditUserQuery editUserQuery)
        {
            var User = await this._mediator.Send(editUserQuery);
            return View(User);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Edit.EditUserViewModel editUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserViewModel);
            }

            var User = await this._mediator.Send(editUserViewModel);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(List.ListUserQuery listUserQuery)
        {
            var Users = await this._mediator.Send(listUserQuery);
            return View(Users);
        }
    }
}