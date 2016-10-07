using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindPowerUp : AIState
{
    // The powerup this ai is moving towards
    private GameObject targetPowerUp;

    public FindPowerUp(AIControls ai, CarController car)
        : base(ai, car) { /* Nothing */ }

    public override void Initialize()
    {
        base.Initialize();

        // Find the closest powerup and move towards it when a path has been calculated
        // Find a different target
        GameObject closestPowerUp = ClosestPowerup(targetPowerUp);
        if (closestPowerUp == null)      // No powerups available... Switch to evasive manoeuvres until a powerup is available
            ai.SetState(new EvasiveManoeuvres(ai, car));
        else
            MoveToPowerUp(closestPowerUp);

        targetPowerUp = closestPowerUp;
    }

    /// <summary>
    /// Finds the closest powerup in the scene. Closest by ABSOLUTE distance, not by PATH distance. Returns null
    /// if there are no available powerups in the scene.
    /// </summary>
    private GameObject ClosestPowerup(GameObject except)
    {
        // Get all powerups in the scene
        IEnumerable<GameObject> powerups = GameObject.FindGameObjectsWithTag("PowerUp").Where(go => go != except);

        // Order powerups by distance to this car
        powerups.OrderBy(p => Vector3.Distance(car.transform.position, p.transform.position));

        // If there are no powerups
        if (powerups == null)
            return null;

        // Return closest powerup
        return powerups.First();
    }

    /// <summary>
    /// Calculates a path to the closest powerup in the scene. 
    /// </summary>
    private void MoveToPowerUp(GameObject powerUp)
    {
        // Listen for when the powerup is removed
        powerUp.GetComponent<Powerup>().PickedUp += Powerup_PickedUp;

        // Set closest powerup as target
        targetPowerUp = powerUp;

        // Calculate path to the closets powerup
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(car.transform.position, powerUp.transform.position, NavMesh.AllAreas, path);

        // Follow path
        ai.Follow(path.corners);
    }

    /// <summary>
    /// Event raised when THIS car picks up a powerup
    /// </summary>
    private void Powerup_PickedUp(object sender, PickupEventArgs e)
    {
        // Stop listeneing to powerup
        ((Powerup)sender).PickedUp -= Powerup_PickedUp;

        // Powerup was picked up by a DIFFERENT car
        if (e.Collector != car.gameObject)
        {
            // Find a different target
            GameObject closestPowerUp = ClosestPowerup(targetPowerUp);
            if (closestPowerUp == null)      // No powerups available... Switch to evasive manoeuvres until a powerup is available
                ai.SetState(new EvasiveManoeuvres(ai, car));
            else
                MoveToPowerUp(closestPowerUp);

            targetPowerUp = closestPowerUp;
        }

        else    // Powerup was pickedup by THIS car
        {
            // IF powerup is a rocket launcher:
            if (sender is GainMissiles)
                ai.SetState(new HuntEnemy(ai, car));            // Switch to hunt enemies mode

            else
            {
                // Find a different target
                GameObject closestPowerUp = ClosestPowerup(targetPowerUp);
                if (closestPowerUp == null)      // No powerups available... Switch to evasive manoeuvres until a powerup is available
                    ai.SetState(new EvasiveManoeuvres(ai, car));
                else
                    MoveToPowerUp(closestPowerUp);  // Move to the next powerup

                targetPowerUp = closestPowerUp;
            }
        }
    }
}
