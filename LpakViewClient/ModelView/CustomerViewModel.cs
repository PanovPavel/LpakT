using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using LpakBL.Controller;
using LpakBL.Controller.Exception;
using LpakBL.Model;
using LpakBL.Model.Exception;

namespace LpakViewClient.ModelView
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class CustomerViewModel : INotifyPropertyChanged
    {
        public CustomerViewModel()
        {
            GetLoadedCustomers();
        }
        private async void GetLoadedCustomers()
        {
            List<Customer> customersList = await new CustomerController().GetListAsync();
            Customers = new ObservableCollection<Customer>(customersList);
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
        private bool IsHandledException(Exception ex)
        {
            return ex is IncorrectLongOrNullException ||
                   ex is InvalidTaxNumber ||
                   ex is InvalidDateException ||
                   ex is NotFoundByIdException ||
                   ex is RelatedRecordsException ||
                   ex is UniquenessStatusException;
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
                               }
                               catch (Exception ex) when (IsHandledException(ex))
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
                               await new CustomerController().RemoveAsync(customer.CustomerId);
                               Customers.Remove(customer);
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
                               GetLoadedCustomers();
                               
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
                //GetLoadedCustomers();
                Customers.Insert(0, newCustomer);
            }
            catch (Exception ex) when (IsHandledException(ex))
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.Show();
            }
        }
        /**/


        
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}