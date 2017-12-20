using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppFP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitPaginator();

            var DB = MOCK_InitializeData();
            ListDB.ItemsSource = DB.Take(10);
        }

        void InitPaginator()
        {           
            var IiemsPerPage = MOCK_InitializeItemsPerPage();
            var pagesToSkip = MOCK_InitializeItemsPerPage(); 

            ComboBoxItemsPerPage.ItemsSource = IiemsPerPage;
            ComboBoxItemsPerPage.SelectedIndex = 0;

            ComboBoxPagesToSkip.ItemsSource = pagesToSkip;
            ComboBoxPagesToSkip.SelectedIndex = 0;
        }

        private List<int> MOCK_InitializeData()
        {
            var res = Enumerable.Range(0, 100).ToList();
            return res;
        }

        private List<int> MOCK_InitializeItemsPerPage()
        {
            var res = Enumerable.Range(2, 10).ToList();
            return res;
        }

        private List<int> MOCK_InitializeItemsPagesToSkip()
        {
            var res = Enumerable.Range(2, 5).ToList();
            return res;
        }


    }
}
