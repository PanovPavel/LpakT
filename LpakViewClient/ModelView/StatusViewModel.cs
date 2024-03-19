using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LpakBL.Controller;
using LpakBL.Model;

namespace LpakViewClient.ModelView
{
    public class StatusViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StatusOrder> _statuses;

        public StatusViewModel()
        {
            GetLoadedCustomers();
        }
        private async void GetLoadedCustomers()
        {
            List<StatusOrder> statusList = await new StatusOrderController().GetListAsync();
            Statuses = new ObservableCollection<StatusOrder>(statusList);
        }

        public ObservableCollection<StatusOrder> Statuses
        {
            get=> _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged("Statuses");
            }
        }
        private StatusOrder _selectedStatus;

        public StatusOrder SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged("SelectedStatus");
                StatusSelectedChanged?.Invoke(this, value.Id);
            }
        }
        
        public event EventHandler<Guid> StatusSelectedChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        
            
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}