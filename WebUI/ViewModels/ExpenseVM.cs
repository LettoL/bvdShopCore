namespace WebUI.ViewModels
{
    public class ExpenseVM
    {
        public int MoneyWorkerId { get; set; }
        public int MoneyWorkerType { get; set; }
        public int ExpenseCategory { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
        public int For { get; set; }
    }
}
