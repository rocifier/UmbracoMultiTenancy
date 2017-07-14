namespace MultiTenantedUsers.Data
{
    public class MultiTenantedUserViewModel
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public int OrganisationNodeId { get; set; }
    }
}