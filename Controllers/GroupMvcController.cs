using ASPNETITSTEP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETITSTEP.Controllers
{
    public class GroupMvcController(DataContext dataContext) : Controller
    {
        private readonly DataContext _dataContext = dataContext;

        public IActionResult Index()
        {
            var groups = _dataContext
                .ProductGroups
                .Include(g => g.Children)
                .Where(g => g.IsHidden == 0 && g.ParentId == null)
                .OrderBy(g => g.OrderInPrice)
                .ToArray();

            return View(groups);
        }
    }
}