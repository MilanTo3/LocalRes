using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM1.NavState
{
    public class NavigationStore : BindableBase
    {

        private BindableBase currentViewModel;

        public BindableBase CurrentViewModel {
            get { return currentViewModel; }
            set {
                SetProperty(ref currentViewModel, value);
            }
        }

    }
}
