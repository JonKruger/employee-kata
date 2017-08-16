using System.Collections.Generic;

namespace EmployeeKata
{
    public abstract class Employee
    {
        public abstract EmployeeType EmployeeType { get; }

        public abstract IEnumerable<PayableHours> GetPayableHours(double hoursWorked, PayPeriod payPeriod);
    }
}