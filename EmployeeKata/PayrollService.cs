using System;
using System.Linq;

namespace EmployeeKata
{
    public class PayrollService
    {
        public const string HoursWorkedMustBeGreaterThanZeroErrorMessage = "Hours worked must be greater than 0.";
        public double RunPayroll(Employee employee, PayPeriod payPeriod, double hoursWorked)
        {
            if (hoursWorked < 0)
                throw new Exception(HoursWorkedMustBeGreaterThanZeroErrorMessage);

            var payableHours = employee.GetPayableHours(hoursWorked, payPeriod);
            return payableHours.Sum(rs => Math.Round((double) (rs.Hours * rs.Rate), 2, MidpointRounding.AwayFromZero));
        }
    }
}