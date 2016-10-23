﻿using System.Collections.Generic;
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
            var Test = Dbase.getGroupList();
            adressGroupGrid.ItemsSource = Test;
        }
    }


    public class adressGroup
    {

        public int groupID { get; set; }
        public string groupName { get; set; }

    }

}
