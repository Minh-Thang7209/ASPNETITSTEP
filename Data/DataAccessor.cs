using ASPNETITSTEP.Models.Admin;

namespace ASPNETITSTEP.Data
{
    public class DataAccessor(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;
        public Guid GetDbIdentity() => Guid.NewGuid();
        public List<Entities.ProductGroup> GetAllProductGroups(bool isIncludeHidden = false)
        {
            IQueryable<Entities.ProductGroup> query = _dataContext.ProductGroups;
            if (!isIncludeHidden)
            {
                query = query.Where(g => g.IsHidden == 0);
            }
            return [.. query.OrderBy(g => g.OrderInPrice)];
        }

        public async Task<Guid> AddNewProductGroup(Entities.ProductGroup productGroup)
        {
            Guid id = GetDbIdentity();
            productGroup.Id = id;
            _dataContext.ProductGroups.Add(productGroup);
            await _dataContext.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> AddNewProduct(AdminAddProductFormModel formModel, String? imageUrl)
        {
        
            Guid id = GetDbIdentity();
            if (formModel.ProductId != null)
                {
                    _dataContext.ProductVersions.Add(new()
                    {
                        Id = id,
                        ProductId = (await GetProductGroupById(formModel.ProductId.Value))!.Id,
                        ImageUrl = imageUrl,
                        Price = (decimal)formModel.Price,
                        Stock = formModel.Stock,
                        OrderInPrice = formModel.Order,
                        Slug = formModel.Slug,
                        IsHidden = formModel.IsHidden,                        
                    });
                }
                else
                {
                    Entities.ProductGroup group = await GetProductGroupById(formModel.GroupId);
                    // Розбираємо дані на Товар і Версію
                    Guid productId = GetDbIdentity();
                    _dataContext.Products.Add(new()
                    {
                        Id = productId,
                        GroupId = group.Id,
                        Name = formModel.Name,
                        Description = formModel.Description,
                        ImageUrl = imageUrl,
                        IsHidden = formModel.IsHidden,
                        OrderInPrice = formModel.Order,
                        Slug = formModel.Slug,
                    });
                    _dataContext.ProductVersions.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        ImageUrl = imageUrl,
                        Price = (decimal)formModel.Price,
                        Stock = formModel.Stock,
                        OrderInPrice = 1,
                        Slug = formModel.Slug,
                        IsHidden = formModel.IsHidden,
                        Version = formModel.Name                    
                    });
                }
            await _dataContext.SaveChangesAsync();
            return id;
        }

        public async Task<bool> IsProductFormModelValidAsync(AdminAddProductFormModel formModel)
        {
            var group = await GetProductGroupById(formModel.GroupId)
                    ?? throw new Exception($"Product group not found with id='{formModel.GroupId}'");
            if (formModel.ProductId != null)
                {
                    // додавання нової версії, слід пересвідчитись у наявності товару
                    _ = await GetProductById(formModel.ProductId!.Value)
                    ?? throw new Exception($"Product not found with id='{formModel.ProductId}'");
                }
            if(formModel.Slug != null)
            {
                if(_dataContext.Products.Any(p => p.Slug == formModel.Slug))
                {
                    throw new Exception($"Slug '{formModel.Slug}' is already in use by other product");
                }
            }

            return true;
        }
        public async Task<Entities.ProductGroup?> GetProductGroupById(Guid guid)
        {
            var grp = _dataContext.ProductGroups.FirstOrDefault(g => g.Id.ToString().ToLower() == guid.ToString().ToLower());
            return grp;
            
        }
        public async Task<Entities.Product?> GetProductById(Guid guid)
        {
            return _dataContext.Products.FirstOrDefault(g => g.Id.ToString().ToLower() == guid.ToString().ToLower());
        }
    }
}
