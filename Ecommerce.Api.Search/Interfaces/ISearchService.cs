using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(bool IsSucess, dynamic SearchResults)> SearchAsync(int customerId);
    }
}
