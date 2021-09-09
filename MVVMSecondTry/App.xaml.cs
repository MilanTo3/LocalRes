using MVVMSecondTry.Stores;
using MVVMSecondTry.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMSecondTry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public int LkRes { get; set; }

        protected override void OnStartup(StartupEventArgs e) {

            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new StartViewModel(navigationStore);

            MainWindow = new MainWindow() {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

    }
}
