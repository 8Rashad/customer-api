namespace TaskApi.Request
{
    public class CustomerRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public decimal Balance { get; set; }
    }
}
