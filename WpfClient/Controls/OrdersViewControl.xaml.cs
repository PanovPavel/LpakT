using System;
using System.Collections.Generic;
using System.Windows.Controls;
using LpakBL.Model;

namespace WpfClient.Controls
{
    public partial class OrdersViewControl : UserControl
    {
        private List<Order> _orders = new List<Order>();

        public List<Order> Orders
        {
            get => _orders;
            set
            {
                //_orders = value ?? throw new ArgumentNullException("orders cannot be null");
                //OrdersListView.ItemsSource = _orders;
            }
        }

        public OrdersViewControl()
        {
            InitializeComponent();
            //OrdersListView.ItemsSource = Orders;
        }
    }
}