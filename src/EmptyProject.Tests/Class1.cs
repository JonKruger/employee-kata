using System;
using System.Collections.Generic;
using NUnit.Framework;
using Should.Extensions.AssertExtensions;

namespace EmptyProject.Tests
{
    [TestFixture]
    public class Class1
    {
        private BiWeeklyPayPeriod _payPeriod;
        private SalariedEmployee _employee;
        private Timecard _timecard;
        private PayrollResult _payrollResult;

        [Test]
        public void Run_payroll_for_a_salaried_employee()
        {
            Given_a_bi_weekly_pay_period();
            Given_a_salaried_employee_whose_salary_is_60000();
            Given_the_employee_has_worked_85_hours_in_the_pay_period();
            When_I_run_payroll();
            Then_the_employee_should_be_paid_the_hourly_rate_for_all_hours_worked_in_the_pay_period();
        }

        // Helper methods

        private void Given_a_bi_weekly_pay_period()
        {
            _payPeriod = new BiWeeklyPayPeriod();
        }

        private void Given_a_salaried_employee_whose_salary_is_60000()
        {
            _employee = new SalariedEmployee {Salary = 60000};
        }

        private void Given_the_employee_has_worked_85_hours_in_the_pay_period()
        {
            _timecard = new Timecard {Employee = _employee, HoursWorked = 85};
        }

        private void When_I_run_payroll()
        {
            var system = new PayrollSystem();
            _payrollResult = system.RunPayroll(_payPeriod, new[] {_timecard});
        }

        private void Then_the_employee_should_be_paid_the_hourly_rate_for_all_hours_worked_in_the_pay_period()
        {
            _payrollResult.GetAmountEmployeeWasPaid(_employee).ShouldEqual(60000m/2080*85);
        }

    }

    public class Timecard
    {
        public SalariedEmployee Employee { get; set; }

        public decimal HoursWorked { get; set; }
    }

    public class PayrollSystem
    {
        public PayrollResult RunPayroll(BiWeeklyPayPeriod payPeriod, IEnumerable<Timecard> timecards)
        {
            
        }
    }

    public class PayrollResult
    {
        public decimal GetAmountEmployeeWasPaid(SalariedEmployee employee)
        {
            throw new NotImplementedException();
        }
    }

    public class SalariedEmployee
    {
        public decimal Salary { get; set; }
    }

    public class BiWeeklyPayPeriod
    {
    }
}