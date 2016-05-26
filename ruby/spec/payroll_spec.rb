require 'rspec'
require 'rspec/given'

=begin
Scenario: Run payroll for a salaried employee
	Given a bi-weekly pay period
	And a salaried employee whose salary is 60000
	And the employee has worked 85 hours in the pay period
	When I run payroll
  Then the employee should be paid salary / 2080 * 80
	
Scenario: Run payroll for an hourly employee
	Given a bi-weekly pay period
	And an hourly employee whose hourly rate is $75 per hour
	And the employee has worked 85 hours in the pay period
	When I run payroll
  Then the employee should be paid the hourly rate for 80 hours and hourly rate * 1.5 for 5 hours
=end

describe 'payroll' do
  describe 'Run payroll for a salaried employee' do
    Given { a_bi_weekly_pay_period } 
    Given { an_employee :type => :salaried, :salary => 60000 }
    Given { employee_worked_hours 85 }
    When { run_payroll }
    Then { the_employee_should_be_paid 2307.69 }
  end

  describe 'Run payroll for an hourly employee' do
    Given { a_bi_weekly_pay_period } 
    Given { an_employee :type => :hourly, :rate => 75.01 }
    Given { employee_worked_hours 85 }
    When { run_payroll }
    Then { the_employee_should_be_paid 6563.38 }

  end

  def a_bi_weekly_pay_period
    @pay_period = PayPeriod.new 
    @pay_period.type = :bi_weekly
  end

  def an_employee(args)
    @employee = 
    case args[:type]
      when :salaried then create_salaried_employee args
      when :hourly then create_hourly_employee args
    end

    @employee.type = args[:type]
  end

  def create_salaried_employee(args)
    employee = SalariedEmployee.new
    employee.salary = args[:salary].to_f
    employee
  end

  def create_hourly_employee(args)
    employee = HourlyEmployee.new
    employee.rate = args[:rate].to_f
    employee
  end

  def employee_worked_hours(hours)
    @hours = hours
  end

  def run_payroll
    @paycheck = @pay_period.run_payroll @employee, @hours
  end

  def the_employee_should_be_paid(amount)
    expect(@paycheck.amount).to eq(amount)
  end
end

class PayPeriod
  attr_accessor :type

  def hours_per_pay_period
    case type
      when :bi_weekly then 80
      else raise 'not implemented'
    end
  end

  def hours_per_year
    2080
  end

  def run_payroll(employee, hours)
    paycheck = Paycheck.new
    paycheck.hours = hours
    paycheck.amount = employee.calculate_pay_amount hours, self
    paycheck
  end
end

class Paycheck
  attr_accessor :employee, :hours, :amount
end

class Employee
  attr_accessor :type
end

class SalariedEmployee < Employee
  attr_accessor :salary

  def calculate_pay_amount(hours, pay_period)
    (salary / pay_period.hours_per_year * pay_period.hours_per_pay_period).round(2)
  end
end

class HourlyEmployee < Employee
  attr_accessor :rate

  def standard_rate
    rate
  end

  def overtime_rate
    rate * 1.5
  end

  def calculate_pay_amount(hours, pay_period)
    standard_hours = hours < pay_period.hours_per_pay_period ? hours : pay_period.hours_per_pay_period
    overtime_hours = hours > pay_period.hours_per_pay_period ? hours - pay_period.hours_per_pay_period : 0
    ((standard_rate * standard_hours).round(2) + (overtime_rate * overtime_hours).round(2)).round(2)
  end
end
