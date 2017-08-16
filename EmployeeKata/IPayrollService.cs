using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeKata
{
    public interface IPayrollService
    {
        /// <summary>
        /// Calculate the amount to pay an employee for an 80 hour pay period.
        /// </summary>
        /// <returns>The amount to pay the employee.</returns>
        double RunPayroll(Employee employee, PayPeriod payPeriod, double hoursWorked);
    }
}
