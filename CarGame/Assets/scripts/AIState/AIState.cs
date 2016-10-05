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
        protected CarController car;

        public AIState(AIBattleMode ai, CarController car)
        {
            this.ai = ai;
            this.car = car;

            Debug.Log(GetType().ToString());
        }

        /// <summary>
        /// Use Initialize for any initial logic that may cause the AI to change state
        /// </summary>
        public virtual void Initialize() { /* Nothing*/ }

        /// <summary>
        /// Use Terminate for any logic to be executed by a state when it is ending.
        /// </summary>
        public virtual void Terminate() { /* Nothing */ }
    }
}
