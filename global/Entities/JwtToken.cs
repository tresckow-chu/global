namespace BPSGobalClient.Entities
{

    public class JwtToken
    {
        public string Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}

