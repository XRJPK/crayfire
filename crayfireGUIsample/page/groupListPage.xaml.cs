using System.Collections.Generic;
using System.Windows.Controls;
using crayfireGUIsample.lib;


namespace crayfireGUIsample.page
{
    /// <summary>
    /// Interaction logic for groupListPage.xaml
    /// </summary>
    public partial class groupListPage : Page
    {
        public groupListPage()
        {
            InitializeComponent();

            //SELECT g.groupID, g.groupName, (SELECT count(*) FROM crayfire_address WHERE groupID = g.groupID) as addressCount FROM crayfire_address_group AS g  
            var Dbase = new Database();
            var Test = Dbase.GetCrayfireAdress("SELECT g.groupID, g.groupName, (SELECT count(groupID) FROM crayfire_address WHERE groupID = g.groupID) as addressCount FROM crayfire_address_group AS g");
            
            adressGroupGrid.CanUserAddRows = false;
            adressGroupGrid.ItemsSource = Test;
        }
    }
}
