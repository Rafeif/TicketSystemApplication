using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketSystemApplication.Data;
using TicketSystemApplication.Models;

namespace TicketSystemApplication.Controllers
{
    [Authorize(Roles = ("Admin,IT,Support"))]
    public class TicketLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketLogs
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

                var applicationDbContext = _context.TicketLog.Include(t => t.Ticket);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

                var applicationDbContext = _context.TicketLog.Where(x => x.UserId == currentUserId).Include(t => t.Ticket);
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: TicketLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TicketLog == null)
            {
                return NotFound();
            }

            var ticketLog = await _context.TicketLog
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketLog == null)
            {
                return NotFound();
            }

            return View(ticketLog);
        }

        // GET: TicketLogs/Create
        public IActionResult Create()
        {
            ViewData["TicketID"] = new SelectList(_context.Ticket, "Id", "Id");
            return View();
        }

        // POST: TicketLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketID,UserId,Comments,Date")] TicketLog ticketLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketID"] = new SelectList(_context.Ticket, "Id", "Id", ticketLog.TicketID);
            return View(ticketLog);
        }

        // GET: TicketLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TicketLog == null)
            {
                return NotFound();
            }

            var ticketLog = await _context.TicketLog.FindAsync(id);
            if (ticketLog == null)
            {
                return NotFound();
            }
            ViewData["TicketID"] = new SelectList(_context.Ticket, "Id", "Id", ticketLog.TicketID);
            return View(ticketLog);
        }

        // POST: TicketLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketID,UserId,Comments,Date")] TicketLog ticketLog)
        {
            if (id != ticketLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketLogExists(ticketLog.Id))
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
            ViewData["TicketID"] = new SelectList(_context.Ticket, "Id", "Id", ticketLog.TicketID);
            return View(ticketLog);
        }

        // GET: TicketLogs/Delete/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TicketLog == null)
            {
                return NotFound();
            }

            var ticketLog = await _context.TicketLog
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketLog == null)
            {
                return NotFound();
            }

            return View(ticketLog);
        }

        // POST: TicketLogs/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TicketLog == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TicketLog'  is null.");
            }
            var ticketLog = await _context.TicketLog.FindAsync(id);
            if (ticketLog != null)
            {
                _context.TicketLog.Remove(ticketLog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketLogExists(int id)
        {
          return (_context.TicketLog?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
