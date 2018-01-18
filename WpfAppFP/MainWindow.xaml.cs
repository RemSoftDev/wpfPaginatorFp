using Microsoft.FSharp.Collections;
using Paginator;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls; 
using static Paginator.Types.CustomTypes;
using static Paginator.PaginatorScope;

namespace WpfAppFP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitPaginator();
        }

        private PaginatorState PaginatorCurrentState;

        // Initialisations
        private void DisableElements(PaginatorState pPaginatorState)
        {
            DisableLeft(pPaginatorState.IsValidLeft);
            DisableLeftMore(pPaginatorState.IsValidLeftMore);
            DisableRight(pPaginatorState.IsValidRight);
            DisableRightMore(pPaginatorState.IsValidRightMore);
        }

        private void DisableLeft(bool pIsEnabled)
        {
            Name_ButtonLeft.IsEnabled = pIsEnabled;
        }

        private void DisableLeftMore(bool pIsEnabled)
        {
            Name_ButtonLeftMore.IsEnabled = pIsEnabled;
        }

        private void DisableRight(bool pIsEnabled)
        {
            Name_ButtonRight.IsEnabled = pIsEnabled;
        }

        private void DisableRightMore(bool pIsEnabled)
        {
            Name_ButtonRightMore.IsEnabled = pIsEnabled;
        }
        List<int> DbData = null;

        void InitPaginator()
        {
            var dbData = MOCK_InitializeData();
            DbData = dbData;
            var currentPage = new IntMore0Less65535Exclsv(1);
            var itemsPerPageList = MOCK_InitializeItemsPerPage().ToArray();
            var pagesToSkipList = MOCK_InitializeItemsPagesToSkip().ToArray();
            var defauleSelectedIndex = 0;

            ComboBoxItemsPerPage.ItemsSource = itemsPerPageList;
            ComboBoxItemsPerPage.SelectedIndex = defauleSelectedIndex;

            ComboBoxPagesToSkip.ItemsSource = pagesToSkipList;
            ComboBoxPagesToSkip.SelectedIndex = defauleSelectedIndex;
            FSharpList<int> niceSharpList = ListModule.OfSeq(dbData);

            PaginatorCurrentState = Init(
                currentPage,
                itemsPerPageList[defauleSelectedIndex],
                pagesToSkipList[defauleSelectedIndex],
                DbData.Count
                );

            RenderPaginator(PaginatorCurrentState);
        }

        //  MOCK
        private List<int> MOCK_InitializeData()
        {
            var res = Enumerable.Range(0, 100).ToList();
            return res;
        }

        private IEnumerable<IntMore0Less65535Exclsv> MOCK_InitializeItemsPerPage()
        {
            var res = Enumerable.Range(2, 10).Select(i => (IntMore0Less65535Exclsv)i);
            return res;
        }

        private IEnumerable<IntMore0Less65535Exclsv> MOCK_InitializeItemsPagesToSkip()
        {
            var res = Enumerable.Range(2, 5).Select(i => (IntMore0Less65535Exclsv)i);
            return res;
        }

        // Handlers
        private void Button_Click_Left
            (object sender, RoutedEventArgs e)
        {
            //PaginatorCurrentState = PaginatorCurrentState.GoRight();
            PaginatorCurrentState = GoLeft(PaginatorCurrentState);
            RenderPaginator(PaginatorCurrentState);
        }

        private void Button_Click_LeftMore
            (object sender, RoutedEventArgs e)
        {
            // PaginatorCurrentState = PaginatorCurrentState.GoLeftMore();
            PaginatorCurrentState = GoLeftMore(PaginatorCurrentState);
            RenderPaginator(PaginatorCurrentState);
        }

        private void Button_Click_Right
            (object sender, RoutedEventArgs e)
        {
            // PaginatorCurrentState = PaginatorCurrentState.GoRight();
            PaginatorCurrentState = GoRight(PaginatorCurrentState);
            RenderPaginator(PaginatorCurrentState);
        }

        private void Button_Click_RightMore
            (object sender, RoutedEventArgs e)
        {
            //PaginatorCurrentState = PaginatorCurrentState.GoRightMore();
            PaginatorCurrentState = GoRightMore(PaginatorCurrentState);
            RenderPaginator(PaginatorCurrentState);
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
        private void RenderPaginator(PaginatorState pPaginatorState)
        {
            DisableElements(pPaginatorState);
            UpdateUI_SetDataToShow(Func.PaginatorScope.GetDataStartEndIndex1(pPaginatorState.LeftIndexInclsv, pPaginatorState.RightIndexInclsv, DbData));
            UpdateUI_CurrentPageIs(pPaginatorState.CurrentPage);
        }

        private void UpdateUI_CurrentPageIs(IntMore0Less65535Exclsv pCurrentPage)
        {
            Name_CurrentPageIs.Text = pCurrentPage.Value.ToString();
        }

        private void UpdateUI_SetDataToShow(IEnumerable<int> data)
        {
            ListDB.ItemsSource = data;
        }
    }
}
