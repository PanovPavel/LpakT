using System;
using System.Windows;
using LpakBL.Controller;
using LpakBL.Model;
using LpakViewClient.ModelView;

namespace LpakViewClient.Windows
{
    public partial class AddNewOrderForCustomerWindow : Window
    {
        public AddNewOrderForCustomerWindow()
        {
            InitializeComponent();
        }
        public AddNewOrderForCustomerWindow(CustomerViewModel customer)
        {
            InitializeComponent();
            DataContext = customer;
        }

        private async void AddNewOrder_OnClick(object sender, RoutedEventArgs e)
        {
            StatusOrder statusOrder = Status_Combobox.SelectionBoxItem is StatusOrder stOrder?stOrder:new StatusOrder("Создан");
            Guid customerId = Guid.Parse(CustomerIdSelectedTextBox.Text);
            string nameOfWork = NameWorkTextBox.Text;
            string descriptionOfWork = DescriptionTextBox.Text;
            DateTime dateTimeCreatedOrder = datePicker.SelectedDate ?? DateTime.Now;
            try
            {
                var order = new Order(statusOrder, customerId, dateTimeCreatedOrder, nameOfWork, descriptionOfWork);
                await new OrderController().AddAsync(order);
                this.Close();
                AddOrderForUser?.Invoke(this, order);
            }
            catch (Exception ex) when (HandlerException.IsHandledException(ex))
            {
                new ErrorWindow(ex.Message).ShowDialog();
            }
            
        }
        
        public static event EventHandler<Order> AddOrderForUser;


    }
}