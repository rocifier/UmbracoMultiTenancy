using System.Collections.Generic;
using Umbraco.Core.Models.Membership;

namespace MultiTenantedUsers
{
    public interface IMultiTenancyRepository
    {
        // Create
        void AssociateUserWithOrganisation(int userId, int orgNodeId);

        // Read
        bool IsAssociated(int userId, int orgNodeId);
        IEnumerable<IUser> GetOrganisationUsers(int orgNodeId, int page, int usersPerPage);
        
        // Update (delete and recreate, or create a new link)

        // Delete
        void RemoveAssociation(int userId, int orgNodeId);

    }
}