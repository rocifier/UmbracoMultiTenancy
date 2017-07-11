using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MultiTenantedUsers.Data
{
    [TableName("multiTenantedUsers")]
    [PrimaryKey("pk")]
    [ExplicitColumns]
    public class MultiTenantedUser
    {
        [Column("pk")]
        [PrimaryKeyColumn]
        public int PrimaryKey { get; set; }

        [Column("identityId")]
        public int IdentityId { get; set; }

        [Column("orgNodeId")]
        public int OrganisationNodeId { get; set; }
        
    }
}
