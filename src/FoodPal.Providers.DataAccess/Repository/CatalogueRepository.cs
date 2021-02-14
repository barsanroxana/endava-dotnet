using FoodPal.Providers.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Providers.DataAccess.Repository
{
    public class CatalogueRepository : Repository<Catalogue>, ICatalogueRepository
    {
        public CatalogueRepository(ProvidersContext context) : base(context)
        {

        }

        public async Task<Catalogue> GetWithCatalogueByIdAsync(int catalogueId)
        {
            return await _providersContext.Catalogue
                 .Include(ci => ci.Provider)
                 .SingleOrDefaultAsync(x => x.Id == catalogueId);
        }
    }
}
