using ClassLibrary1;
using Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {

        static MessageQueue billingQ = new MessageQueue();

        static void Main(string[] args) {

            billingQ.Path = @".\private$\nekiQueue";
            billingQ.Purge();
            checkIfQueueExists();
            Thread thr2 = new Thread(measurementReceiver);
            thr2.Start();

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

        private static void remoteCurrentPowerCalculator() {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();
            int i;
            int j;
            Random randomClass = new Random();

            while (true) {


                rde.SaveChanges();
                Thread.Sleep(4000);
            }

        }

        public static void measurementReceiver() {

            while (true) {
                MessageQueueTransaction transaction = new MessageQueueTransaction();

                using (MessageQueue mq = new MessageQueue(@".\private$\nekiQueue")) {
                    transaction.Begin();
                    mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                    try {
                        Message[] m = mq.GetAllMessages();
                        mq.Purge();
                        List<MVVMSecondTry.Unit> unitsDataSerialized = new List<MVVMSecondTry.Unit>();
                        List<MVVMSecondTry.LkRe> lkresDataSerialized = new List<MVVMSecondTry.LkRe>();
                        List<Switcher> switches = new List<Switcher>(); 
                        foreach (Message message in m) {
                            try {
                                unitsDataSerialized.AddRange(JsonConvert.DeserializeObject<List<MVVMSecondTry.Unit>>(message.Body.ToString()));
                            }
                            catch {
                                try {
                                    lkresDataSerialized.Add(JsonConvert.DeserializeObject<MVVMSecondTry.LkRe>(message.Body.ToString()));
                                }
                                catch {
                                   
                                }
                            }
                            try {
                                switches.Add(JsonConvert.DeserializeObject<Switcher>(message.Body.ToString()));
                            }
                            catch {

                            }

                        }
                        obradiListu(unitsDataSerialized);
                        obradiLkResove(lkresDataSerialized);
                        obradiSwitcheve(switches);
                    }
                    catch {

                    }
                }

                transaction.Commit();

                Thread.Sleep(9000);
            }
        }

        private static void obradiSwitcheve(List<Switcher> switches) {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();

            int i;
            List<Unit> dbgenerators = rde.Units.ToList();
            for (i = 0; i < switches.Count; i++) {
                Unit temp = dbgenerators.Find(x => x.onLocal == switches[i].generatorid);
                if (temp != null) {

                    temp.ControlType = switches[i].state;
                    rde.SaveChanges();
                }

            }

        }

        private static void obradiLkResove(List<MVVMSecondTry.LkRe> listaInfo) {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();

            foreach (MVVMSecondTry.LkRe lkres in listaInfo) {
                if (rde.LkRes.ToList().Exists(x => x.onLocal == lkres.id) == false) {
                    LkRe newLkRES = new LkRe();
                    newLkRES.active = lkres.active;
                    newLkRES.name = lkres.name;
                    newLkRES.onLocal = lkres.id;

                    rde.LkRes.Add(newLkRES);
                }
            }

            rde.SaveChanges();

            int i;
            List<LkRe> dbgenerators = rde.LkRes.ToList();
            for (i = 0; i < listaInfo.Count; i++) {
                LkRe temp = dbgenerators.Find(x => x.onLocal == listaInfo[i].id);
                if (temp != null) {

                    temp.active = listaInfo[i].active;
                }

            }

            rde.SaveChanges();

            foreach (LkRe lkres in rde.LkRes.ToList()) {
                if (lkres.active == false) {
                    rde.LkRes.Remove(lkres);
                }
            }

            rde.SaveChanges();

        }

        private static void obradiListu(List<MVVMSecondTry.Unit> listaInfo) {

            SystemControllerDbEntities rde = new SystemControllerDbEntities();

            foreach (MVVMSecondTry.Unit unit in listaInfo) {
                if (rde.Units.ToList().Exists(x => x.onLocal == unit.id) == false) {
                    Unit newUnit = new Unit();
                    newUnit.CurrentActivePower = unit.CurrentActivePower;
                    newUnit.GroupId = unit.GroupId;
                    newUnit.lkresid = unit.lkresid;
                    newUnit.MaximumActivePower = unit.MaximumActivePower;
                    newUnit.MinimumActivePower = unit.MinimumActivePower;
                    newUnit.UnitType = unit.UnitType;
                    newUnit.ProductionPrice = unit.ProductionPrice;
                    newUnit.ControlType = unit.ControlType;
                    newUnit.onLocal = unit.id;

                    rde.Units.Add(newUnit);
                    rde.SaveChanges();
                }
            }

            int i;
            List<Unit> dbgenerators = rde.Units.ToList();
            for (i = 0; i < listaInfo.Count; i++) {
                Unit temp = dbgenerators.Find(x => x.onLocal == listaInfo[i].id);
                if (temp != null) {
                    
                    temp.CurrentActivePower = listaInfo[i].CurrentActivePower;
                }
                
            }
            
            rde.SaveChanges();

        }

    }
}
