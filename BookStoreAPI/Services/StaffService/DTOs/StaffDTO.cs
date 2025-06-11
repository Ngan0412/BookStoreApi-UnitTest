namespace BookStoreAPI.Services.StaffService.DTOs
{
    public class StaffDTO
    {
        public Guid Id { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CitizenIdentification { get; set; }
        public bool Role { get; set; }
        public bool Gender { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
    }
}
