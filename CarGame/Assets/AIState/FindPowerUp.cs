using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CarGame
{
    public class FindPowerUp : AIState
    {
        public FindPowerUp(AIBattleMode ai, CarDriving car)
            : base(ai, car)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            // If has a power-up, change to hunt enemy mode
            // Else,
            // Locate closest power-up
            // Calculate path
            // Move to it's position

            // Get all powerups in the scene
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerUp");
            if (powerups.Length > 0)
            {
                // Order powerups by distance to this car
                powerups.OrderBy(p => Vector3.Distance(car.transform.position, p.transform.position));

                // Target the closest powerup
                GameObject targetPowerup = powerups[0];

                // Begin calculating path to closest powerup
                ai.Pathfinder.FindPath(targetPowerup.transform.position);
                ai.Pathfinder.PathFound += Pathfinder_PathFound;
            }
        }

        private void Pathfinder_PathFound(object sender, PathfinderEventArgs e)
        {
            ai.FollowPath(e.Path);
        }
    }
}
