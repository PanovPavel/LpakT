using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using LpakBL.Controller;
using LpakBL.Controller.Exception;
using LpakBL.Model;

namespace WpfClient.ViewModel
{
    public class CustomerVM : INotifyPropertyChanged
    {
        private CustomerController _customerController;
        
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged("Customers");
            }
        }
        public string Name
        {
            get { return SelectedCustomer.Name; }
            set
            {
                SelectedCustomer.Name = value;
                OnPropertyChanged("Name");
            }
        }
        
        
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }
        
        //private RelayCommand _removeCommand;
        //public RelayCommand RemoveCommand
        //{
        //    get
        //    {
        //        return _removeCommand ??
        //               (_removeCommand = new RelayCommand(async obj =>
        //               {
        //                   /*try
        //                   {
        //                       await _customerController.RemoveAsync(SelectedCustomer.CustomerId);
        //                   }
        //                   catch (RelatedRecordsException ex)
        //                   {
        //                       //openExceptionWindow
        //                   }*/
        //                   Customers.Remove(SelectedCustomer);
        //               }));
                
        //    }
        //}
        public CustomerVM()
        {   
            _customerController = new CustomerController();
            LoadCustomers();
        }
        
        
        private async void LoadCustomers()
        {
            List<Customer> customersList = await _customerController.GetListAsync();
            Customers = new ObservableCollection<Customer>(customersList);
        }
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}