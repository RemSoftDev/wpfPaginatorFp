﻿using System;
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
        }

        private int CurrentPage;
        private int ItemsPerPage;
        private int PagesToSkip;

        private int TotalNumberOfItemsInDB;

        private void SetDataToShow()
        {
            ListDB.ItemsSource = GetDataToShow();
        }

        private IEnumerable<int> GetDataToShow()
        {
            var DB = MOCK_InitializeData();
            var res = DB.Take(ItemsPerPage);

            TotalNumberOfItemsInDB = DB.Count();

            return res;
        }

        // Initialisations
        void InitPaginator()
        {
            CurrentPage = 1;

            var itemsPerPage = MOCK_InitializeItemsPerPage();
            var pagesToSkip = MOCK_InitializeItemsPagesToSkip();

            ComboBoxItemsPerPage.ItemsSource = itemsPerPage;
            ComboBoxItemsPerPage.SelectedIndex = 0;
            ItemsPerPage = itemsPerPage.First();

            ComboBoxPagesToSkip.ItemsSource = pagesToSkip;
            ComboBoxPagesToSkip.SelectedIndex = 0;
            PagesToSkip = pagesToSkip.First();          

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
            IsValidLeft();
            IsValidLeftMore();
            IsValidRight();
            IsValidRightMore();
            IsValidItemsPerPage();
            IsValidPagesToSkip();

            return false;
        }

        private bool IsValidLeft()
        {
            return false;
        }

        private bool IsValidLeftMore()
        {
            return false;
        }

        private bool IsValidRight()
        {
            return false;
        }

        private bool IsValidRightMore()
        {
            return false;
        }

        private bool IsValidItemsPerPage()
        {
            return false;
        }

        private bool IsValidPagesToSkip()
        {
            return false;
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
            Name_CurrentPageIs.Text = CurrentPage.ToString();
        }
    }
}
