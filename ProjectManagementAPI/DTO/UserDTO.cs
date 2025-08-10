namespace ProjectManagementAPI.DTO
{
    public class UserDTO
    {
        public String Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }

        // Plain-text password, sent only when registering or updating user
        public String Password { get; set; }

        public RoleDTO Role { get; set; }

    }
}
