using System;
using NUnit.Framework;
using Should;
using Should.Core.Assertions;

namespace EmployeeKata.Tests
{
    [TestFixture]
    public class PayrollServiceTests
    {
        private PayPeriod _payPeriod;
        private Employee _employee;
        private double _hoursWorked;
        private double _paymentAmount;
        private Exception _exception;

        [TestCase("overtime", 80, 85, 2307.69)]
        [TestCase("partial week", 80, 45, 2307.69)]
        [TestCase("0 hours", 80, 0, 2307.69)]
        [TestCase("one week pay period", 40, 85, 1153.85)]
        public void RunPayrollForSalariedEmployee(string scenario, double payPeriodHours, double hoursWorked, double expectedPaymentAmount)
        {
            GivenAPayPeriodWithHours(payPeriodHours);
            GivenASalariedEmployeeWhoseSalaryIs(60000);
            GivenTheEmployeeHasWorkedHoursInThePayPeriod(hoursWorked);
            WhenIRunPayroll();
            ThenTheEmployeeShouldBePaid(expectedPaymentAmount);
        }

        [TestCase("overtime", 80, 85, 6562.50)]
        [TestCase("partial week", 80, 47.282, 3546.15)]
        [TestCase("0 hours", 80, 0, 0)]
        [TestCase("one week pay period", 40, 85, 8062.50)]
        public void RunPayrollForHourlyEmployee(string scenario, double payPeriodHours, double hoursWorked, double expectedPaymentAmount)
        {
            GivenAPayPeriodWithHours(payPeriodHours);
            GivenAHourlyEmployeeWhoseHourlyRateIs(75);
            GivenTheEmployeeHasWorkedHoursInThePayPeriod(hoursWorked);
            WhenIRunPayroll();
            ThenTheEmployeeShouldBePaid(expectedPaymentAmount);
        }

        [Test]
        public void NegativeHoursWorked()
        {
            GivenAPayPeriodWithHours(80);
            GivenAHourlyEmployeeWhoseHourlyRateIs(75);
            GivenTheEmployeeHasWorkedHoursInThePayPeriod(-1);
            WhenIRunPayroll();
            ThenAnErrorShouldBeThrownThatSays(PayrollService.HoursWorkedMustBeGreaterThanZeroErrorMessage);
        }

        [Test]
        public void NegativeHoursInPayPeriod()
        {
            GivenAPayPeriodWithHours(-80);
            ThenAnErrorShouldBeThrownThatSays(PayPeriod.HoursInPayPeriodMustBeGreaterThanZeroErrorMessage);
        }

        private void GivenAPayPeriodWithHours(double hours)
        {
            var exception = Record.Exception(() => _payPeriod = new PayPeriod {HoursInPayPeriod = hours});
            _exception = exception ?? _exception;
        }

        private void GivenASalariedEmployeeWhoseSalaryIs(double salary)
        {
            _employee = new SalariedEmployee { Salary = salary };
        }

        private void GivenAHourlyEmployeeWhoseHourlyRateIs(double hourlyRate)
        {
            _employee = new HourlyEmployee { HourlyRate = hourlyRate };
        }

        private void GivenTheEmployeeHasWorkedHoursInThePayPeriod(double hours)
        {
            _hoursWorked = (double)hours;
        }

        private void WhenIRunPayroll()
        {
            var exception = Record.Exception(
                () => _paymentAmount = new PayrollService().RunPayroll(_employee, _payPeriod, _hoursWorked));
            _exception = exception ?? _exception;
        }

        private void ThenTheEmployeeShouldBePaid(double amount)
        {
            _paymentAmount.ShouldEqual((double)amount);
        }

        private void ThenAnErrorShouldBeThrownThatSays(string message)
        {
            _exception.Message.ShouldEqual(message);
        }
    }
}