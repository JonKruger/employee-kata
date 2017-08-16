using System.Collections.Generic;

namespace EmployeeKata
{
    public class SalariedEmployee : Employee
    {
        public double Salary { get; set; }

        private double HourlyRate => Salary / 2080;

        public override EmployeeType EmployeeType => EmployeeType.Salaried;

        public override IEnumerable<PayableHours> GetPayableHours(double hoursWorked, PayPeriod payPeriod)
        {
            return new[] { new PayableHours { Hours = payPeriod.HoursInPayPeriod, Rate = HourlyRate } };
        }
    }
}