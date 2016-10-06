using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChaseTarget : AIState
{
    public ChaseTarget(AIControls ai, CarController car, Transform target) 
        : base(ai, car)
    {
        ai.Follow(target);  // Chase the target
    }
}
