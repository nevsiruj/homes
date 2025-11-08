using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.DTOs
{
    public class PaginationParamsDTO
    {
        private const int MaxPageSize = 1000;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 9;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

       
        public string? Operaciones { get; set; } 
        public List<string>? Tipos { get; set; } 
        public List<string>? Ubicaciones { get; set; }        
        public List<int>? Habitaciones { get; set; } 
        public List<int>? HabitacionesMin { get; set; }
        public List<string>? Amenidades { get; set; }      
        public decimal? PrecioMinimo { get; set; }
        public decimal? PrecioMaximo { get; set; }
        public string? Orden { get; set; }
        public string? SearchTerm { get; set; }

        public string? Titulo { get; set; } 
        public string? CodigoPropiedad { get; set; }
        public bool? Luxury { get; set; }
    }

    // Tu PagedResult<T> permanece igual
    public class PagedResult<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
