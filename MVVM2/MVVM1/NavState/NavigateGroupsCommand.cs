using MVVM1.Commands;
using MVVM1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM1.NavState
{
    public class NavigateGroupsCommand : CommandBase
    {

        private readonly NavigationStore _navigationStore;
        
        public NavigateGroupsCommand(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter) {
            _navigationStore.CurrentViewModel = new StudentViewModel();            
        }
    }
}
