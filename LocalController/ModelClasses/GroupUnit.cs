using System;
using System.Collections.Generic;
using System.Text;
using static LocalController.ModelClasses.Globals;

namespace LocalController.ModelClasses
{
    internal class GroupUnit
    {
        long id;
        UnitType unitType;
        double currentActivePower; //trenutna aktivna snaga.
        double minPower;
        double maxPower;
        Control control; // can be local or remote.
        double workPrice; // [$/kW]



    }
}
