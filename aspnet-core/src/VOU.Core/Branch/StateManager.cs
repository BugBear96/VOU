using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VOU.Branch
{
    public class StateManager : VOUDomainServiceBase, IStateManager
    {
        private readonly IRepository<State> _repository;

        public StateManager(
            IRepository<State> repository)
        {
            _repository = repository;
        }

        public IQueryable<State> States => _repository.GetAll();

        public Task CreateAsync(State state)
        {
            return _repository.InsertAsync(state);
        }

        public Task<State> FindAsync(int id)
        {
            return _repository.GetAll()
                .Where(x => x.Id == id)
                .Include(x => x.Cities)
                .FirstOrDefaultAsync();
        }

        public Task<State> FindByNameAsync(string stateName)
        {
            return _repository.GetAll()
                .Where(x => x.StateName == stateName)
                .Include(x => x.Cities)
                .FirstOrDefaultAsync();
        }
    }
}
