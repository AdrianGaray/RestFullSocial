using System;
using System.Collections.Generic;
using System.Text;

namespace RESTFulSocial.Core.QueryFilters
{
    public class PostQueryFilter
    {
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }

        // Cuantos Registro vamos a querer por pagina
        public int PageSize { get; set; }

        // Numero de la Pagina
        public int PageNumber { get; set; }
    }
}
