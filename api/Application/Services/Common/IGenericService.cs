namespace EIRL.Application.Services.Common
{
    public interface IGenericService<TDto> where TDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(object id);
        Task<TDto> CreateAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(object id);
        Task<int> SaveChangesAsync();
    }

}