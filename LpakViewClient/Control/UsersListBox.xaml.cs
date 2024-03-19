using System.Collections.ObjectModel;
using System.Windows.Controls;
using LpakBL.Model;
using LpakViewClient.ModelView;

namespace LpakViewClient.Control
{
    public partial class UsersListBox : UserControl
    {
        public UsersListBox()
        {
            InitializeComponent();
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