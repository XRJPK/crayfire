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
using crayfireGUIsample;
using crayfireGUIsample.lib;
using WpfTools;
using FontAwesome.WPF;


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
            //InitializePageSubMenu();
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
            //MessageBox.Show(a.Tag.ToString());
            InitializePageSubMenu(a.Tag.ToString());
        }
        /// <summary>
        /// Zeichnet eine Glyphe aus der FontAwesome Bibliothek mit entsprechenden Paramtern
        /// </summary>
        /// <param name="text">Bezeichnung der Glyphe</param>
        /// <param name="fontFamily">Stellt eine Familie von verwandten Schriftarten dar.</param>
        /// <param name="fontStyle">Definiert eine Struktur, die das Format einer Schriftart als normal, kursiv oder schräg darstellt.</param>
        /// <param name="fontWeight">Verweist auf die Dichte einer Schriftart, d. h. darauf, wie fein oder breit die Striche sind</param>
        /// <param name="fontStretch">Beschreibt den Grad, um den eine Schriftart in Bezug auf das normale Verhältnis gestreckt wurde</param>
        /// <param name="foreBrush">Definiert Objekte, die zum Zeichnen grafischer Objekte verwendet werden.Von System.Windows.Media.Brush abgeleitete Klassen beschreiben, wie der Bereich gezeichnet wird.</param>
        /// <returns></returns>
        public ImageSource CreateGlyph(string text,
                FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight,
                FontStretch fontStretch, Brush foreBrush)
        {
            if (fontFamily != null && !String.IsNullOrEmpty(text))
            {
                Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
                GlyphTypeface glyphTypeface;
                if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                    throw new InvalidOperationException("No glyphtypeface found");

                ushort[] glyphIndexes = new ushort[text.Length];
                double[] advanceWidths = new double[text.Length];
                for (int n = 0; n < text.Length; n++)
                {
                    ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
                    glyphIndexes[n] = glyphIndex;
                    double width = glyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
                    advanceWidths[n] = width;
                }

                GlyphRun gr = new GlyphRun(glyphTypeface, 0, false, 1.0, glyphIndexes,
                                            new Point(0, 0), advanceWidths,
                                            null, null, null, null, null, null);
                GlyphRunDrawing glyphRunDrawing = new GlyphRunDrawing(foreBrush, gr);
                return new DrawingImage(glyphRunDrawing);

            }
            return null;
        }

        private void navigatePage(object sender, MouseButtonEventArgs e)
        {
            Label a = (Label)sender;
            if (a.Tag.ToString() == "")
            {
                MessageBox.Show("no page found.");
            }
            else
            {
                workplace.Navigate(new System.Uri("/page/" + a.Tag.ToString() + ".xaml", UriKind.RelativeOrAbsolute));
                //workplace.Navigate("/page/" + a.Tag.ToString() + ".xaml");
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
                                //HorizontalAlignment = HorizontalAlignment.Left,
                                //Background = Brushes.Transparent,
                                //BorderBrush = Brushes.Transparent,
                                //Foreground = Brushes.White,
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