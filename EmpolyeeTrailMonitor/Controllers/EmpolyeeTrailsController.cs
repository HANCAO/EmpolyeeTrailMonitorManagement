using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpolyeeTrailMonitor.Data;
using EmpolyeeTrailMonitor.Models;

namespace EmpolyeeTrailMonitor.Controllers
{
    public class EmpolyeeTrailsController : Controller
    {
        private readonly EmpolyeeTrailContext _context;

        public EmpolyeeTrailsController(EmpolyeeTrailContext context)
        {
            _context = context;
        }

        // GET: EmpolyeeTrails
        public async Task<IActionResult> Index(string searchString, int? pageNumber, string currentFilter)
        {
            var empolyeeTrails = from s in _context.EmpolyeeTrail select s;

            //添加搜索框
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //获取数据总行数
            ViewData["DataCount"]=empolyeeTrails.Count();
            //获取员工个数
            ViewData["EmpolyeeCount"] = empolyeeTrails.Select(s => s.CreateUser).Distinct().Count();

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString)){
                empolyeeTrails = empolyeeTrails.Where(s => s.CreateUser.Contains(searchString));
            }

            //添加分页
            int pageSize = 20;
            return View(await PaginatedList<EmpolyeeTrail>.CreateAsync(empolyeeTrails.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await _context.EmpolyeeTrail.ToListAsync());
        }

        // GET: EmpolyeeTrails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empolyeeTrail = await _context.EmpolyeeTrail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empolyeeTrail == null)
            {
                return NotFound();
            }

            return View(empolyeeTrail);
        }

        // GET: EmpolyeeTrails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmpolyeeTrails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GPSX,GPSY,BmapLap,BmapLng,CreateTime,CreateUser,IsCar,Distance,DistanceSecond")] EmpolyeeTrail empolyeeTrail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empolyeeTrail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empolyeeTrail);
        }

        // GET: EmpolyeeTrails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empolyeeTrail = await _context.EmpolyeeTrail.FindAsync(id);
            if (empolyeeTrail == null)
            {
                return NotFound();
            }
            return View(empolyeeTrail);
        }

        // POST: EmpolyeeTrails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GPSX,GPSY,BmapLap,BmapLng,CreateTime,CreateUser,IsCar,Distance,DistanceSecond")] EmpolyeeTrail empolyeeTrail)
        {
            if (id != empolyeeTrail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empolyeeTrail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpolyeeTrailExists(empolyeeTrail.Id))
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
            return View(empolyeeTrail);
        }

        // GET: EmpolyeeTrails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empolyeeTrail = await _context.EmpolyeeTrail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empolyeeTrail == null)
            {
                return NotFound();
            }

            return View(empolyeeTrail);
        }

        // POST: EmpolyeeTrails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empolyeeTrail = await _context.EmpolyeeTrail.FindAsync(id);
            _context.EmpolyeeTrail.Remove(empolyeeTrail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpolyeeTrailExists(int id)
        {
            return _context.EmpolyeeTrail.Any(e => e.Id == id);
        }
    }
}
