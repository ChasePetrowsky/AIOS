using System.ComponentModel.DataAnnotations;

namespace AIOS.Models
{
    public class FinanceCalculation
    {
        [Required(ErrorMessage = "Vehicle price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Vehicle price must be greater than 0")]
        [Display(Name = "Vehicle Price")]
        public decimal VehiclePrice { get; set; }

        [Required(ErrorMessage = "Credit score is required")]
        [Range(300, 850, ErrorMessage = "Credit score must be between 300 and 850")]
        [Display(Name = "Credit Score")]
        public int CreditScore { get; set; }

        [Required(ErrorMessage = "Loan term is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Loan term must be greater than 0")]
        [Display(Name = "Loan Term")]
        public int LoanTerm { get; set; }

        [Required(ErrorMessage = "Term unit is required")]
        [Display(Name = "Term Unit")]
        public string TermUnit { get; set; } = "Months";

        [Range(0, double.MaxValue, ErrorMessage = "Down payment cannot be negative")]
        [Display(Name = "Down Payment")]
        public decimal DownPayment { get; set; } = 0;

        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }

        [Display(Name = "Monthly Payment")]
        public decimal MonthlyPayment { get; set; }

        [Display(Name = "Total Amount Paid")]
        public decimal TotalPaid { get; set; }
    }
}

