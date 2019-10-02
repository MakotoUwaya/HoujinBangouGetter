using System;
using System.Windows;
using System.Windows.Controls;

namespace HoujinBangouGetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void OnClickSearchButton(object sender, RoutedEventArgs e)
        {
            var searchButton = sender as Button;
            searchButton.IsEnabled = false;

            try
            {
                var result = await HojinNumber.Search(this.SearchHojinNameTextBox.Text.Split('\n'));
                this.ResultDataGrid.ItemsSource = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.ResultDataGrid.ItemsSource = null;
            }
            finally
            {
                searchButton.IsEnabled = true;
            }
        }
    }
}
