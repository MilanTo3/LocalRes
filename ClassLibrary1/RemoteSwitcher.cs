using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Switcher
    {

        public int lkresid { get; set; }
        public int generatorid { get; set; }
        public string state { get; set; }

        public Switcher(int lkid, int gid, string stateSwitch) {

            lkresid = lkid;
            generatorid = gid;
            state = stateSwitch;

        }
    }
}
