namespace InvoiceApp.Dto
{
    public class DtoUserCreateRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }
        public string Country { get; set; }
    }
}
