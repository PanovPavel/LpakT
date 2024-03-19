using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using LpakBL.Controller;
using LpakBL.Controller.Exception;
using LpakBL.Model;
using LpakBL.Model.Exception;
using LpakViewClient.Event;
using LpakViewClient.Windows;

namespace LpakViewClient.ModelView
{
  
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class CustomerViewModel : INotifyPropertyChanged
    {
        public CustomerViewModel()
        {
            GetLoadedCustomers();
            AddNewOrderForCustomerWindow.AddOrderForUser += ViewModelUpdateCustomerOrders;
            OrderViewModel.OrderRemoved += (object sendler, OrderEventArgs orderEventArgs) => {GetLoadedCustomers(); };
            OrderViewModel.OrderUpdated += (object sendler, OrderEventArgs orderEventArgs) => {GetLoadedCustomers(); };
        }
        
        

        private async void GetLoadedCustomers()
        {
            List<Customer> customersList = await new CustomerController().GetListAsync();
            Customers.Clear(); // Очистка существующего списка
            foreach (var customer in customersList)
            {
                Customers.Add(customer); // Добавление каждого клиента в существующую коллекцию
            }
        }
        
        private DateTime _lastDateTimeOrder;
        public DateTime LastDateTimeOrder
        {
            get=> _lastDateTimeOrder;
            set
            {
                _lastDateTimeOrder = value;
                OnPropertyChanged("LastDateTimeOrder");
            }
        }
        private RelayCommand _updateCustomerOpenWindow, _removeSelectedCustomer, _addNewCustomerOpenWindow, _addCustomerCommand, _removeSelectedOrder, _updateCustomer;
        private string _name, _fieldOfBusinessName,_taxNumber,_comment;
        private Order _selectedOrder;
        private Customer _selectedCustomer;
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        public string Name
        {
            get => _name;
            set
            {
                _name = value.Trim();
                OnPropertyChanged("Name");
            }
        }
        public string FieldOfBusinessName
        {
            get => _fieldOfBusinessName;
            set
            {
                _fieldOfBusinessName = value;
                OnPropertyChanged("FieldOfBusinessName");
            }
        }
        public string TaxNumber
        {
            get => _taxNumber;
            set
            {
                    _taxNumber = value;
                    if(value == "12") throw new InvalidTaxNumber(nameof(TaxNumber), "Value cannot be null or empty string");
                OnPropertyChanged("TaxNumber");
            }
        }
        
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }
        
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged("Customers");
            }
        }
        
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }


        
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        private void ClearProperties()
        {
            Name = string.Empty;
            FieldOfBusinessName = string.Empty;
            TaxNumber = string.Empty;
            Comment = string.Empty;
        }
        
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
        private int _countOrders;
        public int CountOrders
        {
            get=>_countOrders;
            set
            {
                _countOrders = SelectedCustomer.Orders.Count;
                OnPropertyChanged("CountOrders");
            }
        }
        public RelayCommand UpdateCustomer
        {
            get
            {
                return _updateCustomer ??
                       (_updateCustomer = new RelayCommand(async obj =>
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
                       }));
            }
        }
        
        
        public RelayCommand RemoveSelectedCustomer
        {
            get
            {
                return _removeSelectedCustomer ??
                       (_removeSelectedCustomer = new RelayCommand(async obj =>
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
                       }, (obj) => SelectedCustomer != null));
            }
        }
        
        public RelayCommand RemoveSelectedOrder
        {
            get
            {
                return _removeSelectedOrder??
                       (_removeSelectedOrder = new RelayCommand(async obj =>
                       {
                           if (obj is Order order)
                           {
                               await new OrderController().RemoveAsync(order.Id);
                               Customer old = SelectedCustomer;
                               SelectedCustomer.Orders.Remove(order);
                               SelectedCustomer = null;
                               GetLoadedCustomers();
                               SelectedCustomer = old;
                               OrderRemovedEvent(order);
                           }
                       }, (obj) => SelectedOrder!= null));
            }
        }
        
        /**/
        public RelayCommand AddNewCustomerOpenWindow
        {
            get
            {
                return _addNewCustomerOpenWindow ??
                       (_addNewCustomerOpenWindow = new RelayCommand(async obj =>
                           {
                               if (obj is CustomerViewModel customerViewModel)
                               {
                                   AddCustomerWindow addNewCustomerOpenWindow =
                                       new AddCustomerWindow(customerViewModel);
                                   addNewCustomerOpenWindow.Show();
                               }
                           }
                           , (obj) => true));
            }
        }
        public RelayCommand AddCustomer
        {
            get
            {
                if (_addCustomerCommand == null)
                {
                    _addCustomerCommand = new RelayCommand(AddCustomerExecute, (obj) => true);
                }
                
                return _addCustomerCommand;
            }
        }

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
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.Show();
            }
        }
        
        private RelayCommand _openWindowAddNewOrderFoUser;

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
        private void ViewModelUpdateCustomerOrders(object sender, Order e)
        {
            SelectedCustomer.Orders.Add(e);
            Customer old = SelectedCustomer;
            GetLoadedCustomers();
            SelectedCustomer = old;
            AddOrderEvent(e);
        }
        
        
        /*************/
        public static event EventHandler<CustomerEventArgs> CustomerAdded;
        public static event EventHandler<CustomerEventArgs> CustomerDeleted;
        public static event EventHandler<CustomerEventArgs> CustomerUpdated;
        public static event EventHandler<OrderEventArgs> OrderAdded;
        public static event EventHandler<OrderEventArgs> OrderRemoved;

        public static void AddOrderEvent(Order order)
        {
            OrderAdded?.Invoke(null, new OrderEventArgs(order));
        }

        public static void OrderRemovedEvent(Order order)
        {
            OrderRemoved?.Invoke(null, new OrderEventArgs(order));
        }

        public static void AddCustomerEvent(Customer customer)
        {
            //  добавления Customer
            CustomerAdded?.Invoke(null, new CustomerEventArgs(customer));
        }

        public static void DeleteCustomerEvent(Customer customer)
        {
            //  удаления Customer
            CustomerDeleted?.Invoke(null, new CustomerEventArgs(customer));
        }

        public static void UpdateCustomerEvent(Customer customer)
        {
            //  обновления Customer
            CustomerUpdated?.Invoke(null, new CustomerEventArgs(customer));
        }
        
        
        /***********************************/
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}