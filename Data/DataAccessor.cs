namespace ASPNETITSTEP.Data
{
    public class DataAccessor(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;
        public Guid GetDbIdentity() => Guid.NewGuid();
        public List<Entities.ProductGroup> GetAllProductGroups(bool isIncludeHidden = false){
            IQueryable<Entities.ProductGroup> query = _dataContext.ProductGroups;
            if (!isIncludeHidden)
            {
                query = query.Where(g => g.IsHidden == 0);
            }
            return [..query.OrderBy(g => g.OrderInPrice)];
        }

        public async Task<Guid> AddNewProductGroup(Entities.ProductGroup productGroup)
        {
            Guid id = GetDbIdentity();
            productGroup.Id = id;
            _dataContext.ProductGroups.Add(productGroup);
            await _dataContext.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> AddNewProduct()
        {
            Guid id = GetDbIdentity();
            return id;
        }
    }
}
