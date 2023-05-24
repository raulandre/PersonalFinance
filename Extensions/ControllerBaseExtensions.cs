using Microsoft.AspNetCore.Mvc;

namespace PersonalFinance.Extensions
{
    public static class UserUtils
    {
        public static Guid LoggedUserId(this ControllerBase controller)
        {
            var id = controller.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value;
            if (id is not null) return Guid.Parse(id);
            return Guid.Empty;
        }
    }
}
