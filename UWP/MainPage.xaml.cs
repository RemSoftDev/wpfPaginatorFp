﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Paginator;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private CustomTypes.PaginatorState PaginatorCurrentState;

        // Initialisations
        private void DisableElements(CustomTypes.PaginatorState pPaginatorState)
        {
            DisableLeft(pPaginatorState.IsValidLeft);
            DisableLeftMore(pPaginatorState.IsValidLeftMore);
            DisableRight(pPaginatorState.IsValidRight);
            DisableRightMore(pPaginatorState.IsValidRightMore);
        }

        private void DisableLeft(bool pIsEnabled)
        {
            //Name_ButtonLeft.IsEnabled = pIsEnabled;
        }

        private void DisableLeftMore(bool pIsEnabled)
        {
            //Name_ButtonLeftMore.IsEnabled = pIsEnabled;
        }

        private void DisableRight(bool pIsEnabled)
        {
            //Name_ButtonRight.IsEnabled = pIsEnabled;
        }

        private void DisableRightMore(bool pIsEnabled)
        {
            //Name_ButtonRightMore.IsEnabled = pIsEnabled;
        }

        void InitPaginator()
        {
            var DbData = MOCK_InitializeData();
            var currentPage = new CustomTypes.IntMore0Less65535Exclsv(1);
            var itemsPerPageList = MOCK_InitializeItemsPerPage().ToArray();
            var pagesToSkipList = MOCK_InitializeItemsPagesToSkip().ToArray();
            var defauleSelectedIndex = 0;

            //ComboBoxItemsPerPage.ItemsSource = itemsPerPageList;
            //ComboBoxItemsPerPage.SelectedIndex = defauleSelectedIndex;

            //ComboBoxPagesToSkip.ItemsSource = pagesToSkipList;
            //ComboBoxPagesToSkip.SelectedIndex = defauleSelectedIndex;

            PaginatorCurrentState = PaginatorScope.Init(
                currentPage,
                itemsPerPageList[defauleSelectedIndex],
                pagesToSkipList[defauleSelectedIndex],
                ""
                );

            RenderPaginator(PaginatorCurrentState);
        }

        //  MOCK
        private List<int> MOCK_InitializeData()
        {
            var res = Enumerable.Range(0, 100).ToList();
            return res;
        }

        private IEnumerable<CustomTypes.IntMore0Less65535Exclsv> MOCK_InitializeItemsPerPage()
        {
            var res = Enumerable.Range(2, 10).Select(i => (CustomTypes.IntMore0Less65535Exclsv)i);
            return res;
        }

        private IEnumerable<CustomTypes.IntMore0Less65535Exclsv> MOCK_InitializeItemsPagesToSkip()
        {
            var res = Enumerable.Range(2, 5).Select(i => (CustomTypes.IntMore0Less65535Exclsv)i);
            return res;
        }

        // Handlers
        private void Button_Click_Left
            (object sender, RoutedEventArgs e)
        {
            //PaginatorCurrentState = PaginatorCurrentState.GoLeft();
            RenderPaginator(PaginatorCurrentState);
        }

        private void Button_Click_LeftMore
            (object sender, RoutedEventArgs e)
        {
            //PaginatorCurrentState = PaginatorCurrentState.GoLeftMore();
            RenderPaginator(PaginatorCurrentState);
        }

        private void Button_Click_Right
            (object sender, RoutedEventArgs e)
        {
            //PaginatorCurrentState = PaginatorCurrentState.GoRight();
            RenderPaginator(PaginatorCurrentState);
        }

        private void Button_Click_RightMore
            (object sender, RoutedEventArgs e)
        {
            //PaginatorCurrentState = PaginatorCurrentState.GoRightMore();
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
        private void RenderPaginator(CustomTypes.PaginatorState pPaginatorState)
        {
            DisableElements(pPaginatorState);
            //UpdateUI_SetDataToShow(pPaginatorState.PagesToShow());
            UpdateUI_CurrentPageIs(pPaginatorState.CurrentPage);
        }

        private void UpdateUI_CurrentPageIs(CustomTypes.IntMore0Less65535Exclsv pCurrentPage)
        {
            //Name_CurrentPageIs.Text = pCurrentPage.Value.ToString();
        }

        private void UpdateUI_SetDataToShow(IEnumerable<int> data)
        {
            //ListDB.ItemsSource = data;
        }

    }
}