using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1.Helpers
{
    public class ColumnVisibilityHelper : INotifyPropertyChanged
    {
        private bool _showCtr;
        private bool _showItemNo;
        private bool _showDescription;
        private bool _showUom;
        private bool _showBatchLot;
        private bool _showExpiry;
        private bool _showQuantity;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ShowCtr
        {
            get => _showCtr;
            set => SetProperty(ref _showCtr, value);
        }

        public bool ShowItemNo
        {
            get => _showItemNo;
            set => SetProperty(ref _showItemNo, value);
        }

        public bool ShowDescription
        {
            get => _showDescription;
            set => SetProperty(ref _showDescription, value);
        }

        public bool ShowUom
        {
            get => _showUom;
            set => SetProperty(ref _showUom, value);
        }

        public bool ShowBatchLot
        {
            get => _showBatchLot;
            set => SetProperty(ref _showBatchLot, value);
        }

        public bool ShowExpiry
        {
            get => _showExpiry;
            set => SetProperty(ref _showExpiry, value);
        }

        public bool ShowQuantity
        {
            get => _showQuantity;
            set => SetProperty(ref _showQuantity, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
