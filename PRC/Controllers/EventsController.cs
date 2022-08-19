using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRC.Data;
using PRC.Models;
using PRC.Models.ViewModels;

namespace PRC.Controllers
{
    [Authorize(Roles = "admin, editor")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        [Route("admin/events")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.OrderByDescending(e => e.Date).ToListAsync());
        }

        // GET: Events
        [AllowAnonymous]
        [Route("events")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Events.OrderBy(e => e.Date).ToListAsync());
        }

        // GET: Events/Create
        [Route("admin/events/create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/events/create")]
        public async Task<IActionResult> Create([Bind("Id,Date,Title,Content,StartTime,EndTime")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.Id = Guid.NewGuid();
                @event.MonthYear = @event.Date.Month.ToString() + @event.Date.Year.ToString();
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        [Route("admin/events/edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/events/edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Date,Title,Content,StartTime,EndTime")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    @event.MonthYear = @event.Date.Month.ToString() + @event.Date.Year.ToString();
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        [Route("admin/events/delete/{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("admin/events/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Events/List
        [AllowAnonymous]
        public IActionResult List([FromBody] EventViewModel @event)
        {
            var allDates = new List<Event>();

            allDates = _context.Events.Where(e => e.MonthYear == @event.MonthYear).ToList();

            return Json(allDates);
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
