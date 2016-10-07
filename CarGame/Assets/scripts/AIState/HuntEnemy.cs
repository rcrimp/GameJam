using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HuntEnemy : AIState
{
    private RocketLauncher launcher;
    private IEnumerable<Transform> cars;
    private Transform target;

    public HuntEnemy(AIControls ai, CarController car)
        : base(ai, car)
    {
        launcher = ai.GetComponent<RocketLauncher>();
    }

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

        // Out of ammo, find another power up
        if (launcher.nRemainingMissiles == 0)
            ai.SetState(new FindPowerUp(ai, car));

        // Target in firing arc... ANNIHILATE!
        if (InCrosshair(target.position))
            launcher.Shoot();
    }

    public bool InCrosshair(Vector3 target)
    {
        // Angle from car's facing to the desired facing
        Vector3 desiredFacing = target - car.transform.position;
        float angleToTarget = Vector3.Angle(car.transform.forward, desiredFacing);

        return angleToTarget < ai.AttackArc;
    }
}
