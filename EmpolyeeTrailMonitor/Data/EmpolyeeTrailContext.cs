using EmpolyeeTrailMonitor.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpolyeeTrailMonitor.Data
{
    public class EmpolyeeTrailContext : DbContext
    {
        public EmpolyeeTrailContext(DbContextOptions<EmpolyeeTrailContext> options) : base(options)
        {

        }

        //实体集=数据表 实体=数据行
        public DbSet<EmpolyeeTrail> EmpolyeeTrail { get; set; } 
    }
}
