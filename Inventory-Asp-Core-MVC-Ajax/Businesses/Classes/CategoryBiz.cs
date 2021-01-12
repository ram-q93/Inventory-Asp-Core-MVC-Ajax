using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using System.Linq;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class CategoryBiz : ICategoryBiz
    {
        private readonly IRepository _repository;
        private readonly InventoryDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryBiz(IRepository repository, InventoryDbContext dbContext, IMapper mapper)
        {
            _repository = repository;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Result<object> ListName() =>
            Result<object>.Try(() =>
           {
               var result = _dbContext.Categories.Select(c => new { c.Id, c.Name }).
                    OrderBy(c => c.Name).ToList();
               return Result<object>.Successful(result);
           });



    }
}
