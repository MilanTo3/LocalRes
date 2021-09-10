using MVVMSecondTry.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVMSecondTry.ViewModels
{
    public class GeneratorsViewModel : ViewModelBase
    {

        private string productionPrice;
        private Unit selectedGenerator;
        private string selectedType;
        private double minimumPowerProduction;
        private double maximumPowerProduction;
        public ObservableCollection<Unit> Generators { get; set; }
        public List<string> Types { get; set; }
        public MyICommand addCommand { get; set; }
        public MyICommand backNavigateCommand { get; set; }
        public ICommand routeCommand { get; set; }
        public ICommand navigate;
        private DispatcherTimer timer = new DispatcherTimer();
        private string GroupId;
        public string WindowTitle { get; set; }

        public GeneratorsViewModel(string groupId, NavigationStore navigationStore) {

            addCommand = new MyICommand(addUnit);
            backNavigateCommand = new MyICommand(backCommand);
            routeCommand = new MyICommand(onRoute);
            Generators = new ObservableCollection<Unit>();
            Types = new List<string>();
            Types.Add("Solar");
            Types.Add("Wind");
            Types.Add("MicroHydro");
            navigate = new NavigateCommand(navigationStore);
            GroupId = groupId;
            ResDbEntities rde = new ResDbEntities();
            WindowTitle = "Group name: " + rde.UnitGroups.ToList().Find(x => x.id == int.Parse(groupId)).UnitName;
            refreshGroups();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();

        }

        private void UpdateTimer_Tick(object sender, EventArgs args) {
            int id = int.Parse(GroupId);
            List<Unit> dbgenerators = new ResDbEntities().Units.ToList().FindAll(x => x.GroupId == id);

            int i;
            for (i = 0; i < Generators.Count; i++) {
                Unit temp = dbgenerators.Find(x => x.id == Generators[i].id);
                if (temp != null && temp != Generators[i]) {
                    Generators[i] = temp;
                }
            }
        }

        private void addUnit() {

            Unit generator = new Unit();
            generator.CurrentActivePower = 0;
            generator.MaximumActivePower = maximumPowerProduction;
            generator.MinimumActivePower = minimumPowerProduction;
            generator.ProductionPrice = float.Parse(productionPrice);
            generator.ControlType = "Local";
            generator.UnitType = selectedType;
            generator.GroupId = int.Parse(GroupId);

            using (ResDbEntities rde = new ResDbEntities()) {
                rde.Units.Add(generator);
                rde.SaveChanges();
                Generators.Add(generator);
            }

        }

        private void onRoute() {
            timer.Stop();
            navigate.Execute("generator" + "|" + selectedGenerator.id);
        }

        private void backCommand() {
            timer.Stop();
            navigate.Execute("group" + "|" + ((App)Application.Current).LkRes.ToString());
        }

        private void refreshGroups() {

            ResDbEntities rde = new ResDbEntities();

            int id = int.Parse(GroupId);
            foreach (Unit generator in rde.Units.ToList().FindAll(x => x.GroupId == id)) {
                if (Generators.ToList().Exists(x => x.id == generator.id) == false) {
                    Generators.Add(generator);
                }
            }

        }

        public string ProductionPrice {

            get { return productionPrice; }
            set {
                if (productionPrice != value) {
                    productionPrice = value;
                    OnPropertyChanged("ProductionPrice");
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

        public string SelectedType {

            get { return selectedType; }
            set {
                selectedType = value;
                OnPropertyChanged("SelectedType");
            }
        }

        public double MaximumPowerProduction {

            get { return maximumPowerProduction; }
            set {
                maximumPowerProduction = value;
                OnPropertyChanged("MaximumPowerProduction");
            }
        }

        public double MinimumPowerProduction {

            get { return minimumPowerProduction; }
            set {
                minimumPowerProduction = value;
                OnPropertyChanged("MinimumPowerProduction");
            }
        }

    }
}
