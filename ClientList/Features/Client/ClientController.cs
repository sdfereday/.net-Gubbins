using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;

namespace ClientList.Features.Client.Controllers
{
    public class ClientController : Controller
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.CreateClientViewModel createClientViewModel)
        {
            var client = await this._mediator.Send(createClientViewModel);
            return RedirectToAction("List");
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Edit.EditClientQuery editClientQuery)
        {
            var client = await this._mediator.Send(editClientQuery);
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Edit.EditClientViewModel editClientViewModel)
        {
            var client = await this._mediator.Send(editClientViewModel);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(List.ListClientQuery listClientQuery)
        {
            var clients = await this._mediator.Send(listClientQuery);
            return View(clients);
        }
    }
}