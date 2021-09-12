using Mvvm.Command;
using Mvvm.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Mvvm.ViewModels
{
    public class GeneratorViewModel : ViewModelBase
    {

        public MyICommand backNavigateCommand { get; set; }
        public ICommand updateCommand { get; set; }
        public ICommand navigate;
        private DispatcherTimer timer = new DispatcherTimer();
        private Unit viewGenerator;
        private string controlType;
        private double currentActivePower;
        private double maximumActivePower;
        private double minimumActivePower;
        private double productionPrice;
        private string unitType;
        public string GroupName { get; set; }
        public List<string> ControlTypes { get; set; }

        public GeneratorViewModel(string generatorId, NavigationStore navigationStore) {

            backNavigateCommand = new MyICommand(backCommand);
            updateCommand = new MyICommand(updateMethodCommand);
            navigate = new NavigateCommand(navigationStore);
            SystemControllerDbEntities rde = new SystemControllerDbEntities();
            viewGenerator = rde.Units.ToList().Find(x => x.id == int.Parse(generatorId));
            setup();
            ControlTypes = new List<string>();
            ControlTypes.Add("Local");
            ControlTypes.Add("Remote");
            GroupName = rde.LkRes.ToList().Find(x => x.onLocal == viewGenerator.lkresid).name;
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Start();

        }

        private void setup() {
            controlType = viewGenerator.ControlType;
            currentActivePower = viewGenerator.CurrentActivePower ?? default(double);
            maximumActivePower = viewGenerator.MaximumActivePower ?? default(double);
            minimumActivePower = viewGenerator.MinimumActivePower ?? default(double);
            productionPrice = viewGenerator.ProductionPrice ?? default(double);
            unitType = viewGenerator.UnitType;
        }

        private void UpdateTimer_Tick(object sender, EventArgs args) {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();
            viewGenerator = rde.Units.ToList().Find(x => x.id == viewGenerator.id);

            controlType = viewGenerator.ControlType;
            CurrentActivePower = viewGenerator.CurrentActivePower ?? default(double);
            productionPrice = viewGenerator.ProductionPrice ?? default(double);
            unitType = viewGenerator.UnitType;

        }

        private void updateMethodCommand() {
            viewGenerator.ProductionPrice = productionPrice;
            viewGenerator.ControlType = controlType;
        }
        
        private void backCommand() {
            timer.Stop();
            navigate.Execute("generators" + "|" + viewGenerator.lkresid.ToString());
        }

        public string ControlType {

            get { return controlType; }
            set {
                if (controlType != value) {
                    controlType = value;
                    OnPropertyChanged("ControlType");
                }
            }
        }

        public double CurrentActivePower {

            get { return currentActivePower; }
            set {
                if (currentActivePower != value) {
                    currentActivePower = value;
                    OnPropertyChanged("CurrentActivePower");
                }
            }
        }

        public double MinimumActivePower {

            get { return minimumActivePower; }
            set {
                if (minimumActivePower != value) {
                    minimumActivePower = value;
                    OnPropertyChanged("MinimumActivePower");
                }
            }
        }

        public double MaximumActivePower {

            get { return maximumActivePower; }
            set {
                if (maximumActivePower != value) {
                    maximumActivePower = value;
                    OnPropertyChanged("MaximumActivePower");
                }
            }
        }

        public double ProductionPrice {

            get { return productionPrice; }
            set {
                if (productionPrice != value) {
                    productionPrice = value;
                    OnPropertyChanged("ProductionPrice");
                }
            }
        }

        public string UnitType {

            get { return unitType; }
            set {
                if (unitType != value) {
                    unitType = value;
                    OnPropertyChanged("UnitType");
                }
            }

        }

    }
}
