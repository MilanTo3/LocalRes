using Mvvm.Command;
using Mvvm.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Mvvm.ViewModels
{
    public class LkResViewModel : ViewModelBase
    {

        private LkRe selectedLkRes;
        public ObservableCollection<LkRe> ActiveLkResList { get; set; }
        public ICommand routeCommand { get; set; }
        public ICommand navigate;
        private DispatcherTimer timer = new DispatcherTimer();

        public LkResViewModel(NavigationStore navigationStore) {

            routeCommand = new MyICommand(onRoute);
            ActiveLkResList = new ObservableCollection<LkRe>();
            navigate = new NavigateCommand(navigationStore);
            SystemControllerDbEntities rde = new SystemControllerDbEntities();
            refreshGroups();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Start();

        }

        private void UpdateTimer_Tick(object sender, EventArgs args) {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();
            refreshGroups();
            List<LkRe> dbgroups = new SystemControllerDbEntities().LkRes.ToList();

            int i;
            for (i = 0; i < ActiveLkResList.Count; i++) {
                LkRe temp = dbgroups.Find(x => x.onLocal == ActiveLkResList[i].onLocal);
                if (temp != null && temp != ActiveLkResList[i]) {
                    ActiveLkResList[i] = temp;
                }
            }

        }

        private void onRoute() {
            timer.Stop();
            navigate.Execute("generators" + "|" + selectedLkRes.onLocal);
        }

        private void refreshGroups() {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();

            ActiveLkResList.Clear();
            foreach (LkRe lkres in rde.LkRes.ToList()) {
                ActiveLkResList.Add(lkres);
            }

        }

        public LkRe SelectedLkRes {

            get { return selectedLkRes; }
            set {
                if (selectedLkRes != value) {
                    selectedLkRes = value;
                    OnPropertyChanged("SelectedLkRes");
                }
            }
        }

    }
}
