namespace CreativeColab.Models
{
    public class Installment
    {
        public int InstallmentId { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }

        // Navigation
        public Project Project { get; set; }
    }

}
