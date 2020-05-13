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

        public ActionResult Show()
        {
            return View();
        }

        public EmpolyeeTrailsController(EmpolyeeTrailContext context)
        {
            _context = context;
        }

        // 显示碎片化数据内容页面
        public async Task<IActionResult> Index(string searchString, int? pageNumber, string currentFilter)
        {
            var empolyeeTrails = from s in _context.EmpolyeeTrail select s;

            //添加搜索框
            if (searchString != null){
                pageNumber = 1;
            }
            else{
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
        }

        // 显示指定ID用户数据
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

        
        // 编辑指定用户数据
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

        // 删除指定用户数据
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

        public JsonResult ReadTableList()//string searchString, int? pageNumber, string currentFilter
        {
            var empolyeeTrails = from s in _context.EmpolyeeTrail select s;
            var  curlist = empolyeeTrails.Take(1000).ToList();
            ////添加搜索框
            //if (searchString != null)
            //{
            //    pageNumber = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            ////获取数据总行数
            //ViewData["DataCount"] = empolyeeTrails.Count();
            ////获取员工个数
            //ViewData["EmpolyeeCount"] = empolyeeTrails.Select(s => s.CreateUser).Distinct().Count();

            //ViewData["CurrentFilter"] = searchString;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    empolyeeTrails = empolyeeTrails.Where(s => s.CreateUser.Contains(searchString));
            //}

            return Json(new { rows = curlist });  // curList就等于你查出来的empolyeeTrails

        }

    }
}
