using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChaseTarget : AIState
{
    private Transform target;

    public ChaseTarget(AIControls ai, CarController car, Transform target) 
        : base(ai, car)
    {
        this.target = target;

        ai.Follow(target);  // Chase the target
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        if (target != null)
            Gizmos.DrawLine(car.transform.position, target.position);
    }
}
