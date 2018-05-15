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

        public IActionResult Index()
        {
            return View();
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
            return RedirectToPage("/Client");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientViewModel client, Guid id)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var modelMapping = this._mapper.Map<ClientModel>(client);

            _context.Clients.Update(modelMapping);
            await _context.SaveChangesAsync();
            return RedirectToPage("client");
        }
    }
}