namespace dotnetWebApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Date { get; set; }
        public int OwnerUserId { get; set; }
        public int TargetUserId { get; set; }
        //public string TargetUserName { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
    }
}
