using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientList.Features.Client.Models;
using ClientList.Features.Client.ViewModels;
using ClientList.Features.User.ViewModels;
using ClientList.Common.Data;
using AutoMapper;
using MediatR;

namespace ClientList.Features.Client.Controllers
{
    public class ClientController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ClientController(DataContext context, IMapper mapper, IMediator mediator)
        {
            this._context = context;
            this._mapper = mapper;
            this._mediator = mediator;
        }

        public ClientModel GetClientById(Guid Id)
        {
            return _context.Clients
                .ToList()
                .Find(x => x.Id == Id);
        }

        public ClientViewModel GetClientWithUsers(Guid Id)
        {
            var clientModel = GetClientById(Id);
            var clientMap = this._mapper.Map<ClientViewModel>(clientModel);

            var associatedUsers = _context.Users
                .Where(x => x.ClientId == clientMap.Id)
                .ToList();

            clientMap.Users = _mapper.Map<List<UserViewModel>>(associatedUsers);

            return clientMap;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientViewModel client)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var modelMapping = this._mapper.Map<ClientModel>(client);

            _context.Clients.Add(modelMapping);
            await _context.SaveChangesAsync();

            return RedirectToAction("List");

            // This doesn't work. Why?
            //return RedirectToAction("Edit", new ClientModel() {
            //    Id = client.Id
            //});
        }

        [HttpGet]
        public IActionResult List()
        {
            var mappingWithUsers = new List<ClientViewModel>();

            foreach(var client in _context.Clients)
            {
                var mappedClient = GetClientWithUsers(client.Id);
                mappingWithUsers.Add(mappedClient);
            }
            
            return View(mappingWithUsers);
        }

        // Query from frontend is cast as a Get.GetClientQuery object which is mapped in to the mediator.
        public async Task<IActionResult> Edit(Edit.GetClientQuery getClientQuery)
        {
            if(getClientQuery == null)
            {
                throw new ArgumentNullException("getClientQuery");
            }

            var client = await this._mediator.Send(getClientQuery);

            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name")] ClientViewModel client)
        {
            var modelMapping = this._mapper.Map<ClientModel>(client);

            _context.Clients.Update(modelMapping);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}