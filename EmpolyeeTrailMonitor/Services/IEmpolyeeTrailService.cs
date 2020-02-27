using EmpolyeeTrailMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpolyeeTrailMonitor.Services
{
    public interface IEmpolyeeTrailService
    {
        Task<IEnumerable<EmpolyeeTrail>> GetAll();
        Task<IEnumerable<EmpolyeeTrail>> GetByCreateUser(string createUser);

        Task<int> CountCreateUser();

        Task<int> CountAll();

    }
}
