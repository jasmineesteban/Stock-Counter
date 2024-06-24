using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

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

        public EmployeeViewModel(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            LoadEmployeesCommand = new AsyncRelayCommand<string>(LoadEmployeesAsync);
            OnSearchBarTextChangedCommand = new AsyncRelayCommand<string>(OnSearchBarTextChanged);
            LoadEmployeesCommand.Execute(null);
        }

        public IAsyncRelayCommand<string> LoadEmployeesCommand { get; }
        public IAsyncRelayCommand<string> OnSearchBarTextChangedCommand { get; }
        public IAsyncRelayCommand OnNextButtonClickedCommand { get; }

        private async Task LoadEmployeesAsync(string pattern)
        {
            try
            {
                var databaseName = await _httpClientService.GetRemoteDatabaseAsync();
                var employees = await _httpClientService.GetEmployeesAsync(databaseName, pattern);
                PopulateEmployeesList(employees);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading employees: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load employees.", "OK");
            }
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
