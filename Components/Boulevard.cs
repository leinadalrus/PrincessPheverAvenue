using PrincessPheverAvenue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincessPheverAvenue.Components
{
    public class Boulevard : BuildingObserver
    {
        public Boulevard(BuildingObserver _observer) : base() { }

        public void Notify(AvenueBuilding[] buildings, Int32[] eventIds) { }
    }
}
