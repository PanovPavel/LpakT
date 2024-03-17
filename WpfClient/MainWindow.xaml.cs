using System;
using System.Collections.Generic;
using System.Windows;
using WpfClient.Controls;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow MainWindowInstance { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            WindowManager.MainWindowInstance.Enqueue(this);
        }

        private void OpeNewWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            var changeCustomerWindow = new ChangeCustomerWindow();
            changeCustomerWindow.ShowDialog();
        }
        
        private void CustomerSelectedHandler(object sender, EventArgs e)
        {
            if (sender is CustomersListViewControl customersListViewControl)
            {
                /*commentForClient.Text = customersListViewControl.Customer.Comment;
                OrdersViewControl.Orders = customersListViewControl.Customer.Orders;*/
            }
        }
    }
}