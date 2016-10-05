using UnityEngine;
using System.Collections;
using System;

namespace CarGame
{
    [RequireComponent(typeof(CarDriving))]
    public class AIBattleMode : MonoBehaviour
    {
        private CarDriving car;
        private AIState state;

        void Awake()
        {
            car = GetComponent<CarDriving>();
        }

        /// <summary>
        /// Use SetState to change the AI state
        /// </summary>
        public void SetState(AIState state)
        {
            // Notify current state it is ending
            this.state.Terminate();

            // Override current state with new state
            this.state = state;

            // Initialize the new state
            state.Initialize();
        }

        public void SetPath(Vector3[] path)
        {
            StartCoroutine(FollowPath(path));
        }

        IEnumerator FollowPath(Vector3[] path)
        {
            // Move to each point in path
            foreach (Vector3 point in path)
            {
                // Orient towards point
                yield return StartCoroutine(OrientTowards(point, 0.1f));

                // Move towards point
                float distanceToTravel = Vector3.Distance(transform.position, point);
                yield return StartCoroutine(MoveForward(distanceToTravel));
            }
        }

        IEnumerator OrientTowards(Vector3 target, float maximumAngle)
        {
            // While angle from target is greater than the maximum allowed angle...
            while (Vector3.Angle(transform.position + transform.forward, target - transform.position) > maximumAngle)
            {
                // Is the target to the left or to the right...
                float dir = MathExtension.AngleDir(transform.position + transform.forward, target - transform.position, Vector3.up);

                // Target is to the left; go Left
                if (dir < 0)
                    car.TurnLeft();

                // Target is to the right; go right
                else if (dir > 0)
                    car.TurnRight();

                // Wait til next frame
                yield return null;
            }
        }

        IEnumerator MoveForward(float distance)
        {
            // Car's position when this coroutine is begun
            Vector3 startPosition = transform.position;
            float distanceCovered = 0;

            // While the car has not moved the given distance
            while (distanceCovered < distance)
            {
                // Continue to accelerate
                car.Accelerate();

                // Measure the distance covered
                distanceCovered = Vector3.Distance(transform.position, startPosition);

                // Wait til next frame
                yield return null;
            }
        }
        
        /*
            Modes:
                Find power-up - Calculate path to closest available power-up
                                Drive there
                Attack Enemy - Check what weapon I have change behaviour accordingly
                                Missile:
                                        Calculate path to closest enemy
                                        Drive towards
                                        When within range AND LOS: shoot                            
                Evasive maneouvers - Move away from players
                                     When away from players, drive to random location
        */

    }
}