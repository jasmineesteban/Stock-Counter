namespace MauiApp1.Models
{
    public class Employee
    {
        public string? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        public string? EmployeeDetails
        {
            get
            {
                return $"{EmployeeId} - {EmployeeName}";
            }
        }
    }
}
