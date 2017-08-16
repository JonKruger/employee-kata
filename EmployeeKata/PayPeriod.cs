using System;

namespace EmployeeKata
{
    public class PayPeriod
    {
        private double _hoursInPayPeriod;

        public const string HoursInPayPeriodMustBeGreaterThanZeroErrorMessage =
            "Hours in pay period must be greater than 0.";

        public double HoursInPayPeriod
        {
            get => _hoursInPayPeriod;
            set
            {
                if (value < 0)
                    throw new Exception(HoursInPayPeriodMustBeGreaterThanZeroErrorMessage);

                _hoursInPayPeriod = value;
            }
        }
    }
}