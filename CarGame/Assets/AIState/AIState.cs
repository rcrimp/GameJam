using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CarGame
{
    public abstract class AIState
    {
        protected AIBattleMode ai;
        protected CarDriving car;

        public AIState(AIBattleMode ai, CarDriving car)
        {
            this.ai = ai;
            this.car = car;
        }

        /// <summary>
        /// Use initialize for any initial logic that may cause the AI to change state
        /// </summary>
        public virtual void Initialize() { /* Nothing*/ }
    }
}
