﻿using RESTFulSocial.Core.QueryFilters;
using System;

namespace RESTFulSocial.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl);        
    }
}
