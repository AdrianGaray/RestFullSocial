using System;
using System.Collections.Generic;
using System.Linq;

namespace RESTFulSocial.Core.CustomEntities
{
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Pagina actual en la estamos ubicado
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Total de paginas
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Cuantos registros qureremos mostrar por pagina
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Total de Registros
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Con esta propiedad vammos a identificar si tenemos paginas anteriores
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;
        // identificar si tenemos paginas siguientes
        public bool HasNextPage => CurrentPage < TotalPages;

        // Numero de la siguiente pagina
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?)null;
        // el numero de la pagina anterior
        public int? PreviousPageNumber => HasPreviousPage? CurrentPage - 1 : (int?)null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items">El listado que queremos paginar</param>
        /// <param name="count">cantidad de registros que hay en este listado</param>
        /// <param name="pageNumber">El numero de la pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();

            // aplicar la paginacion
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
