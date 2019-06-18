namespace dotnetWebApi.Models
{
    public class TransactionExt : Transaction
    {
        public string TargetUserName { get; set; }
        public int TargetUserBalance { get; set; }
    }
}
