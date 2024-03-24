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
    /// <summary>
    /// ViewModel для работы с заказами
    /// </summary>
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
        /// <summary>
        /// Создание экземпляра ViewModel для работы с заказами. Инициализирует данные, обработчики событий.
        /// </summary>
        public OrderViewModel()
        {
            GetData();
            CustomerViewModel.CustomerAdded += AddCustomerEventHandler;
            CustomerViewModel.CustomerDeleted += DeleteCustomerEventHandler;
            CustomerViewModel.CustomerUpdated += UpdateCustomerEventHandler;
            CustomerViewModel.OrderAdded += OrderAddedEventHandler;
            CustomerViewModel.OrderRemoved += OrderRemovedEventHandler;
        }
        
        /// <summary>
        /// Инициализирует orders, customers, statuses данными из базы данных.
        /// </summary>
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

        /// <summary>
        /// Коллекция заказов.
        /// </summary>
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }
        
        /// <summary>
        /// Коллекция заказчиков.
        /// </summary>
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }
        
        /// <summary>
        /// Коллекция статусов заказов.
        /// </summary>
        public ObservableCollection<StatusOrder> Statuses
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged("Statuses");
            }
        }
        
        /// <summary>
        /// Выбранный заказчик
        /// </summary>
        public ObservableCollection<Customer> SelectedCustomers
        {
            get => _selectedCustomers;
            set
            {
                _selectedCustomers = value;
                OnPropertyChanged("SelectedCustomers");
                UpdateFilteredOrders(value);
            }
        }
        
        /// <summary>
        /// Объект заказ.
        /// </summary>
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
        
        /// <summary>
        /// Выбранный статус заказа.
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
        /// Событие выбора определённых клиентов.
        /// Изменяет отображаймые заказы в зависимости от клиентов которые выбраны
        /// </summary>
        /// <param name="selectedCustomers">Список выбранных клиентов чьи заказы необходимо отобразить</param>
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

        /// <summary>
        /// Удалить выбранный заказ
        /// </summary>
        public RelayCommand RemoveSelectedOrder
        {
            get
            {
                return _removeSelectedOrder ?? (_removeSelectedOrder = new RelayCommand(RemoveSelectedOrderAsync, obj => SelectedOrder != null));
            }
        }
        
        /// <summary>
        /// Удалить выбранный заказ
        /// </summary>
        /// <param name="obj">Объект Order который необходимо удалить</param>
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
        
        /// <summary>
        /// Открыть окно изменения выбранного заказа
        /// </summary>
        public RelayCommand OpenWindowUpdateSelectorOrder
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
        
        /// <summary>
        /// Измений  заказ
        /// </summary>
        public RelayCommand UpdateOrderCommand
        {
            get
            {
                return _updateOrderCommand ?? (_updateOrderCommand = new RelayCommand(UpdateOrderCommandAsync, obj => SelectedOrder != null));
            }
        }
        
        /// <summary>
        /// Изменение заказа
        /// </summary>
        /// <param name="obj">объект Order который изменён в view</param>
        private async void UpdateOrderCommandAsync(object obj)
        {
            if (obj is OrderViewModel orderViewModel)
            {
                if (orderViewModel.SelectedOrder != null)
                {
                    Order oldOrder = Orders.First(o => o.Id == orderViewModel.SelectedOrder.Id);
                    Order newOrder = orderViewModel.SelectedOrder;
                    newOrder.Status = orderViewModel.SelectedStatus;
                    
                    oldOrder.Status = orderViewModel.SelectedStatus;
                    oldOrder.DateTimeCreatedOrder = newOrder.DateTimeCreatedOrder;
                    oldOrder.DescriptionOfWork = newOrder.DescriptionOfWork;
                    oldOrder.NameOfWork = newOrder.NameOfWork;
                    await new OrderController().UpdateAsync(newOrder);
                    //GetData();
                    OrderUpdatedEvent(newOrder);
                }
            }
        }

        /// <summary>
        /// Обработка события удаления заказа
        /// </summary>
        /// <param name="sender">null</param>
        /// <param name="e">объект который был удалён</param>
        private void OrderRemovedEventHandler(object sender, OrderEventArgs e)
        {
            Customer customer = Customers.FirstOrDefault(cus => cus.CustomerId == e.Order.CustomerId);
            if (customer != null)
            {
                var findIndex = customer.Orders.FindIndex((ord => ord.Id == e.Order.Id));
                customer.Orders.RemoveAt(findIndex);
            }
            var order = Orders.First(o => o.Id == e.Order.Id);
            int removeIndex = Orders.IndexOf(order);
            Orders.RemoveAt(removeIndex);
        }

        /// <summary>
        /// Обработка события добавления заказа
        /// </summary>
        /// <param name="sender">null</param>
        /// <param name="e">объект который был удалён</param>
        private void OrderAddedEventHandler(object sender, OrderEventArgs e)
        {
            Customer customer = Customers.FirstOrDefault(c => c.CustomerId == e.Order.CustomerId);

            if (customer != null && !customer.Orders.Exists(o => o.Id == e.Order.Id))
            {
                customer.Orders.Add(e.Order);
            }
            if (SelectedCustomers == null || SelectedCustomers.Count == 0)
            {
                Orders.Insert(0, e.Order);
            }
            else if (SelectedCustomers.Any(c => c.CustomerId == e.Order.CustomerId))
            {
                Orders.Insert(0, e.Order);
            }            
        }
        
        /// <summary>
        /// Событие удаления заказчика
        /// </summary>
        /// <param name="sender">null</param>
        /// <param name="e">объект который был удалён</param>
        private void DeleteCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            var customer = Customers.First(c => c.CustomerId == e.Customer.CustomerId);
            var indexRemove = Customers.IndexOf(customer);
            Customers.RemoveAt(indexRemove);
        }
        
        /// <summary>
        /// Обработка события изменения клиета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Изменённый объект</param>
        private void UpdateCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            var customer = Customers.First(c => c.CustomerId == e.Customer.CustomerId);
            customer.Name = e.Customer.Name;
            customer.Orders = e.Customer.Orders;
            customer.TaxNumber = e.Customer.TaxNumber;
            customer.FieldOfBusiness = e.Customer.FieldOfBusiness;
            SelectedCustomers.Add(customer);
        }
        
        /// <summary>
        /// Обработка события добавления клиета
        /// </summary>
        /// <param name="sender">null</param>
        /// <param name="e">Добавленный объект</param>
        private void AddCustomerEventHandler(object sender, CustomerEventArgs e)
        {
            if (e != null && e.Customer != null)
            {
                Customers.Add(e.Customer);
            }
        }
        
        /// <summary>
        /// Событие, возникающее при выбора статуса заказа
        /// </summary>
        public event EventHandler<Guid> StatusSelectedChanged;
        
        /// <summary>
        /// Событие, возникающее при изменение заказа
        /// </summary>
        public static event EventHandler<OrderEventArgs> OrderUpdated;
        
        /// <summary>
        /// Событие, возникающее при удалении заказа
        /// </summary>
        public static event EventHandler<OrderEventArgs> OrderRemoved;
        
        /// <summary>
        /// Инициирует событие изменеия заказа
        /// </summary>
        /// <param name="order">Изменённый заказ</param>
        private static void OrderUpdatedEvent(Order order)
        {
            OrderUpdated?.Invoke(null, new OrderEventArgs(order));
        }
        
        /// <summary>
        /// Инициирует событие удаления заказа
        /// </summary>
        /// <param name="order">удалённый заказ</param>
        private static void OrderRemovedEvent(Order order)
        {
            OrderRemoved?.Invoke(null, new OrderEventArgs(order));
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