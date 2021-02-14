using FoodPal.Providers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Providers.Services.Contracts
{
    public interface ICatalogueService
    {
        Task<CatalogueDto> GetCatalogueByIdAsync(int catalogueId);
    }
}
