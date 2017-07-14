using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Membership;

namespace MultiTenantedUsers
{
    public interface IMultiTenancyRepository
    {
        // Create & Update
        void AssociateUserWithOrganisation(int userId, int orgNodeId);
        void AssociateMemberWithOrganisation(int memberId, int orgNodeId);

        // Read
        bool IsAssociated(int userId, int orgNodeId);
        IEnumerable<IUser> GetOrganisationUsers(int orgNodeId);
        IEnumerable<IMember> GetOrganisationMembers(int orgNodeId);
        int? GetOrgIdForUser(int userId);
        
        // Delete
        void RemoveAssociation(int userId, int orgNodeId);

    }
}