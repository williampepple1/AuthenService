using Microsoft.AspNetCore.Identity;

namespace AuthenService.Domain.Entities
{
    public class User: IdentityUser<long>

    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
    public class UserClaim : IdentityUserClaim<long> { }

    public class UserRole : IdentityUserRole<long> { }

    public class UserLogin : IdentityUserLogin<long> { }

    public class RoleClaim : IdentityRoleClaim<long> { }

    public class UserToken : IdentityUserToken<long> { }

}
