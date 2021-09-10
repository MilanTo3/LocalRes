using MVVMSecondTry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args) {

            Thread thr1 = new Thread(localResCalculating);
            thr1.Start();

            Console.ReadLine();
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
                Thread.Sleep(4000);
            }

        }
    }
}
