using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.API.DTOs.Category;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface ICategoryService
    {
         Task<Result<CategoryToReturnDTO>> CreateAsync(CategoryForCreationDTO model);
         Task<Result> DeleteAsync(int id);
         Task<IEnumerable<CategoryToReturnDTO>> AllAsync();
    }
}