using ASPNETITSTEP.Data;
using ASPNETITSTEP.Models.Admin;
using ASPNETITSTEP.Services.Storage;
using ASPNETITSTEP.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    public class AdminController(IStorageService storageService, DataAccessor dataAccessor, DataContext dataContext) : Controller
    {
        private readonly IStorageService _storageService = storageService;
        private readonly DataContext _dataContext = dataContext;
        private readonly DataAccessor _dataAccessor = dataAccessor;



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product()
        {
            AdminGroupViewModel viewModel = new()
            {
                Groups = _dataAccessor.GetAllProductGroups(isIncludeHidden: true),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AdminAddProductFormModel formModel)
        {
            try
            {
                if (!TryValidateModel(formModel))
                {
                    return BadRequest(ModelState);
                }
                if (formModel.Stock != -1 && formModel.Stock <= 0)
                {
                    return BadRequest("Кількість має бути цілим позитивним числом або -1");
                }
                if (formModel.Price <= 0.01)
                {
                    return BadRequest("Ціна повинна бути більше за 0.01");
                }
                if (!string.IsNullOrWhiteSpace(formModel.Slug))
                {
                    if (_dataContext.Products.Any(p => p.Slug == formModel.Slug))
                    {
                        return BadRequest($"Slug '{formModel.Slug}' is already in use");
                    }
                }

                await _dataAccessor.IsProductFormModelValidAsync(formModel);
                String? imageUrl = null;
                if (formModel.Image != null)
                {
                    imageUrl = _storageService.Save(formModel.Image);
                }

                _dataAccessor.AddNewProduct(formModel, imageUrl);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Group()
        {
            AdminGroupViewModel viewModel = new()
            {
                Groups = _dataContext.ProductGroups.OrderBy(g => g.OrderInPrice).ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(AdminAddGroupFormModel formModel)
        {
            try
            {
                if (!TryValidateModel(formModel))
                {
                    return BadRequest(ModelState);
                }
                if (_dataContext.ProductGroups.Any(g => g.Slug == formModel.Slug))
                {
                    return BadRequest($"Slug '{formModel.Slug}' is already in use");
                }
                /* Д.З. Реалізувати валідацію моделі форми 
                 * додавання нової товарної групи
                 * - назва (довжина, відсутність спецсимволів)
                 * - опис (довжина)
                 * - Slug (унікальність, url-коректність)
                 */
                Guid newGroupId = await _dataAccessor.AddNewProductGroup(new()
                {
                    ParentId = formModel.ParentId,
                    Name = formModel.Name,
                    Description = formModel.Description,
                    Slug = formModel.Slug,
                    IsHidden = formModel.IsHidden,
                    ImageUrl = "/storage/image/" + _storageService.Save(formModel.Image),
                    OrderInPrice = formModel.Order,
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateGroup(Guid id, string name)
        {
            var group = await _dataAccessor.GetProductGroupById(id);

            if (group == null)
                return NotFound();

            group.Name = name;

            await _dataAccessor.UpdateProductGroup(group);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            await _dataAccessor.DeleteProductGroup(id);

            return Ok();
        }
    }
}