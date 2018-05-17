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
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserController(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public UserModel GetUserById(Guid Id)
        {
            return _context.Users
                .ToList()
                .Find(x => x.Id == Id);
        }

        public IActionResult Index()
        {
            var userMapping = this._mapper.Map<IEnumerable<UserViewModel>>(_context.Users);
            return View(userMapping);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Hmm...
            return View(new UserViewModel()
            {
                Clients = this._mapper.Map<IEnumerable<ClientViewModel>>(_context.Clients)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelMapping = this._mapper.Map<UserModel>(user);

            _context.Users.Add(modelMapping);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public IActionResult EditList()
        {
            var userMapping = this._mapper.Map<IEnumerable<UserViewModel>>(_context.Users);
            return View(userMapping);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return RedirectToAction("EditList");
        }

        [HttpGet("User/Edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            var userModel = GetUserById(id);

            if (userModel == null)
            {
                return View("EditList");
            }


            var userViewModel = this._mapper.Map<UserViewModel>(userModel);
            userViewModel.Clients = this._mapper.Map<IEnumerable<ClientViewModel>>(_context.Clients);

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, ClientId, FirstName, LastName, Email")] UserViewModel user)
        {
            var modelMapping = this._mapper.Map<UserModel>(user);

            _context.Users.Update(modelMapping);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditList");
        }
    }
}