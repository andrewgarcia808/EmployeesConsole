using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    class Employee
    {
        /******************************************************************/
        /*                         Constructor                            */
        /******************************************************************/
        public Employee(string employeeID, string firstName, string lastName, string payType, double salary, string startDate, string state, double hours)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            PayType = payType;
            Salary = salary;
            StartDate = startDate;
            State = state;
            Hours = hours;
        }
        public Employee(string employeeID)
        {
            string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath("../../Files/Employees.txt"));
            foreach (string line in lines)
            {
                string[] emp = line.Split(',');
                if(emp[0] == employeeID)
                {
                    EmployeeID = emp[0];
                    FirstName = emp[1];
                    LastName = emp[2];
                    PayType = emp[3];
                    Salary = Convert.ToDouble(emp[4]);
                    StartDate = emp[5];
                    State = emp[6];
                    Hours = Convert.ToDouble(emp[7]);
                    break;
                }
            }
        }
        /******************************************************************/
        /*                          Attributes                            */
        /******************************************************************/
        public string EmployeeID{ get;}
        public string FirstName { get; }
        public string LastName { get; }
        public string PayType { get; }
        public double Salary { get; }
        public string StartDate { get; }
        public string State { get; }
        public double Hours { get; }
        /******************************************************************/
        /*                          Functions                             */
        /******************************************************************/
        public double getGrossPay()
        {
            double GrossPay = 0;

            if(PayType == "S")
            {
                GrossPay = Salary / 26;
            }
            else if(PayType == "H")
            {
                double ExtraOverTime = 0;
                double OverTime = 0;
                double Regular = 0;
                if (Hours > 90)
                {
                    ExtraOverTime = Hours - 90;
                    OverTime = Hours - ExtraOverTime - 80;
                    Regular = Hours - ExtraOverTime - OverTime;
                }
                else if(Hours > 80 && Hours <= 90)
                {
                    OverTime = Hours - 80;
                    Regular = Hours - OverTime;
                }
                else
                {
                    Regular = Hours;
                }

                GrossPay = (ExtraOverTime * Salary) + (OverTime * Salary) + (Regular * Salary);
            }
            return GrossPay;
        }
        public double getFederalTax()
        {
            double GrossPay = getGrossPay();
            double FederalTaxRate = 0.15;
            double FederalTax = 0;

            FederalTax = GrossPay * FederalTaxRate;

            return FederalTax;
        }
        public double getStateTax()
        {
            double GrossPay = getGrossPay();
            double StateTaxRate = 0;
            double StateTax = 0;

            if (State == "UT" || State == "WY" || State == "NV")
            {
                StateTaxRate = 0.05;
            }
            else if (State == "CO" || State == "ID" || State == "AZ" || State == "OR")
            {
                StateTaxRate = 0.065;
            }
            else if (State == "WA" || State == "NM" || State == "TX")
            {
                StateTaxRate = 0.07;
            }

            StateTax = GrossPay * StateTaxRate;

            return StateTax;
        }
        public double getNetPay()
        {
            double NetPay = 0;
            double GrossPay = getGrossPay();
            double FederalTax = getFederalTax();
            double StateTax = getStateTax();

            NetPay = GrossPay - FederalTax - StateTax;
            return NetPay;
        }
        public int getNumberOfYearsWorked()
        {
            int NumberOfYearsWorked = 0;
            DateTime start = Convert.ToDateTime(StartDate);
            DateTime current = start;
            while(current <= DateTime.Now)
            {
                current = current.AddYears(1);
                if(current <= DateTime.Now)
                {
                    NumberOfYearsWorked++;
                }
            }
            return NumberOfYearsWorked;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath("../../Files/Employees.txt"));
            List<Employee> AllEmployees = new List<Employee>();
            foreach(string line in lines)
            {
                string[] emp = line.Split(',');
                string EmployeeID = emp[0];
                string FirstName = emp[1];
                string LastName = emp[2];
                string PayType = emp[3];
                double Salary = Convert.ToDouble(emp[4]);
                string StartDate = emp[5];
                string State = emp[6];
                double Hours = Convert.ToDouble(emp[7]);

                AllEmployees.Add(new Employee(EmployeeID, FirstName, LastName, PayType, Salary, StartDate, State, Hours));
            }
            string input = "";
            
            while (input != "0")
            {
                Console.WriteLine("Please Choose From The Following Options");
                Console.WriteLine("1 - Calculate pay checks for every line in the text file.");
                Console.WriteLine("2 - Get A list of the top 15% earners sorted by the number or years worked at the company (highest to lowest), then alphabetically by last name then first.");
                Console.WriteLine("3 - Get A list of all states with median time worked, median net pay, and total state taxes paid.");
                Console.WriteLine("4 - Run all Reports.");
                Console.WriteLine("5 - Get Employee By ID");
                Console.WriteLine("0 - To Exit Program."); 
                Console.WriteLine("");
                input = Console.ReadLine();
                Console.WriteLine("");
                if (input == "1")
                {
                    Console.WriteLine("Generating Report...");
                    getPayChecks(AllEmployees);
                    Console.WriteLine("");
                    Console.WriteLine("Report is Saved on Your Desktop!");
                    Console.WriteLine("");
                }
                else if(input == "2")
                {
                    Console.WriteLine("Generating Report...");
                    getTop15Percent(AllEmployees);
                    Console.WriteLine("");
                    Console.WriteLine("Report is Saved on Your Desktop!");
                    Console.WriteLine("");
                }
                else if (input == "3")
                {
                    Console.WriteLine("Generating Report...");
                    getStateInfo(AllEmployees);
                    Console.WriteLine("");
                    Console.WriteLine("Report is Saved on Your Desktop!");
                    Console.WriteLine("");
                }
                else if (input == "4")
                {
                    Console.WriteLine("Generating Reports...");
                    getPayChecks(AllEmployees);
                    Console.WriteLine("");
                    Console.WriteLine("PayChecks Completed...");
                    Console.WriteLine("");
                    getTop15Percent(AllEmployees);
                    Console.WriteLine("");
                    Console.WriteLine("Top15Percent Completed...");
                    Console.WriteLine("");
                    getStateInfo(AllEmployees);
                    Console.WriteLine("");
                    Console.WriteLine("StateInfo Completed...");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Reports are Saved on Your Desktop!");
                    Console.WriteLine("");
                }
                else if (input == "5")
                {
                    Console.WriteLine("What is the Employee ID?");
                    string id = Console.ReadLine();
                    Console.WriteLine("Getting Employee...");
                    Employee employee = getEmployeeByID(id);
                    if(employee.EmployeeID != null)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Here Is Some Information On The Employee...");
                        Console.WriteLine("Employee ID: " + employee.EmployeeID);
                        Console.WriteLine("First Name: " + employee.FirstName);
                        Console.WriteLine("Last Name: " + employee.LastName);
                        Console.WriteLine("Pay Type: " + employee.PayType);
                        Console.WriteLine("Salary: " + employee.Salary.ToString("$#,##0.00"));
                        Console.WriteLine("Start Date: " + employee.StartDate);
                        Console.WriteLine("State of Residence: " + employee.State);
                        Console.WriteLine("Number of Hours Worked in a 2 Week Period: " + employee.Hours.ToString("###0.00"));
                        Console.WriteLine("Gross Pay: " + employee.getGrossPay().ToString("$#,##0.00"));
                        Console.WriteLine("Federal Taxes: " + employee.getFederalTax().ToString("$#,##0.00"));
                        Console.WriteLine("State Taxes: " + employee.getStateTax().ToString("$#,##0.00"));
                        Console.WriteLine("Net Pay: " + employee.getNetPay().ToString("$#,##0.00"));
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine("That Employee Does Not Exists!");
                    }
                }

                else if (input == "0")
                {
                    Console.WriteLine("Good Bye!");
                }
                else
                {
                    Console.WriteLine("That Was Not A Valid Response!");
                    Console.WriteLine("");
                }
                Console.WriteLine("Press Enter To Continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        static Employee getEmployeeByID(string EmployeeID)
        {
            Employee employee = new Employee(EmployeeID);
            return employee;
        }
        static void getPayChecks(List<Employee> AllEmployees)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EmployeeID", Type.GetType("System.String"));
            dt.Columns.Add("FirstName", Type.GetType("System.String"));
            dt.Columns.Add("LastName", Type.GetType("System.String"));
            dt.Columns.Add("GrossPay", Type.GetType("System.Double"));
            dt.Columns.Add("FederalTax", Type.GetType("System.Double"));
            dt.Columns.Add("StateTax", Type.GetType("System.Double"));
            dt.Columns.Add("NetPay", Type.GetType("System.Double"));

            foreach(Employee employee in AllEmployees)
            {
                DataRow dtRow = dt.NewRow();
                dtRow["EmployeeID"] = employee.EmployeeID;
                dtRow["FirstName"] = employee.FirstName;
                dtRow["LastName"] = employee.LastName;
                dtRow["GrossPay"] = employee.getGrossPay();
                dtRow["FederalTax"] = employee.getFederalTax();
                dtRow["StateTax"] = employee.getStateTax();
                dtRow["NetPay"] = employee.getNetPay();
                dt.Rows.Add(dtRow);
            }
            dt.DefaultView.Sort = "GrossPay DESC";
            dt = dt.DefaultView.ToTable();
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/PayChecks.txt"))
            {
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    StringBuilder output = new StringBuilder();
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        if (col < 3)
                        {
                            output.Append(dt.Rows[row][col].ToString());
                        }
                        else
                        {
                            output.Append(((double)dt.Rows[row][col]).ToString("$#,##0.00"));
                        }
                        if (col != dt.Columns.Count - 1)
                        {
                            output.Append(", ");
                        }
                    }
                    sw.WriteLine(output.ToString());
                    output.Clear();
                }
            }
        }
        static void getTop15Percent(List<Employee> AllEmployees)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FirstName", Type.GetType("System.String"));
            dt.Columns.Add("LastName", Type.GetType("System.String"));
            dt.Columns.Add("NumberOfYearsWorked", Type.GetType("System.Int32"));
            dt.Columns.Add("GrossPay", Type.GetType("System.Double"));

            foreach (Employee employee in AllEmployees)
            {
                DataRow dtRow = dt.NewRow();
                dtRow["FirstName"] = employee.FirstName;
                dtRow["LastName"] = employee.LastName;
                dtRow["NumberOfYearsWorked"] = employee.getNumberOfYearsWorked();
                dtRow["GrossPay"] = employee.getGrossPay();
                dt.Rows.Add(dtRow);
            }
            dt.DefaultView.Sort = "GrossPay DESC";
            dt = dt.DefaultView.ToTable();

            DataTable Top15Percentdt = new DataTable();
            Top15Percentdt.Columns.Add("FirstName", Type.GetType("System.String"));
            Top15Percentdt.Columns.Add("LastName", Type.GetType("System.String"));
            Top15Percentdt.Columns.Add("NumberOfYearsWorked", Type.GetType("System.Int32"));
            Top15Percentdt.Columns.Add("GrossPay", Type.GetType("System.Double"));
            for (int i = 0; i < (dt.Rows.Count * .15); i++)
            {
                DataRow dtRow = Top15Percentdt.NewRow();
                dtRow["FirstName"] = dt.Rows[i][0];
                dtRow["LastName"] = dt.Rows[i][1];
                dtRow["NumberOfYearsWorked"] = dt.Rows[i][2];
                dtRow["GrossPay"] = dt.Rows[i][3];
                Top15Percentdt.Rows.Add(dtRow);
            }
            Top15Percentdt.DefaultView.Sort = "NumberOfYearsWorked DESC, LastName ASC, FirstName ASC";
            Top15Percentdt = Top15Percentdt.DefaultView.ToTable();
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Top15Percent.txt"))
            {
                for (int row = 0; row < Top15Percentdt.Rows.Count; row++)
                {
                    StringBuilder output = new StringBuilder();
                    for (int col = 0; col < Top15Percentdt.Columns.Count; col++)
                    {
                        if (col < 3)
                        {
                            output.Append(Top15Percentdt.Rows[row][col].ToString());
                        }
                        else
                        {
                            output.Append(((double)Top15Percentdt.Rows[row][col]).ToString("$#,##0.00"));
                        }
                        if (col != Top15Percentdt.Columns.Count - 1)
                        {
                            output.Append(", ");
                        }
                    }
                    sw.WriteLine(output.ToString());
                    output.Clear();
                }
            }
        }
        static void getStateInfo(List<Employee> AllEmployees)
        {
            List<string> States = getStates(AllEmployees);
            States = States.OrderBy(x => x).ToList();
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/StateInfo.txt"))
            {
                foreach (string State in States)
                {
                    StringBuilder output = new StringBuilder();
                    List<Employee> EmployeesInState = AllEmployees.Where(x => x.State == State).ToList();
                    double MedianTimeWorked = getMedianTimeWorked(EmployeesInState.OrderBy(x => x.Hours).ToList());
                    double MedianNetPay = getMedianNetPay(EmployeesInState.OrderBy(x => x.getNetPay()).ToList());
                    double StateTaxes = getStateTaxes(EmployeesInState);
                    output.Append(State + ", " + MedianTimeWorked.ToString("#,##0.00") + ", " + MedianNetPay.ToString("$#,##0.00") + ", " + StateTaxes.ToString("$#,##0.00"));
                    sw.WriteLine(output.ToString());
                    output.Clear();
                }
            }
        }
        static double getMedianTimeWorked(List<Employee> EmployeesInState)
        {
            double MedianTimeWorked = 0;
            List<double> TimeWorked = new List<double>();
            foreach(Employee employee in EmployeesInState)
            {
                TimeWorked.Add(employee.Hours);
            }
            MedianTimeWorked = getMedian(TimeWorked);
            return MedianTimeWorked;
        }
        static double getMedianNetPay(List<Employee> EmployeesInState)
        {
            double MedianNetPay = 0;
            List<double> NetPay = new List<double>();
            foreach (Employee employee in EmployeesInState)
            {
                NetPay.Add(employee.Hours);
            }
            MedianNetPay = getMedian(NetPay);
            return MedianNetPay;
        }
        static double getStateTaxes(List<Employee> EmployeesInState)
        {
            double StateTaxes = 0;
            foreach (Employee employee in EmployeesInState)
            {
                StateTaxes += employee.getStateTax();
            }
            return StateTaxes;
        }
        static double getMedian(List<double> values)
        {
            double Median = 0;
            if(values.Count % 2 == 0)
            {
                Median = (values[(values.Count / 2)] + values[(values.Count / 2) - 1]) / 2;
            }
            else
            {
                Median = values[(values.Count / 2)];
            }
            return Median;
        }
        static List<string> getStates(List<Employee> AllEmployees)
        {
            List<string> States = new List<string>();
            foreach (Employee employee in AllEmployees)
            {
                string State = employee.State;
                if (!StateExists(States, State))
                {
                    States.Add(State);
                }
            }
            return States;
        }
        static bool StateExists(List<string> States, string State)
        {
            foreach(string state in States)
            {
                if(state == State)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
