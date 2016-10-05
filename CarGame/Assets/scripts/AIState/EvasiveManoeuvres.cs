using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    public class EvasiveManoeuvres : AIState
    {
        public EvasiveManoeuvres(AIBattleMode ai, CarDriving car)
            : base(ai, car) { /* Nothing */ }

        // Try to keep a certain distance away from enemies
    }
}
