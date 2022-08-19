using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRC.Data;
using PRC.Models;
using PRC.Models.ViewModels;

namespace PRC.Controllers
{
    public class MotdController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotdController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Update([FromBody]MotdUpdateViewModel model)
        {
            var motd = new Motd
            { 
                Id = model.Id,
                Content = model.Content
            };

            _context.Update(motd);
            _context.SaveChanges();

            return Json("ok");
        }
    }
}