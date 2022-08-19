using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRC.Data;
using PRC.Models;
using PRC.Models.ViewModels;
using PRC.Helpers;
using Microsoft.EntityFrameworkCore.Internal;

namespace PRC.Controllers
{
    
    class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
    [Authorize(Roles = "admin, editor")]
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string NOMENID = "00000000-0000-0000-0000-000000000000";
        private const string HOMEURL = "home";
        private const string NEWSURL = "news";
        private const string PHOTURL = "photo-gallery";
        private const string CONTURL = "contact";

        private string _apiKey { get; set; }
        private string _privateKeyFile { get; set; }
        private bool _scopeUser { get; set; }
        private User[] _users { get; set; }

        public PagesController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            var opts = config.GetSection("TinyDrive");
            _apiKey = opts["apiKey"];
            _privateKeyFile = opts["privateKeyFile"];
            _scopeUser = Boolean.Parse(opts["scopeUser"]);
            _users = opts.GetSection("users")
              .GetChildren()
              .Select(user => new User { UserName = "prc", Password = "P1erpont!", FullName = "Pierpont Racquet Club" })
              .ToArray();
        }

        // GET: Pages
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Url == HOMEURL);

            var motd = await _context.Motd.FirstOrDefaultAsync();

            if (page == null)
            {
                return NotFound();
            }

            var slides = await _context.Slideshow.OrderByDescending(s => s.UploadDate).ToListAsync();

            var model = new HomeViewModel
            {
                Id = page.Id,
                Content = page.Content,
                Url = page.Url,
                Name = page.Name,
                HasContact = page.Contact,
                HasSubPages = page.SubPages,
                MenuId = page.MenuId,
                ParentPage = page.ParentPage,
                Slides = slides,
                MotdId = motd.Id,
                Motd = motd.Content
            };

            return View(model);
        }

        // GET: Pages/Index/contact
        [HttpGet]
        [Route("{url}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string url)
        {
            if (url == NEWSURL)
                return RedirectToAction(nameof(News));
            
            if(url == PHOTURL)
                return RedirectToAction(nameof(Photos));

            if (url == CONTURL)
                return RedirectToAction(nameof(Contact));

            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Url == url);

            var pages = new List<Page>();

            if (page.SubPages)
            {
                // Get a list of sub pages for the menu, but omit the parent page.
                pages = await _context.Pages.Where(p => p.MenuId == page.MenuId && p.ParentPage.ToString() != NOMENID)
                    .OrderBy(p => p.Name).ToListAsync();
            }   

            var model = new PageViewModel
            {
                Id = page.Id,
                Content = page.Content,
                Url = page.Url,
                Name = page.Name,
                HasContact = page.Contact,
                HasSubPages = page.SubPages,
                MenuId = page.MenuId,
                ParentPage = page.ParentPage,
                SubPages = pages
            };

            return View(model);
        }

        
        [Route("admin/pages")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Pages.OrderBy(p => p.Name).ToListAsync());
        }

        // GET: Pages/Create
        public IActionResult Create()
        {
            var menus = _context.Menu.OrderBy(m => m.Name).ToList();

            var model = new PageEditViewModel
            {
                Menus = menus
            };

            return View(model);
        }

        // POST: Pages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Content,Url,SubPages,MenuId,ParentPage")] PageEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var menu = await _context.Menu.FirstOrDefaultAsync(m => m.Id == model.MenuId);
                    model.ParentPage = menu.ParentId;
                }
                catch (Exception) { }

                var page = new Page
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Content = model.Content,
                    Url = model.Url,
                    SubPages = model.SubPages,
                    MenuId = model.MenuId,
                    ParentPage = model.ParentPage
                };

                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            return View();
        }

        public IActionResult Update([FromBody]PageViewModel model)
        {
            var page = new Page
            {
                Id = model.Id,
                Name = model.Name,
                Content = model.Content,
                Url = model.Url,
                ParentPage = model.ParentPage,
                MenuId = model.MenuId,
                SubPages = model.HasSubPages,
                Contact = model.HasContact,
            };

            _context.Update(page);
            _context.SaveChanges();

            return Json("ok");
        }

        [AllowAnonymous]
        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public JsonResult Jwt()
        {
            var username = "prc";
            var fullname = "Pierpont Racquet Club";

            if (username != null)
            {
                var token = JwtHelper.CreateTinyDriveToken(username, fullname, _scopeUser, _privateKeyFile);
                return Json(new { token });
            }
            else
            {
                var result = Json(new { error = "Failed to auth." });
                result.StatusCode = 403;
                return result;
            }
        }

        // GET: Pages/Edit/About
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Id == id);

            if (page == null)
            {
                return NotFound();
            }

            var menus = _context.Menu.OrderBy(m => m.Name).ToList();
            var model = new PageEditViewModel
            {
                Id = id,
                Name = page.Name,
                Content = page.Content,
                Url = page.Url,
                SubPages = page.SubPages,
                MenuId = page.MenuId,
                Menus = menus,
                ParentPage = page.ParentPage
            };

            return View(model);
        }

        // POST: Pages/Edit/About
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Content,Url,SubPages,MenuId,ParentPage")] PageEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var page = new Page
                    {
                        Id = id,
                        Name = model.Name,
                        Content = model.Content,
                        Url = model.Url,
                        SubPages = model.SubPages,
                        MenuId = model.MenuId,
                        ParentPage = model.ParentPage
                    };

                    _context.Update(page);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }

        // GET: Pages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var page = await _context.Pages.FindAsync(id);
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        // GET: Newsletters
        [Route("news")]
        [AllowAnonymous]
        public async Task<IActionResult> News()
        {
            var letters = await _context.Newsletters.OrderByDescending(n => n.Date).Take(2).ToListAsync();
            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Url == NEWSURL);
            //var urls = new List<string>();

            if (page == null)
            {
                return NotFound();
            }

            var model = new NewsPageViewModel
            {
                PageId = page.Id,
                Content = page.Content,
                Newsletters = letters
            };

            return View(model);
        }

        [Route("photo-gallery")]
        [AllowAnonymous]
        public async Task<IActionResult> Photos()
        {
            var photos = await _context.Photos.ToListAsync();

            return View(photos);
        }

        [Route("contact")]
        [AllowAnonymous]
        public async Task<IActionResult> Contact()
        {
            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Url == CONTURL);

            var model = new ContactViewModel
            {
                Content = page.Content
            };

            return View(model);
        }

        private bool PageExists(Guid id)
        {
            return _context.Pages.Any(e => e.Id == id);
        }
    }
}
