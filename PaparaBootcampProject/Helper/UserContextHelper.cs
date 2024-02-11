using System.Security.Claims;

namespace PaparaApp.Project.API.Helper
{
    public class UserContextHelper(IHttpContextAccessor httpContextAccessor)
    {
       private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public Guid GetCurrentTenantId(Guid? tenantIdFromParameter)
        {
            var userRole = GetUserRoleFromToken();
            Guid tenantId;

            switch (userRole)
            {
                case "Tenant":
                    tenantId = GetTenantIdFromToken();
                    break;
                case "Manager":
                    tenantId = tenantIdFromParameter ?? throw new ArgumentException("Tenant ID is required for managers.");
                    break;
                default:
                    throw new UnauthorizedAccessException("Access denied.");
            }

            return tenantId;
        }

        public string GetUserRoleFromToken()
        {
            var claimsIdentity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            var roleClaim = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleClaim))
            {
                throw new InvalidOperationException("User role not found in token.");
            }

            return roleClaim;
        }

        public Guid GetTenantIdFromToken()
        {
            var claimsIdentity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            var tenantClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(tenantClaim, out var tenantId))
            {
                return tenantId;
            }

            throw new InvalidOperationException("Tenant ID not found in token.");
        }
    }
}
