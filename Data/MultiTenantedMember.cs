using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MultiTenantedUsers.Data
{
    [TableName("multiTenantedMembers")]
    [PrimaryKey("pk")]
    [ExplicitColumns]
    public class MultiTenantedMember
    {
        [Column("pk")]
        [PrimaryKeyColumn]
        public int PrimaryKey { get; set; }

        [Column("memberNodeId")]
        public int MemberNodeId { get; set; }

        [Column("orgNodeId")]
        public int OrganisationNodeId { get; set; }
        
    }
}
