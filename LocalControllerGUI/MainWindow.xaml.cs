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

namespace LocalControllerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow() {
            InitializeComponent();
            ControllerNameBox.Visibility = Visibility.Hidden;
            ControllerLabel.Visibility = Visibility.Hidden;
            List<string> controllers = new List<string>();
            controllers.Add("New Controller");
            ControllersList.ItemsSource = controllers;

        }

        private void ControllersList_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            string opt = ((ComboBox)sender).SelectedItem as string;
            if (opt == "New Controller") {
                ControllerNameBox.Visibility = Visibility.Visible;
                ControllerLabel.Visibility = Visibility.Visible;
            }
            else {
                ControllerNameBox.Visibility = Visibility.Hidden;
                ControllerLabel.Visibility = Visibility.Hidden;
            }

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e) {

            if ((ControllersList.SelectedItem as string) != null) {
                GroupsWindow groupswindow = new GroupsWindow((string)ControllersList.SelectedItem);
                groupswindow.Show();

                this.Close();
            }



        }
    }
}
