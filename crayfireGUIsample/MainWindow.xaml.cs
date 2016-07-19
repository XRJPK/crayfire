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
using crayfireGUIsample;

namespace crayfireGUIsample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool subMenuPanelShow =false;
        private string activeMenuItem = "";


        public MainWindow()
        {
            InitializeComponent();
        }

        private void selectMenuItem(object sender, MouseButtonEventArgs e)
        {
            Button a = (Button)sender;//#FF1F2B36
            if (subMenuPanelShow == true && a.Name == activeMenuItem)
            {
                subMenuPanel.Visibility = Visibility.Collapsed;
                subMenuPanelShow = false;
                activeMenuItem = a.Name;
            }
            else if (subMenuPanelShow == false && a.Name == activeMenuItem)
            {
                subMenuPanel.Visibility = Visibility.Visible;
                subMenuPanelShow = true;
                activeMenuItem = a.Name;

            }
            else if (subMenuPanelShow == false && a.Name != activeMenuItem)
            {
                subMenuPanel.Visibility = Visibility.Visible;
                subMenuPanelShow = true;
                activeMenuItem = a.Name;

                //submenu Laden
            }
            else if (subMenuPanelShow == true && a.Name != activeMenuItem)
            {
                activeMenuItem = a.Name;

                //submenu tauschen
            }




        }
    }
}
