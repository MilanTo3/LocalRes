using Mvvm.Command;
using Mvvm.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Mvvm.ViewModels
{
    public class GeneratorsViewModel : ViewModelBase 
    {

        private Unit selectedGenerator;
        public ObservableCollection<Unit> Generators { get; set; }
        public MyICommand backNavigateCommand { get; set; }
        public ICommand routeCommand { get; set; }
        public ICommand navigate;
        private DispatcherTimer timer = new DispatcherTimer();
        public string WindowTitle { get; set; }

        public GeneratorsViewModel(string groupId, NavigationStore navigationStore) {

            backNavigateCommand = new MyICommand(backCommand);
            routeCommand = new MyICommand(onRoute);
            Generators = new ObservableCollection<Unit>();
            navigate = new NavigateCommand(navigationStore);
            SystemControllerDbEntities rde = new SystemControllerDbEntities();
            WindowTitle = "LkRes name: " + rde.UnitGroups.ToList().Find(x => x.id == int.Parse(groupId)).UnitName;
            refreshGroups();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();

        }

        private void UpdateTimer_Tick(object sender, EventArgs args) {

            List<Unit> dbgenerators = new SystemControllerDbEntities().Units.ToList().FindAll(x => x.GroupId == id);

            int i;
            for (i = 0; i < Generators.Count; i++) {
                Unit temp = dbgenerators.Find(x => x.id == Generators[i].id);
                if (temp != null && temp != Generators[i]) {
                    Generators[i] = temp;
                }
            }
        }

        private void onRoute() {
            timer.Stop();
            navigate.Execute("generator" + "|" + selectedGenerator.id);
        }

        private void backCommand() {
            timer.Stop();
            navigate.Execute("lkres" + "|");
        }

        private void refreshGroups() {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();

            int id = int.Parse(GroupId);
            foreach (Unit generator in rde.Units.ToList().FindAll(x => x.GroupId == id)) {
                if (Generators.ToList().Exists(x => x.id == generator.id) == false) {
                    Generators.Add(generator);
                }
            }

        }

        public Unit SelectedGenerator {

            get { return selectedGenerator; }
            set {
                if (selectedGenerator != value) {
                    selectedGenerator = value;
                    OnPropertyChanged("SelectedGenerator");
                }
            }
        }
    }
}
