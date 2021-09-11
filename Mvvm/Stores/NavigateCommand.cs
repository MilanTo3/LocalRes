using Mvvm.Command;
using Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvvm.Stores
{
    public class NavigateCommand : CommandBase
    {

        private readonly NavigationStore _navigationStore;

        public NavigateCommand(NavigationStore navigationStore) {

            _navigationStore = navigationStore;

        }

        public override void Execute(object parameter) {
            string[] info = ((string)parameter).Split('|');

            if (info[0] == "generators") {
                _navigationStore.CurrentViewModel = new GeneratorsViewModel(info[1], _navigationStore);
            }
            else if (info[0] == "generator") {
                _navigationStore.CurrentViewModel = new GeneratorViewModel(info[1], _navigationStore);
            }
            else if (info[0] == "start") {
                _navigationStore.CurrentViewModel = new LkResViewModel(_navigationStore);
            }
        }

    }
}
