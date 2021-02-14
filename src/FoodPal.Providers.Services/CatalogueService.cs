using AutoMapper;
using FoodPal.Providers.DataAccess.UnitOfWork;
using FoodPal.Providers.DomainModels;
using FoodPal.Providers.Dtos;
using FoodPal.Providers.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Providers.Services
{
    public class CatalogueService : ICatalogueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CatalogueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        public async Task<CatalogueDto> GetCatalogueByIdAsync(int catalogueId)
        {
            Catalogue model = await _unitOfWork.CatalogueRepository.GetWithCatalogueByIdAsync(catalogueId);
            return _mapper.Map<Catalogue, CatalogueDto>(model);
        }
    }
}
