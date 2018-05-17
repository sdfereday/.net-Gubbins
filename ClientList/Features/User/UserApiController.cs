using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ClientList.Models;
using ClientList.ViewModels;
using ClientList.Mapping;
using AutoMapper;

namespace ClientList
{
    [Route("api/[controller]")]
    public class UserApiController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserApiController(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public class Result
        {
            public IEnumerable<UserViewModel> Items { get; set; }
        }

        public UserModel GetUserById(Guid Id)
        {
            return _context.Users
                .ToList()
                .Find(x => x.Id == Id);
        }

        // Unsure if this is a good idea, it feels like it's logic that should be somewhere else...
        public bool ClientExists(Guid Id)
        {
            return _context.Clients
                .ToList()
                .Any(x => x.Id == Id);
        }
        
        [HttpGet("/api/users/")]
        public Result Get()
        {
            List<UserModel> users = _context.Users.ToList();
            IEnumerable<UserViewModel> userMapping = this._mapper.Map<IEnumerable<UserViewModel>>(users);

            return new Result()
            {
                Items = userMapping
            };
        }
        
        [HttpGet("/api/users/{id}")]
        public UserViewModel Get(Guid id)
        {
            var userModel = GetUserById(id);
            return this._mapper.Map<UserViewModel>(userModel);
        }

        [HttpPost("/api/users/create")]
        public IActionResult Create([FromBody] UserViewModel userData)
        {
            if (userData == null || userData.ClientId == null || userData.ClientId == Guid.Empty)
            {
                return BadRequest();
            }

            if(!ClientExists(userData.ClientId))
            {
                return NotFound();
            }

            var modelMapping = this._mapper.Map<UserModel>(userData);

            _context.Users.Add(modelMapping);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("/api/users/update/{id}")]
        public IActionResult Update(Guid id, [FromBody] UserViewModel userData)
        {
            if (userData == null || id == null || id == Guid.Empty || userData.Id != id)
            {
                return BadRequest();
            }

            var existingModel = GetUserById(id);

            if (existingModel == null)
            {
                return NotFound();
            }

            var newModel = this._mapper.Map<UserModel>(userData);

            // _context.Entry(existingModel).CurrentValues.SetValues(userData);
            _context.Users.Update(newModel);
            _context.SaveChanges();
            return Ok();
        }
    }
}
