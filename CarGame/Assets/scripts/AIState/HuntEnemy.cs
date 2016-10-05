using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    public class HuntEnemy : AIState
    {
        public HuntEnemy(AIBattleMode ai, CarController car)
            : base(ai, car) { /* Nothing */ }
    }
}
