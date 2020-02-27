using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpolyeeTrailMonitor.Data;
using EmpolyeeTrailMonitor.Models;
using EmpolyeeTrailMonitor.Services;

namespace EmpolyeeTrailMonitor.Controllers
{
    public class EmpolyeeTrailsController : Controller
    {
        #region 使用数据库上下文类
        private readonly EmpolyeeTrailContext empolyeeTrailContext;
        public EmpolyeeTrailsController(EmpolyeeTrailContext empolyeeTrailContext)
        {
            this.empolyeeTrailContext = empolyeeTrailContext;
        }
        #endregion

        #region 使用注册服务类
        //private readonly IEmpolyeeTrailService empolyeeTrailService;
        //public EmpolyeeTrailsController(IEmpolyeeTrailService empolyeeTrailService)
        //{
        //    this.empolyeeTrailService = empolyeeTrailService;
        //}
        #endregion

        public async Task<IActionResult> Index(string searchString, int? pageNumber, string currentFilter)
        {
            //添加搜索框
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            #region 使用数据库上下文类操作数据表
            var empolyeeTrails = from s in empolyeeTrailContext.EmpolyeeTrail select s;

            //获取数据总行数
            ViewData["DataCount"] = empolyeeTrails.Count();
            //获取员工个数
            ViewData["EmpolyeeCount"] = empolyeeTrails.Select(s => s.CreateUser).Distinct().Count();

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                empolyeeTrails = empolyeeTrails.Where(s => s.CreateUser.Contains(searchString));
            }
            #endregion

            #region 使用注册服务类操作数据表
            //var empolyeeTrails=empolyeeTrailService.GetAll();
            //ViewData["DataCount"] = empolyeeTrailService.CountAll();
            //ViewData["EmpolyeeCount"] = empolyeeTrailService.CountCreateUser();
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    empolyeeTrails = empolyeeTrails.Where(s => s.CreateUser.Contains(searchString));
            //}
            #endregion


            //添加分页
            int pageSize = 20;

            return View(await PaginatedList<EmpolyeeTrail>.CreateAsync(empolyeeTrails.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
