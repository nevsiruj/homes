using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlmacenSystem.Infrastructure.Repositories.Common;
using Astuc.Domain.DTOs;
using AutoMapper;
using EIRL.Application.Services.Common;

namespace EIRL.Application.Services
{
    public abstract class GenericService<TEntity, TDto> : IGenericService<TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        protected readonly ProjectDbContext _context; 

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;            
        }

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper, ProjectDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context; 
        }
  
        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public virtual async Task<TDto> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.CreateAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(object id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _repository.SaveChangesAsync();
        }
    }
}