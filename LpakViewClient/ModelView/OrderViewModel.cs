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
        private StatusOrder _selectedStatus;
        private ObservableCollection<StatusOrder> _statuses = new ObservableCollection<StatusOrder>();
        private RelayCommand _updateOrderCommand;
        private RelayCommand _removeSelectedOrder;
        private RelayCommand _openWindowUpdateSelectedOrder;

        public OrderViewModel()
        {
            GetData();
            CustomerViewModel.CustomerAdded += AddCustomerEventHandler;
            CustomerViewModel.CustomerDeleted += DeleteCustomerEventHandler;
            CustomerViewModel.CustomerUpdated += UpdateCustomerEventHandler;
            CustomerViewModel.OrderAdded += OrderAddedEventHandler;
            CustomerViewModel.OrderRemoved += OrderRemovedEventHandler;
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

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public ObservableCollection<StatusOrder> Statuses
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged("Statuses");
            }
        }
        public ObservableCollection<Customer> SelectedCustomers
        {
            get => _selectedCustomers;
            set
            {
                _selectedCustomers = value;
                OnPropertyChanged(nameof(SelectedCustomers));
                UpdateFilteredOrders(value);
            }
        }
        public Order SelectedOrder
        {
            get => _selectedOrder;
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


        public RelayCommand RemoveSelectedOrder
        {
            get
            {
                return _removeSelectedOrder ?? (_removeSelectedOrder = new RelayCommand(RemoveSelectedOrderAsync, obj => SelectedOrder != null));
            }
        }

        private async void RemoveSelectedOrderAsync(object obj)
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
        }
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
        public RelayCommand UpdateOrderCommand
        {
            get
            {
                return _updateOrderCommand ?? (_updateOrderCommand = new RelayCommand(UpdateOrderCommandAsync, obj => SelectedOrder != null));
            }
        }

        private async void UpdateOrderCommandAsync(object obj)
        {
            if (obj is OrderViewModel orderViewModel)
            {
                if (orderViewModel.SelectedOrder != null)
                {
                    Order newOrder = orderViewModel.SelectedOrder;
                    newOrder.Status = orderViewModel.SelectedStatus;
                    await new OrderController().UpdateAsync(newOrder);
                    GetData();
                    OrderUpdatedEvent(newOrder);
                }
            }
        }
        
        private void OrderRemovedEventHandler(object sender, OrderEventArgs e)
        {
            var order = Orders.First(o => o.Id == e.Order.Id);
            int removeIndex = Orders.IndexOf(e.Order);
            Orders.RemoveAt(removeIndex);
        }

        private void OrderAddedEventHandler(object sender, OrderEventArgs e)
        {
            Orders.Insert(0, e.Order);
        }

        private void DeleteCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            var customer = Customers.First(c => c.CustomerId == e.Customer.CustomerId);
            var indexRemove = Customers.IndexOf(customer);
            Customers.RemoveAt(indexRemove);
        }

        private void UpdateCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            var customer = Customers.First(c => c.CustomerId == e.Customer.CustomerId);
            var indexOldCustomer = Customers.IndexOf(customer);
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
        
        public event EventHandler<Guid> StatusSelectedChanged;
        public static event EventHandler<OrderEventArgs> OrderUpdated;

        private static void OrderUpdatedEvent(Order order)
        {
            OrderUpdated?.Invoke(null, new OrderEventArgs(order));
        }
        public static event EventHandler<OrderEventArgs> OrderRemoved;

        private static void OrderRemovedEvent(Order order)
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