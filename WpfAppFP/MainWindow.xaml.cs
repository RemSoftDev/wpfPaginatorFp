using Func;
using Func.Types;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppFP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitPaginator();

            UpdateUI();
        }

        private PaginatorState Paginator = new PaginatorState();

        // Initialisations
        private void RenderPaginator(PaginatorState paginatorState)
        {
            DisableLeft();
            DisableLeftMore();
            DisableRight();
            DisableRightMore();
        }

        private void DisableLeft()
        {
            Name_ButtonLeft.IsEnabled = Paginator.IsValidLeft;
        }

        private void DisableLeftMore()
        {
            Name_ButtonLeftMore.IsEnabled = Paginator.IsValidLeftMore;
        }

        private void DisableRight()
        {
            Name_ButtonRight.IsEnabled = Paginator.IsValidRight;
        }

        private void DisableRightMore()
        {
            Name_ButtonRightMore.IsEnabled = Paginator.IsValidRightMore;
        }

        void InitPaginator()
        {
            Paginator.DbData = MOCK_InitializeData();

            var itemsPerPageList = MOCK_InitializeItemsPerPage();
            var pagesToSkipList = MOCK_InitializeItemsPagesToSkip();

            ComboBoxItemsPerPage.ItemsSource = itemsPerPageList;
            ComboBoxItemsPerPage.SelectedIndex = 0;
            Paginator.ItemsPerPage.Value = itemsPerPageList.First().Value;

            ComboBoxPagesToSkip.ItemsSource = pagesToSkipList;
            ComboBoxPagesToSkip.SelectedIndex = 0;
            Paginator.PagesToSkip.Value = pagesToSkipList.First().Value;

            CurrentStatePaginator = PaginatorScope.Init()(Paginator, itemsPerPageList.First().Value, pagesToSkipList.First().Value);
        }

        //  MOCK
        private List<int> MOCK_InitializeData()
        {
            var res = Enumerable.Range(0, 100).ToList();
            return res;
        }

        private IEnumerable<IntGreater0Less65535Exclusive> MOCK_InitializeItemsPerPage()
        {
            var res = Enumerable.Range(2, 10).Select(i => (IntGreater0Less65535Exclusive)i);
            return res;
        }

        private IEnumerable<IntGreater0Less65535Exclusive> MOCK_InitializeItemsPagesToSkip()
        {
            var res = Enumerable.Range(2, 5).Select(i => (IntGreater0Less65535Exclusive)i);
            return res;
        }

        // Handlers
        private void Button_Click_Left
            (object sender, RoutedEventArgs e)
        {

            CurrentStatePaginator = CurrentStatePaginator.GoLeft<PaginatorState>();
                UpdateUI(CurrentStatePaginator);
        }

        private void Button_Click_LeftMore
            (object sender, RoutedEventArgs e)
        {
            if (Paginator.IsValidLeftMore)
            {
                Paginator = Paginator.GoLeftMore<PaginatorState>();
                UpdateUI();
            }
        }

        private void Button_Click_Right
            (object sender, RoutedEventArgs e)
        {
            if (Paginator.IsValidRight)
            {
                Paginator = Paginator.GoRight<PaginatorState>();
                UpdateUI();
            }
        }

        private void Button_Click_RightMore
            (object sender, RoutedEventArgs e)
        {
            if (Paginator.IsValidRightMore)
            {
                Paginator = Paginator.GoRightMore<PaginatorState>();
                UpdateUI();
            }
        }

        private void ComboBoxPagesToSkip_SelectionChanged
            (object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItemsPerPage_SelectionChanged
            (object sender, SelectionChangedEventArgs e)
        {

        }

        // Update UI
        private void UpdateUI()
        {
            DisableElements();
            UpdateUI_CurrentPageIs();
            UpdateUI_SetDataToShow(Paginator.PagesToShow());
        }

        private void UpdateUI_CurrentPageIs()
        {
            Name_CurrentPageIs.Text = Paginator.CurrentPage.Value.ToString();
        }

        private void UpdateUI_SetDataToShow(IEnumerable<int> data)
        {
            ListDB.ItemsSource = data;
        }
    }
}
