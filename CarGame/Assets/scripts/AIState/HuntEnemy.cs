using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HuntEnemy : AIState
{
    private IEnumerable<Transform> cars;
    private Transform target;

    public HuntEnemy(AIControls ai, CarController car)
        : base(ai, car) { /* Nothing */ }

    public override void Update()
    {
        base.Update();

        // Get collection of other cars transforms (so their position can be tracked)
        if (cars == null)
            cars = GameObject.FindGameObjectsWithTag("Car").Select(go => go.transform).Where(t => t != car.transform);

        // Order cars by distance from this car
        cars.OrderBy(t => Vector3.Distance(car.transform.position, t.position));

        // If the closest car is not the car currently being chased
        if (cars.First() != target)
        {
            target = cars.First();       // Overwrite ref to closest car
            ai.Follow(target);           // Chase new closest car
        }

        if (InCrosshair(target.position))
        {
            ai.GetComponent<RocketLauncher>().Shoot();
        }
    }

    public bool InCrosshair(Vector3 target)
    {
        // Angle from car's facing to the desired facing
        Vector3 desiredFacing = target - car.transform.position;
        float angleToTarget = Vector3.Angle(car.transform.forward, desiredFacing);

        return angleToTarget < ai.AttackArc;
    }
}
