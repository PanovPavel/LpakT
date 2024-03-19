using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LpakBL.Controller;
using LpakBL.Model;
using LpakViewClient.Event;
using LpakViewClient.Windows;

namespace LpakViewClient.ModelView
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        private Order _selectedOrder;
        private ObservableCollection<Customer> _selectedCustomers;


        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }


        public ObservableCollection<Customer> SelectedCustomers
        {
            get { return _selectedCustomers; }
            set
            {
                _selectedCustomers = value;
                OnPropertyChanged(nameof(SelectedCustomers));
                UpdateFilteredOrders(value);
            }
        }

        /*********************************************/
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

        private ObservableCollection<StatusOrder> _statuses = new ObservableCollection<StatusOrder>();

        public ObservableCollection<StatusOrder> Statuses
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged("Statuses");
            }
        }

        public event EventHandler<Guid> StatusSelectedChanged;
        /******************************************/


        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                OnPropertyChanged("SelectedOrder");
                if (_selectedOrder != null)
                {
                    SelectedStatus = Statuses.FirstOrDefault(status => status.Name == _selectedOrder.Status.Name);
                }
            }
        }


        public OrderViewModel()
        {
            GetData();
            CustomerViewModel.CustomerAdded += AddCustomerEventHandler;
            CustomerViewModel.CustomerDeleted += DeleteCustomerEventHandler;
            CustomerViewModel.CustomerUpdated += UpdateCustomerEventHandler;
            CustomerViewModel.OrderAdded += OrderAddedEventHandler;
            CustomerViewModel.OrderRemoved += OrderRemovedEventHandler;
        }

        private void OrderRemovedEventHandler(object sender, OrderEventArgs e)
        {
            Orders.Remove(e.Order);
        }

        private void OrderAddedEventHandler(object sender, OrderEventArgs e)
        {
            Orders.Insert(0, e.Order);
        }

        private void DeleteCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            int indexRemove = Customers.IndexOf(e.Customer);
            Customers.RemoveAt(indexRemove);
        }

        private void UpdateCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            var customer = Customers.First(c => c.CustomerId == e.Customer.CustomerId);
            int indexOldCustomer = Customers.IndexOf(customer);
            Customers.RemoveAt(indexOldCustomer);
            Customers.Insert(indexOldCustomer, e.Customer);
        }

        private void AddCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            if (e != null && e.Customer != null)
            {
                Customers.Add(e.Customer);
            }
        }

        private async void GetData()
        {
            
            List<Order> orders = await new OrderController().GetListAsync();
            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }

            List<Customer> customers = await new CustomerController().GetListAsync();
            Customers.Clear();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }

            List<StatusOrder> statusList = await new StatusOrderController().GetListAsync();
            Statuses.Clear();
            foreach (var status in statusList)
            {
                Statuses.Add(status);
            }
        }

        private void UpdateFilteredOrders(ObservableCollection<Customer> selectedCustomers)
        {
            if (selectedCustomers.Count == 0)
            {
                GetData();
            }
            else
            {
                Orders = new ObservableCollection<Order>();
                foreach (var customer in selectedCustomers)
                {
                    foreach (var order in customer.Orders)
                    {
                        Orders.Add(order);
                    }
                }
            }
        }

        private RelayCommand _removeSelectedOrder;

        public RelayCommand RemoveSelectedOrder
        {
            get
            {
                return _removeSelectedOrder ?? (
                    _removeSelectedOrder = new RelayCommand(async obj =>
                    {
                        if (obj is Order order)
                        {
                            try
                            {
                                await new OrderController().RemoveAsync(order.Id);
                                Orders.Remove(order);
                                OrderRemovedEvent(order);
                            }
                            catch (Exception e)
                            {
                                new ErrorWindow(e.Message).ShowDialog();
                            }
                        }
                    }, obj => SelectedOrder != null));
            }
        }

        public RelayCommand _openWindowUpdateSelectedOrder;

        public RelayCommand OpenWindowUpdateSelectorCustomer
        {
            get
            {
                return _openWindowUpdateSelectedOrder ?? (
                    _openWindowUpdateSelectedOrder = new RelayCommand(obj =>
                    {
                        if (obj is OrderViewModel orderViewModel)
                        {
                            new UpdateOrderWindow(orderViewModel).ShowDialog();
                        }
                    }, obj => SelectedOrder != null));
            }
        }

        private RelayCommand _updateOrderCommand;

        public RelayCommand UpdateOrderCommand
        {
            get
            {
                return _updateOrderCommand ?? (
                    _updateOrderCommand = new RelayCommand(async obj =>
                    {
                        if (obj is OrderViewModel orderViewModel)
                        {
                            if (orderViewModel.SelectedOrder != null)
                            {
                                Order newOrders = orderViewModel.SelectedOrder;
                                newOrders.Status = orderViewModel.SelectedStatus;
                                await new OrderController().UpdateAsync(newOrders);
                                GetData();
                            }
                        }
                    }, obj => SelectedOrder != null)
                );
            }
        }


        public static event EventHandler<OrderEventArgs> OrderRemoved;

        public static void OrderRemovedEvent(Order order)
        {
            OrderRemoved?.Invoke(null, new OrderEventArgs(order));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}