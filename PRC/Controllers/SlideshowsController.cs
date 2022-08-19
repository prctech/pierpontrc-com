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
    public class SlideshowsController : Controller
    {
        private readonly ApplicationDbContext _context;
        const string BLOB = "https://pierpontrcassets.blob.core.windows.net/website/";

        public SlideshowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Slideshows
        [Route("admin/slides")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Slideshow.ToListAsync());
        }

        // GET: Slideshows/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Slideshows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url")] Slideshow slideshow, IFormFile file)
        {
            slideshow.Url = uploadFile(file);

            if (ModelState.IsValid)
            {
                slideshow.Id = Guid.NewGuid();
                _context.Add(slideshow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(slideshow);
        }

        // GET: Slideshows/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slideshow = await _context.Slideshow.FindAsync(id);
            if (slideshow == null)
            {
                return NotFound();
            }
            var model = new SlideshowsEditViewModel();
            return View(model);
        }

        // POST: Slideshows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Url")] Slideshow slideshow, IFormFile file)
        {
            if (id != slideshow.Id)
            {
                return NotFound();
            }

            slideshow.Url = uploadFile(file);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(slideshow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideshowExists(slideshow.Id))
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
            return View(slideshow);
        }

        // GET: Slideshows/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slideshow = await _context.Slideshow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slideshow == null)
            {
                return NotFound();
            }

            return View(slideshow);
        }

        // POST: Slideshows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var slideshow = await _context.Slideshow.FindAsync(id);
            _context.Slideshow.Remove(slideshow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        private bool SlideshowExists(Guid id)
        {
            return _context.Slideshow.Any(e => e.Id == id);
        }

        private string uploadFile(IFormFile file)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=pierpontrcassets;AccountKey=3gG7N0e/zeIrW4ka6vc/KpuPY45B2FeBcQ+x/3i9iwdQPHwa+B8KBwFdVKk2/QKoIB6esQPkQtESyOdvOdBzWQ==;EndpointSuffix=core.windows.net";
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
