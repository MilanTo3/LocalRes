using MVVMSecondTry.Command;
using MVVMSecondTry.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSecondTry.Stores
{
    public class NavigateCommand : CommandBase
    {

        private readonly NavigationStore _navigationStore;

        public NavigateCommand(NavigationStore navigationStore) {

            _navigationStore = navigationStore;

        }

        public override void Execute(object parameter) {
            string[] info = ((string)parameter).Split('|');

            if (info[0] == "group") {
                _navigationStore.CurrentViewModel = new GroupViewModel("");
            }
        }
    }
}
