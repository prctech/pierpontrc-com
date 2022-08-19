using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRC.Data;
using PRC.Models;

namespace PRC.Controllers
{
    public class WidgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WidgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Widgets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Widgets.ToListAsync());
        }

        // GET: Widgets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var widget = await _context.Widgets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (widget == null)
            {
                return NotFound();
            }

            return View(widget);
        }

        // GET: Widgets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Widgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PageId")] Widget widget)
        {
            if (ModelState.IsValid)
            {
                widget.Id = Guid.NewGuid();
                _context.Add(widget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(widget);
        }

        // GET: Widgets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var widget = await _context.Widgets.FindAsync(id);
            if (widget == null)
            {
                return NotFound();
            }
            return View(widget);
        }

        // POST: Widgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,PageId")] Widget widget)
        {
            if (id != widget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(widget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WidgetExists(widget.Id))
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
            return View(widget);
        }

        // GET: Widgets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var widget = await _context.Widgets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (widget == null)
            {
                return NotFound();
            }

            return View(widget);
        }

        // POST: Widgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var widget = await _context.Widgets.FindAsync(id);
            _context.Widgets.Remove(widget);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WidgetExists(Guid id)
        {
            return _context.Widgets.Any(e => e.Id == id);
        }
    }
}
