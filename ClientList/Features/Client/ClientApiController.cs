using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ClientList.Common.Data;
using ClientList.Features.Client.Models;
using ClientList.Features.Client.ViewModels;
using AutoMapper;

namespace ClientList.Features.Client.Controllers
{
    [Route("api/[controller]")]
    public class ClientApiController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ClientApiController(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public class Result
        {
            public IEnumerable<ClientViewModel> Items { get; set; }
        }

        public ClientModel GetClientById(Guid Id)
        {
            return _context.Clients
                .ToList()
                .Find(x => x.Id == Id);
        }

        [HttpGet("/api/clients/")]
        public Result Get ()
        {
            List<ClientModel> clients = _context.Clients.ToList();
            IEnumerable<ClientViewModel> clientMapping = this._mapper.Map<IEnumerable<ClientViewModel>>(clients);

            return new Result()
            {
                Items = clientMapping
            };
        }

        [HttpGet("/api/clients/{id}")]
        public ClientViewModel Get(Guid id)
        {
            var clientModel = GetClientById(id);
            return this._mapper.Map<ClientViewModel>(clientModel);
        }
        
        [HttpPost("/api/clients/update/{id}")]
        public IActionResult Update(Guid id, [FromBody] ClientViewModel clientData)
        {
            if (clientData == null || id == null || id == Guid.Empty || clientData.Id != id)
            {
                return BadRequest();
            }

            var existingModel = GetClientById(clientData.Id);

            if (existingModel == null)
            {
                return NotFound();
            }

            var newModel = this._mapper.Map<ClientModel>(clientData);

            // _context.Entry(existingClient).CurrentValues.SetValues(clientData);
            _context.Clients.Update(newModel);
            _context.SaveChanges();
            return Ok();
        }
    }
}
