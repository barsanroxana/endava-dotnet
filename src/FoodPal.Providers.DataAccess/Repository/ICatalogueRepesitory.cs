using FoodPal.Providers.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Providers.DataAccess.Repository
{
    public interface ICatalogueRepository : IRepository<Catalogue>
    {
        Task<Catalogue> GetWithCatalogueByIdAsync(int catalogueId);
    }
}
