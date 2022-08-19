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
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;
        const string BLOB = "https://pierpontrcassets.blob.core.windows.net/website/";
        const string XSTR = "DefaultEndpointsProtocol=https;AccountName=pierpontrcassets;AccountKey=3gG7N0e/zeIrW4ka6vc/KpuPY45B2FeBcQ+x/3i9iwdQPHwa+B8KBwFdVKk2/QKoIB6esQPkQtESyOdvOdBzWQ==;EndpointSuffix=core.windows.net";

        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("admin/staff")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staff.OrderBy(s => s.Name).ToListAsync());
        }

        [Route("admin/staff/new")]
        public IActionResult Create()
        {
            var departments = _context.Departments.OrderBy(d => d.Name).ToList();

            var model = new StaffEditViewModel
            { 
                Departments = departments
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/staff/new")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Department,Role,Lead")] StaffEditViewModel model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                model.Image = uploadFile(image);
                var staff = new Staff
                { 
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                    Department = model.Department,
                    Role = model.Role,
                    Lead = model.Lead,
                    Image = model.Image
                };

                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("admin/staff/edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            var departments = _context.Departments.OrderBy(d => d.Name).ToList();

            var model = new StaffEditViewModel
            {
                Id = staff.Id,
                Name = staff.Name,
                Description = staff.Description,
                Department = staff.Department,
                Departments = departments,
                Role = staff.Role,
                Lead = staff.Lead,
                Image = staff.Image
            };

            return View(model);
        }

        [Route("admin/staff/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Department,Role,Lead,Image")] StaffEditViewModel model, IFormFile image)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                model.Image = uploadFile(image);
                var staff = new Staff
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Department = model.Department,
                    Role = model.Role,
                    Lead = model.Lead,
                    Image = model.Image
                };

                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            return View(model);
        }

        [Route("admin/staff/delete/{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [Route("admin/staff/delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        private bool StaffExists(Guid id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
    }
}
