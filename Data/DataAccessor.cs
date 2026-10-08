using ASPNETITSTEP.Data.Entities;
using ASPNETITSTEP.Models.Admin;
using ASPNETITSTEP.Models.User;
using ASPNETITSTEP.Services.Kdf;
using Microsoft.EntityFrameworkCore;

namespace ASPNETITSTEP.Data
{
    public class DataAccessor(DataContext dataContext, IKdfService kdfService)
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService = kdfService;
        public Guid GetDbIdentity()
        {
            return _dataContext.Database
                .SqlQueryRaw<string>("SELECT UUID()")
                .AsEnumerable()
                .Select(Guid.Parse)
                .First();
        }
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
                    Id = GetDbIdentity(),
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
            Console.WriteLine("BEFORE SAVE");
            await _dataContext.SaveChangesAsync();
            Console.WriteLine("AFTER SAVE");
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
            if (formModel.Slug != null)
            {
                if (_dataContext.Products.Any(p => p.Slug == formModel.Slug))
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
        public async Task UpdateProductGroup(Entities.ProductGroup productGroup)
        {
            _dataContext.ProductGroups.Update(productGroup);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteProductGroup(Guid id)
        {
            var productGroup = await GetProductGroupById(id)
                ?? throw new Exception($"Product group not found with id='{id}'");

            _dataContext.ProductGroups.Remove(productGroup);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<UserAccess> RegisterUserAsync(UserSignupFormModel formModel)
        {
            if (_dataContext.UserAccesses.Any(ua => ua.Login == formModel.Login))
            {
                throw new Exception(
                    $"Login '{formModel.Login}' is already in use");
            }

            Guid userId = Guid.NewGuid();

            _dataContext.UsersData.Add(new()
            {
                Id = userId,
                FullName = formModel.FullName,
                Email = formModel.Email,
                Phone = formModel.Phone,
                RegisteredAt = DateTime.Now,
                Birthdate = default,
            });

            String salt = Guid.NewGuid().ToString();

            UserAccess userAccess = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = _dataContext.UserRoles.First(r => r.Name == "User").Id,
                Login = formModel.Login,
                Salt = salt,
                Dk = _kdfService.Dk(formModel.Password, salt),
            };

            _dataContext.UserAccesses.Add(userAccess);

            await _dataContext.SaveChangesAsync();

            return userAccess;
        }
        public UserAccess? AuthenticateUser(string login, string password)
        {
            UserAccess? userAccess = _dataContext
                .UserAccesses
                .Include(ua => ua.UserData)
                .Include(ua => ua.UserRole)
                .AsNoTracking()
                .FirstOrDefault(ua => ua.Login == login);

            if (userAccess == null)
            {
                return null;
            }

            String dk = _kdfService.Dk(password, userAccess.Salt);

            if (dk == userAccess.Dk)
            {
                return userAccess;
            }

            return null;
        }
    }
}
