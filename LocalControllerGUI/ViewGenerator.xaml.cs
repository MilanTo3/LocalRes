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
    /// Interaction logic for ViewGenerator.xaml
    /// </summary>
    public partial class ViewGenerator : Window {
        public ViewGenerator() {
            InitializeComponent();
            bindValues();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void bindValues() {

            ResDbEntities resDb = new ResDbEntities();
            var item = resDb.Units.ToList();
            

        }
    }
}
