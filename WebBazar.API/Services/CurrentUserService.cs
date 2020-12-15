using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebBazar.API.Extensions;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) 
        {
            this.user = httpContextAccessor.HttpContext?.User;
        }

        public string GetUserName()
        {
            return this.user?.Identity?.Name;
        }

        public int GetId()
        {
            var userId = this.user?.GetId();

            if (userId == null)
            {
                return 0;
            }

            return int.Parse(userId);
        }
    }
}