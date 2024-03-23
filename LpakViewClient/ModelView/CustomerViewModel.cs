using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using LpakBL.Controller;
using LpakBL.Controller.Exception;
using LpakBL.Model;
using LpakBL.Model.Exception;
using LpakViewClient.Event;
using LpakViewClient.Exceptions;
using LpakViewClient.Windows;

namespace LpakViewClient.ModelView
{
  
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class CustomerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Создание экземпляра класса CustomerViewModel. 
        /// </summary>
        public CustomerViewModel()
        {
            GetLoadedCustomers();
            AddNewOrderForCustomerWindow.AddOrderForUser += ViewModelUpdateCustomerOrders;
            OrderViewModel.OrderRemoved += async (object sender, OrderEventArgs orderEventArgs) =>
            {
                Customer selectedCustomerBeforeUpdate = SelectedCustomer;
                await GetLoadedCustomersAsync();
                if(selectedCustomerBeforeUpdate != null) SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerId == selectedCustomerBeforeUpdate.CustomerId);
                OnPropertyChanged("SelectedCustomer");
            };
            OrderViewModel.OrderUpdated += async (object sender, OrderEventArgs orderEventArgs) =>
            {
                Customer selectedCustomerBeforeUpdate = SelectedCustomer;
                await GetLoadedCustomersAsync();
                if(selectedCustomerBeforeUpdate != null) SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerId == selectedCustomerBeforeUpdate.CustomerId);
                OnPropertyChanged("SelectedCustomer");
            };
        }
        
        /// <summary>
        /// Инциализация ObservableCollection<Customer>  клиентами из базы данных. 
        /// </summary>
        private async void GetLoadedCustomers()
        {
            List<Customer> customersList = await new CustomerController().GetListAsync();
            Customers.Clear();
            foreach (var customer in customersList)
            {
                Customers.Add(customer); 
            }
        }
        
        private async Task GetLoadedCustomersAsync()
        {
            List<Customer> customersList = await new CustomerController().GetListAsync();
            Customers.Clear();
            foreach (var customer in customersList)
            {
                Customers.Add(customer); 
            }
        }


        private DateTime _lastDateTimeOrder;
        private RelayCommand _openWindowAddNewOrderFoUser, _updateCustomerOpenWindow, _removeSelectedCustomer, _addNewCustomerOpenWindow, _addCustomerCommand, _removeSelectedOrder, _updateCustomer;
        private string _name, _fieldOfBusinessName,_taxNumber,_comment;
        private Order _selectedOrder;
        private Customer _selectedCustomer;
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        private int _countOrders;

        /// <summary>
        /// Коллекция заказчиков.
        /// </summary>
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged("Customers");
            }
        }
        /// <summary>
        /// Имя заказчика. 
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value.Trim();
                OnPropertyChanged("Name");
            }
        }
        /// <summary>
        /// Область деятельности заказчика. 
        /// </summary>
        public string FieldOfBusinessName
        {
            get => _fieldOfBusinessName;
            set
            {
                _fieldOfBusinessName = value;
                OnPropertyChanged("FieldOfBusinessName");
            }
        }
        /// <summary>
        /// Налоговый номер заказчика. 
        /// </summary>
        public string TaxNumber
        {
            get => _taxNumber;
            set
            {
                _taxNumber = value;
                OnPropertyChanged("TaxNumber");
            }
        }
        /// <summary>
        /// Примечание к заказчику. 
        /// </summary>
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        /// <summary>
        /// Выделенный заказ в GridTable 
        /// </summary>
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        /// <summary>
        /// Выделенный заказчик DataGrid
        /// </summary>
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }
        
        
        /// <summary>
        /// Сбросить свойства текущего дата контекст на Empty
        /// </summary>
        private void ClearProperties()
        {
            Name = string.Empty;
            FieldOfBusinessName = string.Empty;
            TaxNumber = string.Empty;
            Comment = string.Empty;
        }
        /// <summary>
        /// Количество заказов для текущего заказчика. 
        /// </summary>
        public int CountOrders
        {
            get=>_countOrders;
            set
            {
                _countOrders = SelectedCustomer.Orders.Count;
                OnPropertyChanged("CountOrders");
            }
        }
        /// <summary>
        /// Дата последнего заказа для текущего заказчика. 
        /// </summary>
        public DateTime LastDateTimeOrder
        {
            get=> _lastDateTimeOrder;
            set
            {
                _lastDateTimeOrder = value;
                OnPropertyChanged("LastDateTimeOrder");
            }
        }
        /// <summary>
        /// Открыть окно для изменения выделенного нового заказчика.
        /// Команда доступна только при SelectedCustomer != null
        /// </summary>
        public RelayCommand UpdateCustomerOpenWindow
        {
            get
            {
                return _updateCustomerOpenWindow ??
                       (_updateCustomerOpenWindow = new RelayCommand(obj =>
                           {
                               if (obj is CustomerViewModel customerViewModel)
                               {
                                   ChangeCustomerWindow changeCustomerWindow = new ChangeCustomerWindow(customerViewModel);
                                   changeCustomerWindow.Show();
                               }
                           },
                           (obj) => SelectedCustomer != null));
            }
        }

        
        /// <summary>
        /// Изменить текущий заказчик. 
        /// </summary>
        public RelayCommand UpdateCustomer => _updateCustomer ?? (_updateCustomer = new RelayCommand(UpdateCustomerAsync));
        /// <summary>
        /// Изменить текущий заказчик. 
        /// </summary>
        /// <param name="obj">Объект выделенного в DataGrid заказчика</param>
        private async void UpdateCustomerAsync(object obj)
        {
            if (obj is Customer customerUpdated)
            {
                try
                {
                    await new CustomerController().UpdateAsync(customerUpdated);
                    UpdateCustomerEvent(customerUpdated);
                }
                catch (Exception ex) when (ex is IncorrectLongOrNullException)
                {
                    ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                    errorWindow.Show();
                }
            }   
        }
        
        /// <summary>
        /// Удалить выбранный заказчик. 
        /// </summary>
        public RelayCommand RemoveSelectedCustomer
        {
            get
            {
                return _removeSelectedCustomer ?? (_removeSelectedCustomer = new RelayCommand(RemoveSelectedCustomerAsync, (obj) => SelectedCustomer != null));
            }
        }
        /// <summary>
        /// Удалить выбранный заказчик. 
        /// </summary>
        /// <param name="obj">Объект Customer который необходимо удалить</param>
        private async void RemoveSelectedCustomerAsync(object obj)
        {
            if (obj is Customer customer)
            {
                try
                {
                    await new CustomerController().RemoveAsync(customer.CustomerId);
                    Customers.Remove(customer);
                    DeleteCustomerEvent(customer);
                }
                catch (RelatedRecordsException ex)
                {
                    new ErrorWindow(ex.Message).ShowDialog();
                }
            }
        }
        /// <summary>
        /// Удалить выбранный заказ. 
        /// </summary>
        public RelayCommand RemoveSelectedOrder
        {
            get
            {
                return _removeSelectedOrder ?? (_removeSelectedOrder = new RelayCommand(RemoveSelectedOrderAsync, (obj) => SelectedOrder != null));
            }
        }

        /// <summary>
        /// Удалить выбранный заказ. 
        /// </summary>
        /// <param name="obj">Объект Order который необходимо удалить</param>
        private async void RemoveSelectedOrderAsync(object obj)
        {
            if (obj is Order order)
            {
                await new OrderController().RemoveAsync(order.Id);
                SelectedCustomer.Orders.Remove(order);
                Customer selectedCustomerBeforeUpdate = SelectedCustomer;
                await GetLoadedCustomersAsync();
                //SelectedCustomer = selectedCustomerBeforeUpdate;
                SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerId == selectedCustomerBeforeUpdate.CustomerId);
                OrderRemovedEvent(order);
                OnPropertyChanged("SelectedCustomer");
            }
        }
        
        /// <summary>
        /// Открыть окно для добавления нового заказчика.
        /// </summary>
        public RelayCommand AddNewCustomerOpenWindow
        {
            get
            {
                return _addNewCustomerOpenWindow ??
                       (_addNewCustomerOpenWindow = new RelayCommand( obj =>
                           {
                               if (obj is CustomerViewModel customerViewModel)
                               {
                                   var addNewCustomerOpenWindow =
                                       new AddCustomerWindow(customerViewModel);
                                   addNewCustomerOpenWindow.Show();
                               }
                           }
                           , (obj) => true));
            }
        }
        
        /// <summary>
        /// Добавить нового заказчика. 
        /// </summary>
        public RelayCommand AddCustomer
        {
            get
            {
                return _addCustomerCommand ??
                       (_addCustomerCommand = new RelayCommand(AddCustomerExecute, (obj) => true));
            }
        }
        
        /// <summary>
        /// Добавить нового заказчика. 
        /// </summary>
        private async void AddCustomerExecute(object parameter)
        {
            try
            {
                var customerController = new CustomerController();
                var newCustomer = new Customer(Name, TaxNumber, Comment, new FieldOfBusiness(FieldOfBusinessName));
                await customerController.AddAsync(newCustomer);
                ClearProperties();
                Customers.Insert(0, newCustomer);
                AddCustomerEvent(newCustomer);
            }
            catch (Exception ex) when (HandlerException.IsHandledException(ex))
            {
                var errorWindow = new ErrorWindow(ex.Message);
                errorWindow.Show();
            }
        }
        
        
        /// <summary>
        /// Открыть окно для добавления нового заказа.
        /// </summary>
        public RelayCommand OpenWindowAddNewOrderFoUser
        {
            get
            {
                return _openWindowAddNewOrderFoUser ??
                       (_openWindowAddNewOrderFoUser = new RelayCommand(obj =>
                           {
                               if (obj is CustomerViewModel customerViewModel)
                               {
                                   new AddNewOrderForCustomerWindow(customerViewModel).Show();
                               }
                           },
                           (obj) => SelectedCustomer != null));
            }
        }
        private async void ViewModelUpdateCustomerOrders(object sender, Order order)
        {
            SelectedCustomer.Orders.Add(order);
            Customer selectedCustomerBeforeUpdate = SelectedCustomer;
            await GetLoadedCustomersAsync();
            SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerId == selectedCustomerBeforeUpdate.CustomerId);
            AddOrderEvent(order);
            OnPropertyChanged("SelectedCustomer");
        }
        /// <summary>
        /// Событие, возникающее при добавлении нового клиента.
        /// </summary>
        public static event EventHandler<CustomerEventArgs> CustomerAdded;
        /// <summary>
        /// Событие, возникающее при удалении клиента.
        /// </summary>
        public static event EventHandler<CustomerEventArgs> CustomerDeleted;
        /// <summary>
        /// Событие, возникающее при обновлении информации о клиенте.
        /// </summary>
        public static event EventHandler<CustomerEventArgs> CustomerUpdated;
        
        /// <summary>
        /// Событие, возникающее при добавлении нового заказа.
        /// </summary>
        public static event EventHandler<OrderEventArgs> OrderAdded;
        /// <summary>
        /// Событие, возникающее при удалении заказа.
        /// </summary>
        public static event EventHandler<OrderEventArgs> OrderRemoved;
        
        /// <summary>
        /// Инициирует событие о добавлении нового заказа.
        /// </summary>
        /// <param name="order">Добавленный заказ.</param>
        public static void AddOrderEvent(Order order)
        {
            OrderAdded?.Invoke(null, new OrderEventArgs(order));
        }
        
        /// <summary>
        /// Инициирует событие о удалении заказа.
        /// </summary>
        /// <param name="order">Удаленный заказ.</param>
        public static void OrderRemovedEvent(Order order)
        {
            OrderRemoved?.Invoke(null, new OrderEventArgs(order));
        }
        /// <summary>
        /// Инициирует событие о добавлении нового клиента.
        /// </summary>
        /// <param name="customer">Добавленный клиент.</param>
        public static void AddCustomerEvent(Customer customer)
        {
            //  добавления Customer
            CustomerAdded?.Invoke(null, new CustomerEventArgs(customer));
        }
        /// <summary>
        /// Инициирует событие о удалении клиента.
        /// </summary>
        /// <param name="customer">Удаленный клиент.</param>
        public static void DeleteCustomerEvent(Customer customer)
        {
            //  удаления Customer
            CustomerDeleted?.Invoke(null, new CustomerEventArgs(customer));
        }
        /// <summary>
        /// Инициирует событие об обновлении информации о клиенте.
        /// </summary>
        /// <param name="customer">Обновленный клиент.</param>
        public static void UpdateCustomerEvent(Customer customer)
        {
            //  обновления Customer
            CustomerUpdated?.Invoke(null, new CustomerEventArgs(customer));
        }
        /// <summary>
        /// Событие, возникающее при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Вызывает событие о изменении значения свойства.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}