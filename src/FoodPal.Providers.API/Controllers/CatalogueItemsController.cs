using AutoMapper;
using FoodPal.Providers.Dtos;
using FoodPal.Providers.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Providers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueItemsController : ControllerBase
    {
        private readonly ICatalogueItemService _catalogueItemService;
        private readonly IProviderService _providerService;
        private readonly ICatalogueService _catalogueService;
        private readonly IMapper _mapper;


        public CatalogueItemsController(ICatalogueItemService catalogueItemService, IProviderService providerService, ICatalogueService catalogueService, IMapper mapper)
        {
            _catalogueItemService = catalogueItemService ?? throw new ArgumentNullException(nameof(catalogueItemService));
            _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
            _catalogueService = catalogueService ?? throw new ArgumentNullException(nameof(catalogueService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogueItems(int providerId)
        {
            try
            {
                var catalogueItems = await _catalogueItemService.GetCatalogueItemsForProviderAsync(providerId);
                return Ok(catalogueItems);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to succeed the operation!");
            }
        }

        [HttpGet("{itemId}", Name = "GetCatalogueItem")]
        public async Task<IActionResult> GetCatalogueItem(int itemId)
        {
            try
            {
                var provider = await _catalogueItemService.GetCatalogueItemByIdAsync(itemId);

                if (provider == null)
                    return NotFound();

                return Ok(provider);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to succeed the operation!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProvider(NewCatalogueItemDto catalogueItem)
        {
            try
            {
                //TODO: verificare si pe CategoryId 

                var catalogueDto = _catalogueService.GetCatalogueByIdAsync(catalogueItem.CatalogueId).Result;

                if (catalogueDto == null)
                {
                    ModelState.AddModelError(
                        "CatalogueId",
                        "Unexisting catalog!");
                    return BadRequest(ModelState);
                }

                if (await _catalogueItemService.CatalogueItemExistsAsync(catalogueItem.Name, catalogueDto.Provider.Id))
                {
                    ModelState.AddModelError(
                        "Name",
                        "A CatalogueItem with the same name already exists into the database!");
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var insertedCatalogueItem = await _catalogueItemService.CreateAsync(catalogueItem);

                if (insertedCatalogueItem == null)
                    return Problem();

                // metoda va retuna doar Id-ul obiectul creat
                // return CreatedAtRoute("GetCatalogueItem", new { itemId = insertedCatalogueItem });

                // return Id + obiectul creat
                return CreatedAtRoute("GetCatalogueItem", new { itemId = insertedCatalogueItem.Id }, insertedCatalogueItem);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to succeed the operation!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCatalogueItem(int id, [FromBody] CatalogueItemDto catalogueItem)
        {
            try
            {
                if (catalogueItem.Id != id)
                {
                    ModelState.AddModelError(
                        "Identifier",
                        "Request body not apropiate for ID");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _catalogueItemService.GetCatalogueItemByIdAsync(id) == null)
                {
                    return NotFound();
                }

                await _catalogueItemService.UpdateAsync(catalogueItem);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to succeed the operation!");
            }
        }

        [HttpDelete("{catalogueItemId}")]
        public async Task<IActionResult> DeleteCatalogueItem(int catalogueItemId)
        {
            try
            {
                if (await _catalogueItemService.GetCatalogueItemByIdAsync(catalogueItemId) == null)
                {
                    return NotFound();
                }

                await _catalogueItemService.DeleteAsync(catalogueItemId);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to succeed the operation!");
            }
        }
    }
}

