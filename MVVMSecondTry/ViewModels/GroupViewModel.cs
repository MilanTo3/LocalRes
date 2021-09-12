using MVVMSecondTry.Stores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVMSecondTry.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {

        private string groupName;
        private UnitGroup selectedGroup;
        public ObservableCollection<UnitGroup> Groups { get; set; }
        public MyICommand addCommand { get; set; }
        private DispatcherTimer timer = new DispatcherTimer();
        private int LKRES;
        public ICommand routeCommand { get; set; }
        public ICommand navigate;
        public MyICommand backNavigateCommand { get; set; }
        public string WindowTitle { get; set; }

        public GroupViewModel(NavigationStore navigationStore) {

            LKRES = ((App)Application.Current).LkRes;
            backNavigateCommand = new MyICommand(backCommand);
            addCommand = new MyICommand(addUnit);
            Groups = new ObservableCollection<UnitGroup>();
            navigate = new NavigateCommand(navigationStore);
            routeCommand = new MyICommand(onRoute);
            ResDbEntities rde = new ResDbEntities();
            WindowTitle = "Local Controller name: " + rde.LkRes.ToList().Find(x => x.id == LKRES).name;
            rde.LkRes.ToList().Find(x => x.id == LKRES).active = true;
            rde.SaveChanges();
            refreshGroups();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Start();

        }

        private void onRoute() {
            timer.Stop();
            navigate.Execute("generators" + "|" + selectedGroup.id);
        }

        private void backCommand() {
            timer.Stop();
            using (ResDbEntities rde = new ResDbEntities()) {
                rde.LkRes.ToList().Find(x => x.id == ((App)Application.Current).LkRes).active = false;
                rde.SaveChanges();
            }
            sendLkResOffSignal();
            navigate.Execute("start|");
        }

        private void sendLkResOffSignal() {
            using (ResDbEntities rde = new ResDbEntities()) {
                rde.LkRes.ToList().Find(x => x.id == ((App)Application.Current).LkRes).active = false;
                rde.SaveChanges();

                MessageQueue billingQ = new MessageQueue();
                billingQ.Path = @".\private$\nekiQueue";

                Message m = new Message();
                m.Formatter = new XmlMessageFormatter((new Type[] { typeof(String) }));
                m.Body = JsonConvert.SerializeObject(rde.LkRes.ToList().Find(x => x.id == ((App)Application.Current).LkRes));
                billingQ.Send(m);
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs args) {
            List<UnitGroup> dbgroups = new ResDbEntities().UnitGroups.ToList().FindAll(x => x.lkResId == LKRES);

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

        private void refreshGroups() {

            ResDbEntities rde = new ResDbEntities();

            foreach (UnitGroup group in rde.UnitGroups.ToList().FindAll(x => x.lkResId == LKRES)) {
                if (Groups.ToList().Exists(x => x.id == group.id) == false) {
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
