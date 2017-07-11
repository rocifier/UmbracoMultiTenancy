using MultiTenantedUsers.Data;
using System.Linq;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Persistence;

namespace MultiTenantedUsers.Implementation
{
    public class MultiTenancyRepository : IMultiTenancyRepository
    {
        private static readonly UmbracoDatabase Database = ApplicationContext.Current.DatabaseContext.Database;

        public void AssociateUserWithOrganisation(int userId, int orgNodeId)
        {
            Database.Insert(new MultiTenantedUser()
            {
                IdentityId = userId,
                OrganisationNodeId = orgNodeId
            });
        }

        public IEnumerable<IUser> GetOrganisationUsers(int orgNodeId, int page, int usersPerPage)
        {
            Page<MultiTenantedUser> users = Database.Page<MultiTenantedUser>(page, usersPerPage, "SELECT * FROM @0 WHERE orgNodeId = @1", MultiTenantedConstants.MULTITENANCY_TABLE_NAME, orgNodeId);
            return users.Items.Select(multiUser => ApplicationContext.Current.Services.UserService.GetUserById(multiUser.IdentityId)); // warning: fetches identity users 1-by-1
        }

        public bool IsAssociated(int userId, int orgNodeId)
        {
            var foundUser = Database.ExecuteScalar<MultiTenantedUser>("SELECT * FROM @0 WHERE identityId = @1 AND orgNodeId = @2", MultiTenantedConstants.MULTITENANCY_TABLE_NAME, userId, orgNodeId);
            return foundUser != null;
        }

        public void RemoveAssociation(int userId, int orgNodeId)
        {
            Database.Execute("DELETE FROM @0 WHERE identityId = @1 AND orgNodeId = @2", MultiTenantedConstants.MULTITENANCY_TABLE_NAME, userId, orgNodeId);
        }
    }
}