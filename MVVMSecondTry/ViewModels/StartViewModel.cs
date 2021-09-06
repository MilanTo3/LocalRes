using MVVMSecondTry.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMSecondTry.ViewModels
{
    public class StartViewModel : ViewModelBase
    {

        public ObservableCollection<string> controllers { get; set; }
        private string chosenSetup;
        private string controllerName;
        public ICommand routeCommand { get; set; }
        public ICommand navigate;

        public StartViewModel(NavigationStore navigationStore) {

            loadControllers();
            navigate = new NavigateCommand(navigationStore);
            routeCommand = new MyICommand(onRoute);

        }

        private void loadControllers() {

            controllers = new ObservableCollection<string>();
            controllers.Add("New Controller");

            ResDbEntities rde = new ResDbEntities();
            foreach (LkRe controller in rde.LkRes.ToList()) {
                controllers.Add(controller.name);
            }

        }

        private void onRoute() {
            navigate.Execute("group" + "|" + chosenSetup + "|" + controllerName);
        }

        public string ChosenSetup {
            get { Console.WriteLine($"Got {chosenSetup}"); return chosenSetup; }
            set {
                if (chosenSetup != value) {
                    chosenSetup = value;
                    OnPropertyChanged("ChosenSetup");
                    Console.WriteLine($"Changed setup. Bind successfull! {chosenSetup}");
                }
            }
        }

        public string ControllerName {
            get { Console.WriteLine($"Got {controllerName}"); return controllerName; }
            set {
                if (controllerName != value) {
                    controllerName = value;
                    OnPropertyChanged("ControllerName");
                    Console.WriteLine($"Changed controller name. Bind successfull! {controllerName}");
                }
            }
        }

    }
}
