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

namespace PRC.Controllers
{
    [Authorize(Roles = "admin, editor")]
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        const string BLOB = "https://prcassets.blob.core.windows.net/website/";

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Photos
        [Route("admin/gallery")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Photos.ToListAsync());
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,Title,Alt")] Photo photo, IFormFile file)
        {
            photo.Url = uploadFile(file);

            if (ModelState.IsValid)
            {
                photo.Id = Guid.NewGuid();
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Url,Title,Alt")] Photo photo, IFormFile file)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            photo.Url = uploadFile(file);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
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
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(Guid id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }

        private string uploadFile(IFormFile file)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=prcassets;AccountKey=3gG7N0e/zeIrW4ka6vc/KpuPY45B2FeBcQ+x/3i9iwdQPHwa+B8KBwFdVKk2/QKoIB6esQPkQtESyOdvOdBzWQ==;EndpointSuffix=core.windows.net";
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
