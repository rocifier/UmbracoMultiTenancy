using MultiTenantedUsers.Implementation;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace MultiTenantedUsers.Controllers
{
    [PluginController("UmbracoApi")]
    public class MultiTenancyController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public bool EnsureMemberTenancy([FromUri]int memberId)
        {
            IMultiTenancyRepository repository = new MultiTenancyRepository();
            var currentOrg = repository.GetOrgIdForUser(Security.GetUserId());
            if (currentOrg == null) return false;
            repository.AssociateMemberWithOrganisation(memberId, currentOrg.Value);
            return true;
        }

        [HttpPost]
        public bool EnsureUserTenancy([FromUri]int userId)
        {
            IMultiTenancyRepository repository = new MultiTenancyRepository();
            var currentOrg = repository.GetOrgIdForUser(Security.GetUserId());
            if (currentOrg == null) return false;
            repository.AssociateUserWithOrganisation(userId, currentOrg.Value);
            return true;
        }

        /*
        [HttpPost]
        public bool CreateTenant(MultiTenantedUserViewModel request)
        {
            IUser newUser = Services.UserService.CreateWithIdentity(request.Username, request.Email, request.Password, request.UserType);
            MultiTenancyRepository.AssociateUserWithOrganisation(newUser.Id, request.OrganisationNodeId);
            return true;
        }

        [HttpGet]
        public IEnumerable<MultiTenantedUserViewModel> GetOrganisationTenants([FromUri]int orgId)
        {
            return MultiTenancyRepository.GetOrganisationUsers(orgId).Select(u => new MultiTenantedUserViewModel()
            {
                UserId = u.Id,
                Email = u.Email,
                OrganisationNodeId = orgId,
                Password = null,
                Username = u.Username,
                UserType = u.UserType.Alias
            });
        }
        */

    }
}