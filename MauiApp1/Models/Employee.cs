namespace MauiApp1.Models
{
    public class Employee
    {
        public string? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        public string FullDetails => $"{EmployeeId} {EmployeeName}";
    }
}