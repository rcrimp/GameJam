using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EvasiveManoeuvres : AIState
{
    private IEnumerable<Transform> cars;
    private Transform target;

    public EvasiveManoeuvres(AIControls ai, CarController car)
        : base(ai, car)
    {
        // TODO: listen for powerup spawn
    }

    public override void Update()
    {
        base.Update();

        // Get collection of other cars transforms (so their position can be tracked)
        if (cars == null)
            cars = GameObject.FindGameObjectsWithTag("Car").Select(go => go.transform).Where(t => t != car.transform);

        // Order cars by distance from this car
        cars.OrderBy(t => Vector3.Distance(car.transform.position, t.position));

        // If the closest car is not the car currently being avoided
        if (cars.First() != target)
        {
            target = cars.First();      // Overwrite ref to closest car
            ai.Avoid(target);           // Avoid new closest car
        }
    }

    public void Powerup_Spawned(object sender, EventArgs e)
    {
        ai.SetState(new FindPowerUp(ai, car));
    }
}
