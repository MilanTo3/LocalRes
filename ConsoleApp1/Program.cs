using MVVMSecondTry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {

        static MessageQueue billingQ = new MessageQueue();

        static void Main(string[] args) {

            billingQ.Path = @".\private$\nekiQueue";
            checkIfQueueExists();
            Thread thr1 = new Thread(localResCalculating);
            thr1.Start();

            Console.ReadLine();
        }

        private static void checkIfQueueExists() {
            if (MessageQueue.Exists(billingQ.Path)) {
                return;
            }

            else {
                MessageQueue.Create(billingQ.Path);
            }
        }

        private static void localResCalculating() {

            ResDbEntities rde = new ResDbEntities();
            int i;
            int j;
            Random randomClass = new Random();

            while (true) {
                List<LkRe> activeLkResList = rde.LkRes.Where(x => x.active == true).ToList();

                // sve grupe u aktivnom lkres-u:
                List<UnitGroup> groups = new List<UnitGroup>();

                foreach (LkRe activelkres in activeLkResList) {
                    groups.AddRange(rde.UnitGroups.Where(x => x.lkResId == activelkres.id).ToList());
                }

                List<Unit> units;

                for (i = 0; i < groups.Count; i++) {

                    int id = groups[i].id;
                    units = rde.Units.Where(x => x.GroupId == id && x.ControlType == "Local").ToList();

                    for (j = 0; j < units.Count; j++) {
                        units[j].CurrentActivePower = randomClass.Next((int)units[j].MinimumActivePower, (int)units[j].MaximumActivePower);
                    }

                    groups[i].CurrentProduction = units.Sum(x => x.CurrentActivePower);
                    if (groups[i].MaxProduction < units.Sum(x => x.CurrentActivePower)) {
                        groups[i].MaxProduction = units.Sum(x => x.CurrentActivePower);
                    }

                }
          
                rde.SaveChanges();
                List<Unit> tosend = new List<Unit>();
                foreach (LkRe re in activeLkResList) {
                    tosend.AddRange(rde.Units.Where(x => x.lkresid == re.id).ToList());
                }
                SendMeasurements(tosend);
                Thread.Sleep(3000);
            }

        }

        private static void SendMeasurements(List<Unit> measurements) {

            Message m = new Message();
            m.Formatter = new XmlMessageFormatter((new Type[] { typeof(String) }));
            m.Body = JsonConvert.SerializeObject(measurements);
            billingQ.Send(m);

            /*
            using (MessageQueueTransaction mqt = new MessageQueueTransaction()) {
                mqt.Begin();
                Message message = new Message();
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                message.Body = JsonConvert.SerializeObject(measurements);
                billingQ.Send(message, mqt);
                mqt.Commit();
            }*/
        }
    }
}
