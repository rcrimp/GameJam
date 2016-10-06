using System;
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
        bool powerupFound = MoveToClosestPowerup();
        if (powerupFound == false)  // Switch to evasive manoeuvres until a powerup is available
            ai.SetState(new EvasiveManoeuvres(ai, car));
    }

    /// <summary>
    /// Finds the closest powerup in the scene. Closest by ABSOLUTE distance, not by PATH distance. Returns null
    /// if there are no available powerups in the scene.
    /// </summary>
    private GameObject FindClosestPowerup()
    {
        // Get all powerups in the scene
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerUp");
        if (powerups.Length == 0)
            return null;

        // Order powerups by distance to this car
        powerups.OrderBy(p => Vector3.Distance(car.transform.position, p.transform.position));

        // Return the closest powerup
        return powerups[0];
    }

    /// <summary>
    /// Calculates a path to the closest powerup in the scene. 
    /// </summary>
    private bool MoveToClosestPowerup()
    {
        // Get closest powerup
        GameObject closestPowerup = FindClosestPowerup();

        // No powerup found
        if (closestPowerup == null)
            return false;

        // Set closest powerup as target
        targetPowerUp = closestPowerup;

        // Calculate path to the closets powerup
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(car.transform.position, closestPowerup.transform.position, NavMesh.AllAreas, path);

        // Follow path
        ai.Follow(path.corners);

        // Powerup has been found successfully
        return true;
    }

    /// <summary>
    /// Event raised when THIS car picks up a powerup
    /// </summary>
    private void Powerup_PickedUp(object sender, EventArgs e)
    {
        // Switch to hunt enemies mode
    }

    /// <summary>
    /// Event raised when the powerup targeted by this ai is removed (picked up by another car, destroyed etc)
    /// </summary>
    private void Powerup_Removed(object sender, EventArgs e)
    {
        // Target no longer valid, delete reference
        targetPowerUp = null;

        // Find a difference target
        bool powerupFound = MoveToClosestPowerup();
        if (powerupFound == false)      // No powerups available... Switch to evasive manoeuvres until a powerup is available
            ai.SetState(new EvasiveManoeuvres(ai, car));
    }
}
