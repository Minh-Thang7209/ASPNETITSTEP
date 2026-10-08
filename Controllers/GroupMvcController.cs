using ASPNETITSTEP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETITSTEP.Controllers
{
    public class GroupMvcController(DataContext dataContext) : Controller
    {
        private readonly DataContext _dataContext = dataContext;

        public IActionResult Index(int page = 1, int pageSize = 3)
        {
            var query = _dataContext
    .ProductGroups
    .Include(g => g.Children)
    .Where(g => g.IsHidden == 0 && g.ParentId == null)
    .OrderBy(g => g.OrderInPrice);

            int totalItems = query.Count();

            int totalPages = (int)Math.Ceiling(
                (double)totalItems / pageSize
            );

            if (page < 1)
                page = 1;

            if (page > totalPages && totalPages > 0)
                page = totalPages;

            var groups = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArray();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(groups);

        }
    }
}