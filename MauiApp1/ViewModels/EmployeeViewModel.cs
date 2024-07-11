using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiApp1.ViewModels
{
    public partial class EmployeeViewModel : ObservableObject
    {
        private readonly HttpClientService _httpClientService;

        [ObservableProperty]
        private ObservableCollection<Employee> _employees = new ObservableCollection<Employee>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNextButtonEnabled))]
        private Employee _selectedEmployee;

        public bool IsNextButtonEnabled => SelectedEmployee != null;

        public string SelectedEmployeeDetails => SelectedEmployee?.EmployeeDetails ?? string.Empty;

        public EmployeeViewModel(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            LoadEmployeesCommand = new AsyncRelayCommand<string>(LoadEmployeesAsync);
            OnSearchBarTextChangedCommand = new AsyncRelayCommand<string>(OnSearchBarTextChanged);
            LoadEmployeesCommand.Execute(null);
        }

        public IAsyncRelayCommand<string> LoadEmployeesCommand { get; }
        public IAsyncRelayCommand<string> OnSearchBarTextChangedCommand { get; }

        private async Task LoadEmployeesAsync(string pattern)
        {
            await DataLoader.LoadDataAsync(Employees, () => _httpClientService.GetEmployeesAsync(pattern));
        }


        private void PopulateEmployeesList(IEnumerable<Employee> employees)
        {
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private async Task OnSearchBarTextChanged(string newTextValue)
        {
            await LoadEmployeesAsync(newTextValue);
        }
    }
}
