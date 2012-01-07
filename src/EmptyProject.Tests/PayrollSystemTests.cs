using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Should.Extensions.AssertExtensions;

namespace EmptyProject.Tests
{
    [TestFixture]
    public class PayrollSystemTests
    {
        private BiWeeklyPayPeriod _payPeriod;
        private Employee _employee;
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

        [Test]
        public void Run_payroll_for_an_hourly_employee()
        {
            Given_a_bi_weekly_pay_period();
            Given_an_hourly_employee_whose_hourly_rate_is_75_per_hour();
            Given_the_employee_has_worked_85_hours_in_the_pay_period();
            When_I_run_payroll();
            Then_the_employee_should_be_paid_the_hourly_rate_for_the_first_80_hours_and_time_and_half_for_hours_beyond_that();
        }

        // Helper methods

        private void Given_a_bi_weekly_pay_period()
        {
            _payPeriod = new BiWeeklyPayPeriod();
        }

        private void Given_an_hourly_employee_whose_hourly_rate_is_75_per_hour()
        {
            _employee = new HourlyEmployee {HourlyRate = 75};
        }

        private void Given_a_salaried_employee_whose_salary_is_60000()
        {
            _employee = new SalariedEmployee { Salary = 60000 };
        }

        private void Given_the_employee_has_worked_85_hours_in_the_pay_period()
        {
            _timecard = new Timecard { Employee = _employee, HoursWorked = 85 };
        }

        private void When_I_run_payroll()
        {
            var system = new PayrollSystem();
            _payrollResult = system.RunPayroll(_payPeriod, new[] { _timecard });
        }

        private void Then_the_employee_should_be_paid_the_hourly_rate_for_all_hours_worked_in_the_pay_period()
        {
            _payrollResult.GetAmountEmployeeWasPaid(_employee).ShouldEqual(60000m / 2080 * 85);
        }

        private void Then_the_employee_should_be_paid_the_hourly_rate_for_the_first_80_hours_and_time_and_half_for_hours_beyond_that()
        {
            _payrollResult.GetAmountEmployeeWasPaid(_employee).ShouldEqual((75m * 80) + ((75m * 1.5m) * 5));
        }

    }

    public class HourlyEmployee : Employee
    {
        public decimal HourlyRate { get; set; }
 
        public override decimal CalculatePay(decimal hoursWorked, BiWeeklyPayPeriod payPeriod)
        {
            var straightTimeHours = Math.Min(hoursWorked, payPeriod.HoursPerPeriod);
            var timeAndAHalfHours = Math.Max(hoursWorked - payPeriod.HoursPerPeriod, 0);

            return (straightTimeHours * HourlyRate) + (timeAndAHalfHours * (HourlyRate * 1.5m));
        }
    }

    public class Timecard
    {
        public Employee Employee { get; set; }

        public decimal HoursWorked { get; set; }

        public decimal CalculatePay(Timecard timecard, BiWeeklyPayPeriod payPeriod)
        {
            return timecard.Employee.CalculatePay(HoursWorked, payPeriod);
        }
    }

    public class PayrollSystem
    {
        public PayrollResult RunPayroll(BiWeeklyPayPeriod payPeriod, IEnumerable<Timecard> timecards)
        {
            var result = new PayrollResult();
            foreach (var timecard in timecards)
            {
                result.Paychecks.Add(new Paycheck
                                         {
                                             Employee = timecard.Employee, 
                                             Amount = timecard.CalculatePay(timecard, payPeriod)
                                         });
            }
            return result;
        }
    }

    public class Paycheck
    {
        public Employee Employee { get; set; }
        public decimal Amount { get; set; }
    }

    public class PayrollResult
    {
        public IList<Paycheck> Paychecks { get; private set; }

        public PayrollResult()
        {
            Paychecks = new List<Paycheck>();
        }

        public decimal GetAmountEmployeeWasPaid(Employee employee)
        {
            var paycheck = Paychecks.SingleOrDefault(p => p.Employee == employee);
            return paycheck != null ? paycheck.Amount : 0;
        }
    }

    public class SalariedEmployee : Employee
    {
        private const decimal HoursPerYear = 2080;

        public decimal Salary { get; set; }

        public override decimal CalculatePay(decimal hoursWorked, BiWeeklyPayPeriod payPeriod)
        {
            return Salary / HoursPerYear * hoursWorked;
        }
    }

    public abstract class Employee
    {
        public abstract decimal CalculatePay(decimal hoursWorked, BiWeeklyPayPeriod payPeriod);
    }

    public class BiWeeklyPayPeriod
    {
        public decimal HoursPerPeriod { get { return 80; } }
    }
}