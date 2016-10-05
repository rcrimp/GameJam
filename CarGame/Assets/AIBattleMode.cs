using UnityEngine;
using System.Collections;
using System;

namespace CarGame
{
    [RequireComponent(typeof(CarDriving))]
    [RequireComponent(typeof(Pathfinder))]
    public class AIBattleMode : MonoBehaviour
    {
        public Pathfinder Pathfinder { get; private set; }
        private CarDriving car;

        void Awake()
        {
            Pathfinder = GetComponent<Pathfinder>();
            car = GetComponent<CarDriving>();
        }

        public void FollowPath(Vector3[] path)
        {
            // foreach point in path
            // orient towards
            // move towards
            //
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