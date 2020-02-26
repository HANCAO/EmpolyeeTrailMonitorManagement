using EmpolyeeTrailMonitor.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpolyeeTrailMonitor.ViewComponents
{
    public class EmployeeCountViewComponent:ViewComponent
    {
        private readonly EmpolyeeTrailContext empolyeeTrails;
        public EmployeeCountViewComponent(EmpolyeeTrailContext empolyeeTrails)
        {
            this.empolyeeTrails = empolyeeTrails;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //获取数据总行数
            ViewData["DataCount"] = empolyeeTrails.EmpolyeeTrail.Count();
            //获取员工个数
            ViewData["EmpolyeeCount"] = empolyeeTrails.EmpolyeeTrail.Select(s => s.CreateUser).Distinct().Count();

            return View();
        }
    }
}
