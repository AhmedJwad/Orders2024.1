﻿using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Implementations
{
    public class StatesUnitOfWork : GenericUnitOfWork<State>, IStatesUnitOfWork
    {
        private readonly IStatesRepository _statesRepository;

        public StatesUnitOfWork(IGenericRepository<State> Repository , IStatesRepository statesRepository) : base(Repository)
        {
           _statesRepository = statesRepository;
        }

        public override async Task<ActionResponse<IEnumerable< State>>> GetAsync()=> await _statesRepository.GetAsync();
        public override async Task<ActionResponse<State>> GetAsync(int Id)=>await _statesRepository.GetAsync(Id);
    }
}