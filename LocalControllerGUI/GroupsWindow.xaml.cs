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
using System.Windows.Shapes;

namespace LocalControllerGUI
{
    /// <summary>
    /// Interaction logic for GroupsWindow.xaml
    /// </summary>
    public partial class GroupsWindow : Window
    {
        public GroupsWindow() {
            InitializeComponent();
        }

        public GroupsWindow(string argument) {

            InitializeComponent();
            Canvas c1 = new Canvas();
            c1.Width = 120;
            c1.Height = 120;

            Label label = new Label();
            label.Content = "Mio Naganohara";
            Canvas.SetLeft(label, 20);

            Button button1 = new Button();
            button1.Content = "Mio Naganohara";
            Canvas.SetLeft(button1, 20);
            Canvas.SetTop(button1, 100);
            c1.Background = Brushes.Aqua;

            c1.Children.Add(label);
            c1.Children.Add(button1);
            GroupCanvas.Children.Add(c1);

            Canvas c2 = new Canvas();
            c2.Width = 120;
            c2.Height = 120;
            c2.Background = Brushes.Aqua;
            GroupCanvas.Children.Add(c2);
            Canvas.SetTop(c2, 140);

            Canvas c3 = new Canvas();
            c3.Width = 120;
            c3.Height = 120;
            c3.Background = Brushes.Aqua;
            GroupCanvas.Children.Add(c3);
            Canvas.SetTop(c3, 280);

        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {

            CreateGroupDialog cgd = new CreateGroupDialog();
            cgd.ShowDialog();

        }
    }
}
