using System.Threading.Tasks;
using System.Collections.Generic;
using WebApp.API.Models;
using WebApp.API.Helpers;

namespace WebApp.API.Data
{
    public interface IAdsRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<Ad>> GetAds(UserParams userParams);
         Task<Ad> GetAd(int id);
         IEnumerable<Ad> GetUserAds(int userId);
         Category GetAdCategory(int categoryId);
         IEnumerable<Category> GetCategories();
         Task<Like> GetLike(int userId, int adId);
         int GetAdLikesCount(int adId);
         IEnumerable<Ad> GetUserFavorites(int userId);
         Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForAd(int adId);
    }
}