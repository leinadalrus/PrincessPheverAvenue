using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincessPheverAvenue.Functions
{
    internal class EasingFunctions
    {
        public Double EasInSine(Double d)
        {
            return 1 - Math.Cos((d * Math.PI) / 2);
        }

        public Double EaseOutSine(Double d)
        {
            return Math.Sin((d * Math.PI) / 2);
        }
    }
}
