using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVMSecondTry.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {

        private string groupName;
        private UnitGroup selectedGroup;
        public ObservableCollection<UnitGroup> Groups { get; set; }
        public MyICommand addCommand { get; set; }
        public MyICommand testCommand { get; set; }
        private DispatcherTimer timer = new DispatcherTimer();
        private int LKRES;


        public GroupViewModel(string lkResId) {

            LKRES = int.Parse(lkResId);
            addCommand = new MyICommand(addUnit);
            testCommand = new MyICommand(changeValues);
            Groups = new ObservableCollection<UnitGroup>();
            refreshGroups();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();

        }

        private void UpdateTimer_Tick(object sender, EventArgs args) {
            List<UnitGroup> dbgroups = new ResDbEntities().UnitGroups.ToList();

            int i;
            for (i = 0; i < Groups.Count; i++) {
                UnitGroup temp = dbgroups.Find(x => x.id == Groups[i].id);
                if (temp != null && temp != Groups[i]) {
                    Groups[i] = temp;
                }
            }
        }

        private void addUnit() {

            UnitGroup group = new UnitGroup();
            group.UnitName = groupName;
            group.MaxProduction = 0;
            group.UnitNum = 0;
            group.CurrentProduction = 0;
            group.lkResId = LKRES;
            using (ResDbEntities rde = new ResDbEntities()) {
                rde.UnitGroups.Add(group);
                rde.SaveChanges();
                Groups.Add(group);
            }

        }

        private void changeValues() {

            ResDbEntities rde = new ResDbEntities();

            foreach (UnitGroup group in rde.UnitGroups.ToList()) {
                group.MaxProduction = group.MaxProduction++;
            }

        }

        private void refreshGroups() {

            ResDbEntities rde = new ResDbEntities();

            foreach (UnitGroup group in rde.UnitGroups.ToList()) {
                if (Groups.ToList().Exists(x => x.id == group.id) == false) {
                    group.MaxProduction = 0;
                    Groups.Add(group);
                }
            }

        }

        public string GroupName {

            get { return groupName; }
            set {
                if (groupName != value) {
                    groupName = value;
                    OnPropertyChanged("GroupName");
                    Console.WriteLine($"Changed group name. Bind successfull! {groupName}");
                }
            }
        }

        public UnitGroup SelectedGroup {

            get { return selectedGroup; }
            set {
                if (selectedGroup != value) {
                    selectedGroup = value;
                    OnPropertyChanged("SelectedGroup");
                    Console.WriteLine($"Changed selection group. Bind successfull! {selectedGroup}");
                }
            }
        }

    }
}
