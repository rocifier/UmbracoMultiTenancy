using MultiTenantedUsers.Data;
using System.Linq;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Persistence;
using Umbraco.Core.Models;

namespace MultiTenantedUsers.Implementation
{
    public class MultiTenancyRepository : IMultiTenancyRepository
    {
        private static readonly UmbracoDatabase Database = ApplicationContext.Current.DatabaseContext.Database;

        public void AssociateUserWithOrganisation(int userId, int orgNodeId)
        {
            Database.BeginTransaction();
            try
            {

                IEnumerable<MultiTenantedUser> existingAssociations = Database.Fetch<MultiTenantedUser>("SELECT * FROM multiTenantedUsers WHERE identityId = @0", userId).ToList();
                foreach (MultiTenantedUser user in existingAssociations)
                {
                    Database.Delete<MultiTenantedUser>(user);
                }
                Database.Insert(new MultiTenantedUser()
                {
                    IdentityId = userId,
                    OrganisationNodeId = orgNodeId
                });
            }
            catch
            {
                Database.AbortTransaction();
                return;
            }
            Database.CompleteTransaction();
        }

        public void AssociateMemberWithOrganisation(int memberId, int orgNodeId)
        {
            Database.BeginTransaction();
            try
            {
                IEnumerable<MultiTenantedMember> existingAssociations = Database.Fetch<MultiTenantedMember>("SELECT * FROM multiTenantedMembers WHERE memberNodeId = @0", memberId).ToList();
                foreach (MultiTenantedMember user in existingAssociations)
                {
                    Database.Delete<MultiTenantedMember>(user);
                }
                Database.Insert(new MultiTenantedMember()
                {
                    MemberNodeId = memberId,
                    OrganisationNodeId = orgNodeId
                });
            }
            catch
            {
                Database.AbortTransaction();
                return;
            }
            Database.CompleteTransaction();
        }

        public IEnumerable<IUser> GetOrganisationUsers(int orgNodeId)
        {
            List<MultiTenantedUser> users = Database.Fetch<MultiTenantedUser>("SELECT * FROM multiTenantedUsers WHERE orgNodeId = @0", orgNodeId);
            return users.Select(multiUser => ApplicationContext.Current.Services.UserService.GetUserById(multiUser.IdentityId)); // warning: fetches identity users 1-by-1
        }

        public IEnumerable<IMember> GetOrganisationMembers(int orgNodeId)
        {
            List<MultiTenantedMember> users = Database.Fetch<MultiTenantedMember>("SELECT * FROM multiTenantedMembers WHERE orgNodeId = @0", orgNodeId);
            return users.Select(multiUser => ApplicationContext.Current.Services.MemberService.GetById(multiUser.MemberNodeId)); // warning: fetches members 1-by-1
        }

        public int? GetOrgIdForUser(int userId)
        {
            return Database.ExecuteScalar<int?>("SELECT TOP 1 orgNodeId FROM multiTenantedUsers WHERE identityId = @0", userId);
        }

        public bool IsAssociated(int userId, int orgNodeId)
        {
            var foundUser = Database.ExecuteScalar<MultiTenantedUser>("SELECT * FROM multiTenantedUsers WHERE identityId = @0 AND orgNodeId = @1", userId, orgNodeId);
            return foundUser != null;
        }

        public void RemoveAssociation(int userId, int orgNodeId)
        {
            Database.Execute("DELETE FROM multiTenantedUsers WHERE identityId = @0 AND orgNodeId = @1", userId, orgNodeId);
        }
    }
}