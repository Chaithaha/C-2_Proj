namespace CreativeColab.Models.ViewModels
{
    public class PaymentViewModel
    {
        public List<Payment>? Payments { get; set; }
        public Payment? Payment { get; set; }
        public List<User>? AvailableUsers { get; set; }
        public List<Project>? AvailableProjects { get; set; }
        public List<Payment>? UserPayments { get; set; }
        public List<Payment>? ProjectPayments { get; set; }
    }
}