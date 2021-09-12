using MVVMSecondTry.Command;
using MVVMSecondTry.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                sendLkResOnSignal();
                _navigationStore.CurrentViewModel = new GroupViewModel(_navigationStore);
            }else if (info[0] == "generators") {
                _navigationStore.CurrentViewModel = new GeneratorsViewModel(info[1], _navigationStore);
            }else if (info[0] == "generator") {
                _navigationStore.CurrentViewModel = new GeneratorViewModel(info[1], _navigationStore);
            }else if(info[0] == "start") {
                _navigationStore.CurrentViewModel = new StartViewModel(_navigationStore);
            }
        }

        private void sendLkResOnSignal() {

            using (ResDbEntities rde = new ResDbEntities()) {
                MessageQueue billingQ = new MessageQueue();
                billingQ.Path = @".\private$\nekiQueue";

                Message m = new Message();
                m.Formatter = new XmlMessageFormatter((new Type[] { typeof(String) }));
                m.Body = JsonConvert.SerializeObject(rde.LkRes.ToList().Find(x => x.id == ((App)Application.Current).LkRes));
                billingQ.Send(m);
            }
        }
    }
}
