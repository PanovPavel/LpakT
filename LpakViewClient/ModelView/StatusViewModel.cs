using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LpakBL.Controller;
using LpakBL.Model;

namespace LpakViewClient.ModelView
{
    /// <summary>
    /// ViewModel для работы со статусами заказов
    /// </summary>
    public class StatusViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StatusOrder> _statuses;
        /// <summary>
        /// Конструктор класса StatusViewModel
        /// </summary>
        public StatusViewModel()
        {
            GetLoadedCustomers();
        }
        
        /// <summary>
        /// Метод инициализации коллекции статусов
        /// </summary>
        private async void GetLoadedCustomers()
        {
            List<StatusOrder> statusList = await new StatusOrderController().GetListAsync();
            Statuses = new ObservableCollection<StatusOrder>(statusList);
        }
        /// <summary>
        /// Коллекция статусов
        /// </summary>
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
        
        /// <summary>
        /// Выделенный статус
        /// </summary>
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
        /// <summary>
        /// Событие, возникающее при выборе статуса
        /// </summary>
        public event EventHandler<Guid> StatusSelectedChanged;
        /// <summary>
        /// Событие, возникающее при изменении свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Вызывает событие о изменении значения свойства.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}