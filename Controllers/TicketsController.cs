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
using System.Security.Claims;

namespace TicketSystemApplication.Controllers
{
    [Authorize(Roles = ("Admin,IT,Support"))]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Support"))
                {
                    var applicationDbContextt = _context.Ticket.Where(x => x.TicketStatusId == 1 || x.TicketStatusId == 2)
                      .OrderByDescending(x=>x.Id)  
                        .Include(t => t.Department).Include(t => t.TicketStatus);
                    return View(await applicationDbContextt.ToListAsync());
                }
                else if (User.IsInRole("IT"))
                {
                    var applicationDbContextt = _context.Ticket.Where(x => x.TicketStatusId == 3)
                        .OrderByDescending(x => x.Id).Include(t => t.Department).Include(t => t.TicketStatus);
                    return View(await applicationDbContextt.ToListAsync());
                }
                else
                {
                    var applicationDbContext = _context.Ticket
                        .OrderByDescending(x => x.Id).Include(t => t.Department).Include(t => t.TicketStatus);
                    return View(await applicationDbContext.ToListAsync());
                }
                
            }
            return View();
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Department)
                .Include(t => t.TicketStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (ticket.TicketStatusId == 1)
            {
                ticket.TicketStatusId = 2;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
                RedirectToAction(nameof(Details));
            }
            return View(ticket);
        }
        public async Task<IActionResult> Redirect(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Department)
                .Include(t => t.TicketStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
           
                ticket.TicketStatusId = 3;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            RedirectToAction(nameof(Index));

           return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Solve(int? id,string comments)
        {
            if (id == null )
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Department)
                .Include(t => t.TicketStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            TicketLog log = new TicketLog();
            log.TicketID = ticket.Id;
            log.Comments = comments;
            log.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            log.Date = DateTime.Now;
            _context.Add(log);
            await _context.SaveChangesAsync();

            ticket.TicketStatusId = 4;
            _context.Update(ticket);
            await _context.SaveChangesAsync();
           // RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Phone,DepartmentId")] Ticket ticket)
        {
            ticket.Type = null;
            ticket.UserId = null;
            ticket.Date= DateTime.Now;
            ticket.TicketStatusId = 1;


            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", ticket.DepartmentId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", ticket.DepartmentId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Type,UserId,Phone,Date,TicketStatusId,DepartmentId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", ticket.DepartmentId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Department)
                .Include(t => t.TicketStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        [Authorize(Roles = "Admin")]
        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ticket == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ticket'  is null.");
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Ticket?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
