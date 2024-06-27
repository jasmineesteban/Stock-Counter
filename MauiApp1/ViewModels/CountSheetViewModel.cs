using System;
using System.Threading.Tasks;
using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.ViewModels
{
    public class CountSheetViewModel
    {
        private readonly CountSheetService _countSheetService;

        public CountSheetViewModel(CountSheetService countSheetService)
        {
            _countSheetService = countSheetService;
        }

        public async Task AddCountSheet(string employeeCode, string description, DateTime date)
        {
            var countSheet = new CountSheetAddition
            {
                CountSheetEmployee = employeeCode,      
                CountDescription = description,
                CountDate = date
            };

            await _countSheetService.AddCountSheetAsync(countSheet);
        }
    }
}
