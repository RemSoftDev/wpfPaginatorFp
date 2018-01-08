using Func;
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

            SetDataToShow();
            DisableElements();
        }

        private (
             IEnumerable<int> itemsToShow,
             int CurrentPage,
             int NumberOfPages,
             bool IsValidLeft,
             bool IsValidLeftMore,
             bool IsValidRight,
             bool IsValidRightMore
             )
             PageRight;

        private void SetDataToShow()
        {
            ListDB.ItemsSource = PageRight.itemsToShow;
        }

        // Initialisations
        private void DisableElements()
        {
            DisableLeft();
            DisableLeftMore();
            DisableRight();
            DisableRightMore();
        }

        private void DisableLeft()
        {
            Name_ButtonLeft.IsEnabled = PageRight.IsValidLeft;
        }

        private void DisableLeftMore()
        {
            Name_ButtonLeftMore.IsEnabled = PageRight.IsValidLeftMore;
        }

        private void DisableRight()
        {
            Name_ButtonRight.IsEnabled = PageRight.IsValidRight;
        }

        private void DisableRightMore()
        {
            Name_ButtonRightMore.IsEnabled = PageRight.IsValidRightMore;
        }

        void InitPaginator()
        {
            var db = MOCK_InitializeData();

            var currentPage = 1;

            var itemsPerPageList = MOCK_InitializeItemsPerPage();
            var pagesToSkipList = MOCK_InitializeItemsPagesToSkip();

            ComboBoxItemsPerPage.ItemsSource = itemsPerPageList;
            ComboBoxItemsPerPage.SelectedIndex = 0;
            var itemsPerPage = itemsPerPageList.First();

            ComboBoxPagesToSkip.ItemsSource = pagesToSkipList;
            ComboBoxPagesToSkip.SelectedIndex = 0;
            var pagesToSkip = pagesToSkipList.First();

            var ddd = PaginatorScope.Init()(currentPage, itemsPerPage, pagesToSkip, db).PageRight<(IEnumerable<int>, int, int, bool, bool, bool, bool)>()();

            PageRight = ddd;



            UpdateUI_CurrentPageIs();
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

        // Validators
        private bool IsValid()
        {

            IsValidItemsPerPage();
            IsValidPagesToSkip();

            return false;
        }



        private bool IsValidItemsPerPage()
        {
            var res = false;

            //if (CurrentPage > 1)
            //{
            //    res = true;
            //}

            return res;
        }

        private bool IsValidPagesToSkip()
        {
            var res = false;

            //if (CurrentPage > 1)
            //{
            //    res = true;
            //}

            return res;
        }

        // Handlers
        private void Button_Click_Left(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_LeftMore(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Right(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_RightMore(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxPagesToSkip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItemsPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Update UI
        private void UpdateUI_CurrentPageIs()
        {
            Name_CurrentPageIs.Text = PageRight.CurrentPage.ToString();
        }
    }
}
