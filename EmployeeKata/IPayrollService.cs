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
        /// <param name="employeeType">The type of employee (salaried or hourly).</param>
        /// <param name="rate">The employee's rate</param>
        /// <param name="rateUnitInHours">
        /// The number of hours that the rate applies to.
        /// For example:
        /// For hourly, this will likely be 1 and the rate would be the hourly rate.
        /// For salaried, this will likely be 2080 and the rate would be their annual salary.</param>
        /// <param name="hours">The number of hours worked in this pay period.</param>
        /// <returns>The amount to pay the employee.</returns>
        decimal RunPayroll(EmployeeType employeeType, decimal rate, decimal rateUnitInHours, decimal hours);
    }
}
