using NavigationMVVM.Commands;
using NavigationMVVM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace NavigationMVVM.ViewModels
{
    public class StartViewModel : ViewModelBase
    {

        public ObservableCollection<string> controllers { get; set; }
        private string chosenSetup;
        private string controllerName;
        public ICommand routeCommand { get; set; }

        public StartViewModel(INavigationService groupsNavigation) {

            loadControllers();
            routeCommand = new NavigateCommand(groupsNavigation);

        }

        private void onRoute() {

        }

        private void loadControllers() {

            

        }

        public string ChosenSetup {
            get { Console.WriteLine($"Got {chosenSetup}"); return chosenSetup; }
            set {
                chosenSetup = value;
                OnPropertyChanged(nameof(ChosenSetup));
            }
        }

        public string ControllerName {
            get { Console.WriteLine($"Got {controllerName}"); return controllerName; }
            set {
                controllerName = value;
                OnPropertyChanged(nameof(ControllerName));
            }
        }

    }
}
