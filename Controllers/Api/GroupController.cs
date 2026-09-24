using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ASPNETITSTEP.Data;
using ASPNETITSTEP.Data.Entities;
using ASPNETITSTEP.Models.Rest;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETITSTEP.Controllers.Api
{
    [Route("api/group")]
    [ApiController]
    public class GroupController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;
        [HttpGet]
        public RestResponse GetAllGroups(int page = 1, int pageSize = 10)
        {
            var query = _dataContext
                .ProductGroups
                .Include(g => g.Children)
                .Where(g => g.IsHidden == 0 && g.ParentId == null)
                .OrderBy(g => g.OrderInPrice);
            int cnt = query.Count();
            RestMetaPagination pagination = new()
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = cnt,
                TotalPages = (int) Math.Ceiling( (float)cnt / pageSize),
            };
            ProductGroup[] groups = query.Skip(pageSize *(page-1)).Take(pageSize).ToArray();
            foreach(var group in groups)
            {
                if (group.ImageUrl.StartsWith('/'))
                {
                    group.ImageUrl = $"{Request.Scheme}://{Request.Host}{group.ImageUrl}";
                }
                
            }
            return new()
            {
                Meta = new()
                {
                    ApiName = "Product Groups",
                    DataType = "json/array",
                    CacheTime = 86_400_000,
                    Manipulations = ["GET"],
                    Links = { 
                        { "self", "/api/group" },
                        { "sub", "/api/group/{slug}" },
                    },
                    Pagination = pagination
                },
                Data = groups
            };   
        }
        [HttpPost]
        public bool CreateNewGroup(Data.Entities.ProductGroup group)
        {
            return true;
        }
    }
}