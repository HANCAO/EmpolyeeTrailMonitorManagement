using EmpolyeeTrailMonitor.Data;
using EmpolyeeTrailMonitor.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpolyeeTrailMonitor.Services
{
    public class EmpolyeeTrailService : IEmpolyeeTrailService
    {
        private readonly EmpolyeeTrailContext empolyeeTrailContext;
        private readonly List<EmpolyeeTrail> empolyeeTrails = new List<EmpolyeeTrail>();

        public EmpolyeeTrailService(EmpolyeeTrailContext empolyeeTrailContext)
        {
            this.empolyeeTrailContext = empolyeeTrailContext;
            empolyeeTrails = (from s in empolyeeTrailContext.EmpolyeeTrail select s).ToList();
        }


        public Task<int> CountAll()
        {
            return Task.Run(function: () => empolyeeTrails.Count());
        }

        public Task<int> CountCreateUser()
        {
            return Task.Run(function: () => empolyeeTrails.Select(s => s.CreateUser).Distinct().Count());
        }

        public Task<IEnumerable<EmpolyeeTrail>> GetAll()
        {
            return Task.Run(function: () => empolyeeTrails.AsEnumerable());
        }

        public Task<IEnumerable<EmpolyeeTrail>> GetByCreateUser(string createUser)
        {
            return Task.Run(function: () => empolyeeTrails.Where(s => s.CreateUser.Contains(createUser)));
        }
    }
}
