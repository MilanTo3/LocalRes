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
        private List<UnitGroup> groups;

        public GroupsWindow() {
            InitializeComponent();
        }

        public GroupsWindow(string argument) {

            InitializeComponent();
            bindValues();

        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {

            CreateGroupDialog cgd = new CreateGroupDialog();
            cgd.ShowDialog();
            bindValues();

        }

        private void bindValues() {

            ResDbEntities rde = new ResDbEntities();
            groups = rde.UnitGroups.ToList();

            foreach (var group in groups) {
                Binding myBinding = new Binding();
                myBinding.Path = new PropertyPath("UnitName");
                myBinding.Source = group;
                myBinding.Mode = BindingMode.TwoWay;
                myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                Canvas c1 = new Canvas();
                c1.Background = Brushes.Aqua;
                Label groupName = new Label();
                c1.Children.Add(groupName);
                groupName.DataContext = group.UnitName;

                BindingOperations.SetBinding(groupName, Label.ContentProperty, myBinding);

                GroupCanvas.Children.Add(c1);
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {

            using (ResDbEntities rde = new ResDbEntities()) {
                foreach (var group in rde.UnitGroups.ToList()) {
                    group.UnitName = ".";
                    
                }
                rde.SaveChanges();
            }

        }
    }
}
