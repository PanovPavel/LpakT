using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LpakBL.Model;
using LpakViewClient.ModelView;

namespace LpakViewClient
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            /*ChangeCustomerWindow changeCustomerWindow = new ChangeCustomerWindow();
            changeCustomerWindow.DataContext = this.DataContext;
            changeCustomerWindow.Show();*/
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.DataContext is OrderViewModel viewModel)
                {
                    var selectedItems = new ObservableCollection<Customer>();
                    foreach (var selectedItem in listBox.SelectedItems)
                    {
                        if (selectedItem is Customer customer)
                            selectedItems.Add(customer);
                    }
                    viewModel.SelectedCustomers = selectedItems;
                }
            }
        }
    }
}