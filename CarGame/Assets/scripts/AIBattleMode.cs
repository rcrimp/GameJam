//using UnityEngine;
//using System.Collections;
//using System;

//namespace CarGame
//{
//    [RequireComponent(typeof(CarController))]
//    public class AIBattleMode : MonoBehaviour
//    {
//        private AIControls ai;
//        private AIState state;

//        void Awake()
//        {
//            car = GetComponent<CarController>();
//        }

//        void Start()
//        {
//            SetState(new FindPowerUp(this, car));
//        }

//        /// <summary>
//        /// Use SetState to change the AI state
//        /// </summary>
//        public void SetState(AIState state)
//        {
//            // Notify current state it is ending
//            if (this.state != null)
//                this.state.Terminate();

//            // Override current state with new state
//            this.state = state;

//            // Initialize the new state
//            state.Initialize();
//        }

//        public void SetPath(Vector3[] path)
//        {
//            StartCoroutine(FollowPath(path));
//        }

//        IEnumerator FollowPath(Vector3[] path)
//        {
//            // Move to each point in path
//            foreach (Vector3 point in path)
//            {
//                if (Vector3.Distance(transform.position, point) > 1)
//                {
//                    // Orient towards point
//                    yield return StartCoroutine(OrientTowards(point, 10));

//                    // Move towards point
//                    float distanceToTravel = Vector3.Distance(transform.position, point);
//                    yield return StartCoroutine(MoveForward(distanceToTravel));
//                }
//            }
//        }

//        IEnumerator OrientTowards(Vector3 target, float maximumAngle)
//        {
//            //// While angle from target is greater than the maximum allowed angle...
//            //float angle = Vector3.Angle(transform.forward, transform.position - target);
//            //while (angle > maximumAngle)
//            //{
//            //    // Is the target to the left or to the right...
//            //    float dir = MathExtension.AngleDir(transform.forward, transform.position - target, Vector3.up);

//            //    // Target is to the left; go Left
//            //    if (car.Turning == null && dir <= 0)
//            //        car.TurnLeft(target);

//            //    // Target is to the right; go right
//            //    else if (car.Turning == null && dir > 0)
//            //        car.TurnRight(target);

//            //    // Wait til next frame
//            //    yield return null;

//            //    angle = Vector3.Angle(transform.position + transform.forward, target - transform.position);
//            //}
//            yield return null;
//        }

//        IEnumerator MoveForward(float distance)
//        {
//            //// Car's position when this coroutine is begun
//            //Vector3 startPosition = transform.position;
//            //float distanceCovered = 0;

//            //// While the car has not moved the given distance
//            //while (distanceCovered < distance)
//            //{
//            //    // Continue to accelerate
//            //    car.Accelerate();

//            //    // Measure the distance covered
//            //    distanceCovered = Vector3.Distance(transform.position, startPosition);

//            //    // Wait til next frame
//            //    yield return null;
//            //}

//            yield return null;
//        }
        
//        /*
//            Modes:
//                Find power-up - Calculate path to closest available power-up
//                                Drive there
//                Attack Enemy - Check what weapon I have change behaviour accordingly
//                                Missile:
//                                        Calculate path to closest enemy
//                                        Drive towards
//                                        When within range AND LOS: shoot                            
//                Evasive maneouvers - Move away from players
//                                     When away from players, drive to random location
//        */

//    }
//}