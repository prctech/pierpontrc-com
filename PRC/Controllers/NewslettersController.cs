using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRC.Data;
using PRC.Models;
using PRC.Models.ViewModels;

namespace PRC.Controllers
{
    [Authorize(Roles = "admin, editor")]
    public class NewslettersController : Controller
    {
        private readonly ApplicationDbContext _context;
        const string BLOB = "https://pierpontrcassets.blob.core.windows.net/website/";
        const string XSTR = "DefaultEndpointsProtocol=https;AccountName=pierpontrcassets;AccountKey=3gG7N0e/zeIrW4ka6vc/KpuPY45B2FeBcQ+x/3i9iwdQPHwa+B8KBwFdVKk2/QKoIB6esQPkQtESyOdvOdBzWQ==;EndpointSuffix=core.windows.net";
        const string NEWS = "news";

        public NewslettersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("news/archives")]
        [AllowAnonymous]
        public async Task<IActionResult> Archive()
        {
            return View(await _context.Newsletters.ToListAsync());
        }

        // GET: Newsletters/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsletter == null)
            {
                return NotFound();
            }

            return View(newsletter);
        }

        [Route("admin/newsletters")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Newsletters.OrderByDescending(n => n.Date).ToListAsync());
        }

        // GET: Newsletters/Create
        public IActionResult Create()
        {
            var model = new NewsletterViewModel
            {
                Date = DateTime.Now    
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Title")] Newsletter newsletter,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                newsletter.Url = uploadFile(file);
                newsletter.Id = Guid.NewGuid();
                _context.Add(newsletter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(newsletter);
        }

        // GET: Newsletters/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletters.FindAsync(id);
            if (newsletter == null)
            {
                return NotFound();
            }

            var model = new NewsLetterEditViewModel
            {
                Id = newsletter.Id,
                Date = newsletter.Date,
                Title = newsletter.Title,
                Url = newsletter.Url
            };

            return View(model);
        }

        // POST: Newsletters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Date,Title,Url")] Newsletter newsletter)
        {
            if (id != newsletter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsletter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsletterExists(newsletter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newsletter);
        }

        // GET: Newsletters/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsletter == null)
            {
                return NotFound();
            }

            return View(newsletter);
        }

        // POST: Newsletters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var newsletter = await _context.Newsletters.FindAsync(id);
            _context.Newsletters.Remove(newsletter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        private bool NewsletterExists(Guid id)
        {
            return _context.Newsletters.Any(e => e.Id == id);
        }

        private string uploadFile(IFormFile file)
        {
            string connectionString = XSTR;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("website");
            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            // Open the file and upload its data
            blobClient.Upload(file.OpenReadStream(), true);

            //return RedirectToAction(nameof(Index));
            return BLOB + file.FileName;
        }
    }
}
