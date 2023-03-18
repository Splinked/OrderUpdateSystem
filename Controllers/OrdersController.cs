using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderUpdateSystem.Data;
using OrderUpdateSystem.Models;
using OrderUpdateSystem.Services;

namespace OrderUpdateSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderUpdateSystemContext _context;
        private readonly MailService _mService;

        public OrdersController(OrderUpdateSystemContext context, MailService mailService)
        {
            _mService = mailService;
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
              return _context.Orders != null ? 
                          View(await _context.Orders.ToListAsync()) :
                          Problem("Entity set 'OrderUpdateSystemContext.Orders'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderUpdateDate,Note,Status,OperatorId")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderUpdateDate,Note,Status,OperatorId")] Orders orders)
        {
            if (id != orders.Id)
            {
                return NotFound();
            }

            if (CheckForStarted(id) && orders.Status != "Completed")
            {
                return Problem("Order has already been started");
            }

            if (orders.Status.Equals("Completed"))
            {
                if (!CheckForCompleted(id, orders.OperatorId))
                    {
                        return Problem("Order can only be completed by the operator who started the order");
                    }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();

                    _context.Update(orders);
                    await _context.SaveChangesAsync();

                    if (!_mService.Notify("Order status has been updated for order: " + orders.Id))
                    {
                        orders.Status = "Fail";
                        _context.Update(orders);
                        await _context.SaveChangesAsync();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.Id))
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
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'OrderUpdateSystemContext.Orders'  is null.");
            }
            var orders = await _context.Orders.FindAsync(id);
            if (orders != null)
            {
                _context.Orders.Remove(orders);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool CheckForStarted(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            return (order.Status.Equals("Started"));
        }

        private bool CheckForCompleted(int id, int operatorId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            return (operatorId == order?.OperatorId);
        }
    }
}
