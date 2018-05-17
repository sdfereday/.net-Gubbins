using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientList.Models;
using ClientList.ViewModels;
using ClientList.Mapping;
using AutoMapper;
namespace ClientList.Controllers
{
    public class ClientController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ClientController(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
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

        public IActionResult Index()
        {
            var clientMapping = this._mapper.Map<IEnumerable<ClientViewModel>>(_context.Clients);
            return View(clientMapping);
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
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public IActionResult EditList()
        {
            var mappingWithUsers = new List<ClientViewModel>();

            foreach(var client in _context.Clients)
            {
                var mappedClient = GetClientWithUsers(client.Id);
                mappingWithUsers.Add(mappedClient);
            }
            
            return View(mappingWithUsers);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return RedirectToAction("EditList");
        }

        [HttpGet("Client/Edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            var clientModel = GetClientById(id);

            if(clientModel == null)
            {
                return View("EditList");
            }

            var clientViewModel = this._mapper.Map<ClientViewModel>(clientModel);
            return View(clientViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name")] ClientViewModel client)
        {
            var modelMapping = this._mapper.Map<ClientModel>(client);

            _context.Clients.Update(modelMapping);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditList");
        }
    }
}