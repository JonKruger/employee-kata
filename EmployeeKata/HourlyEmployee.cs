using System;
using System.Collections.Generic;

namespace EmployeeKata
{
    public class HourlyEmployee : Employee
    {
        public override EmployeeType EmployeeType => EmployeeType.Hourly;
        public double HourlyRate { get; set; }

        public override IEnumerable<PayableHours> GetPayableHours(double hoursWorked, PayPeriod payPeriod)
        {
            var payableHours = new List<PayableHours>();
            payableHours.Add(GetPayableHoursForStandardHours(hoursWorked, payPeriod));
            payableHours.Add(GetPayableHoursForOvertimeHours(hoursWorked, payPeriod));
            return payableHours;
        }

        private PayableHours GetPayableHoursForOvertimeHours(double hoursWorked, PayPeriod payPeriod)
        {
            return new PayableHours { Hours = Math.Max(hoursWorked - payPeriod.HoursInPayPeriod, 0), Rate = HourlyRate * 1.5 };
        }

        private PayableHours GetPayableHoursForStandardHours(double hoursWorked, PayPeriod payPeriod)
        {
            return new PayableHours { Hours = Math.Min(hoursWorked, payPeriod.HoursInPayPeriod), Rate = HourlyRate };
        }
    }
}