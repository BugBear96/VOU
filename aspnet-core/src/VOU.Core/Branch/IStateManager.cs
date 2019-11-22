using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOU.Branch
{
    public interface IStateManager
    {
        IQueryable<State> States { get; }

        Task CreateAsync(State state);

        Task<State> FindByNameAsync(string stateName);

        Task<State> FindAsync(int id);
    }
}
