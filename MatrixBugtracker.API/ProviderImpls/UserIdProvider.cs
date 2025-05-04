using MatrixBugtracker.DAL.ProviderInterfaces;
using System.Security.Claims;

namespace MatrixBugtracker.API.ProviderImpls
{
    // Sends authorized user's id to data layer.
    public class UserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserIdProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int UserId
        {
            get
            {
                var nameId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                return nameId != null ? int.Parse(nameId.Value) : 0;
            }
        }
    }
}
