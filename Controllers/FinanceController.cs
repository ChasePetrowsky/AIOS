using Microsoft.AspNetCore.Mvc;
using AIOS.Models;

namespace AIOS.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Calculator()
        {
            return View(new FinanceCalculation());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculator(FinanceCalculation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Map credit score to interest rate
            model.InterestRate = GetInterestRate(model.CreditScore);

            // Convert loan term to months
            int months = model.TermUnit == "Years" ? model.LoanTerm * 12 : model.LoanTerm;

            // Calculate principal (price - down payment)
            decimal principal = model.VehiclePrice - model.DownPayment;

            // Calculate monthly payment using amortization formula
            // M = P * [r(1+r)^n] / [(1+r)^n - 1]
            // Where P = principal, r = monthly interest rate, n = number of months
            decimal monthlyRate = model.InterestRate / 100 / 12;
            
            if (monthlyRate == 0)
            {
                // If interest rate is 0, simple division
                model.MonthlyPayment = principal / months;
            }
            else
            {
                decimal numerator = monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months);
                decimal denominator = (decimal)Math.Pow((double)(1 + monthlyRate), months) - 1;
                model.MonthlyPayment = principal * (numerator / denominator);
            }

            // Calculate total amount paid
            model.TotalPaid = model.MonthlyPayment * months;

            return View(model);
        }

        private decimal GetInterestRate(int creditScore)
        {
            if (creditScore >= 800 && creditScore <= 850)
                return 3.0m;
            else if (creditScore >= 740 && creditScore <= 799)
                return 5.0m;
            else if (creditScore >= 670 && creditScore <= 739)
                return 7.0m;
            else if (creditScore >= 580 && creditScore <= 669)
                return 10.0m;
            else if (creditScore >= 300 && creditScore <= 579)
                return 15.0m;
            else
                return 15.0m; // Default to highest rate for invalid scores
        }
    }
}
