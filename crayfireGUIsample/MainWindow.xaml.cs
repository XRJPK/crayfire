using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using crayfireGUIsample.lib;


namespace crayfireGUIsample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        // Initialize Log4Net 
        //
        // based on tutorial  https://csharp.today/log4net-tutorial-great-library-for-logging/

        private static readonly log4net.ILog log4 =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            log4.Info("Crayfire initalized - User: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            InitializeComponent();
            InitializePageMenu();
            InitializeDockPanel();
            initSesson();
        }
        ~MainWindow() {}
        public void initSesson(){

        }


        private void InitializePageMenu()
        {
            var Dbase = new Database();
            var Test = Dbase.GetCrayfireMenu();
            List<pageMenuItem> pageMenuItemList = new List<pageMenuItem>();

            foreach (var a in Test)
            {
                if (a.parentMenuItem == "")
                {
                    pageMenuItemList.Add(new pageMenuItem() { menuItemID = a.menuItemID, menuItem = a.menuItem, icon = a.icon, menuItemLink = a.menuItemLink });
                }
            }
            pageMenu.ItemsSource = pageMenuItemList;
        }

        private void selectMenuItem(object sender, MouseButtonEventArgs e)
        {
            Button a = (Button)sender;
            InitializePageSubMenu(a.Tag.ToString());
        }



        private void navigatePage(object sender, MouseButtonEventArgs e)
        {
            Label a = (Label)sender;
            if (a.Tag.ToString() == "")
            {
                /*BUILD ERRORHANDLER*/
                MessageBox.Show("no page found. CHECK DATABASE");
            }
            else
            {
                try
                {
                    Uri page = new System.Uri("/page/" + a.Tag.ToString() + ".xaml", UriKind.RelativeOrAbsolute);
                    //TODO check uri
                     workplace.Navigate(page);
                }
                catch(Exception Ex) {
                    /*BUILD ERRORHANDLER*/
                    MessageBox.Show("no page found. CHECK PROGRAM COMPONENT");
                    log4.Error(Ex);
                    throw;
                }
                
            }

        }

        private void InitializePageSubMenu(string menuItem)
        {
            subMenuPanel.Children.Clear();
            var Dbase = new Database();
            var Test = Dbase.GetCrayfireMenu();
            foreach (var a in Test)
            {
                if (a.parentMenuItem == menuItem)
                {

                    Label m = new Label
                    {
                        Style = this.FindResource("subMenuHeader") as Style,
                        Content = a.menuItemLink
                    };
                    subMenuPanel.Children.Add(m);
                    foreach (var b in Test)
                    {
                        if (a.menuItem == b.parentMenuItem)
                        {

                            Label m2 = new Label
                            {
                                Content = b.menuItemLink,
                                Tag = b.menuItemController
                            };
                            m2.MouseLeftButtonUp += new MouseButtonEventHandler(navigatePage);

                            StackPanel sp2 = new StackPanel
                            {
                                Style = this.FindResource("subMenuItem") as Style,
                            };
                            sp2.Children.Add(m2);
                            subMenuPanel.Children.Add(sp2);
                        }
                    }
                }
            }
        }

        private void workplace_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void InitializeDockPanel ()
        {
            string Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            DockPanel_Bottom.Content += " " + Username;
        }
    }

    public class pageMenuItem
    {

        public int menuItemID { get; set; }
        public string menuItem { get; set; }
        public string icon { get; set; }
        public string menuItemLink { get; set; }
    }
}