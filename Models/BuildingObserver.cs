using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PrincessPheverAvenue.Components;

namespace PrincessPheverAvenue.Models
{
    public interface BuildingObserver
    {
        public void Notify(AvenueBuilding[] buildings, Int32[] eventIds) { }
    }
}
