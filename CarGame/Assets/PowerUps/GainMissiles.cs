using UnityEngine;
using System.Collections;

public class GainMissiles : Powerup {

    /// <summary>
    /// Assigns a rocket launcher to the colliding game object 
    /// </summary>
    /// 

    public int nMissiles;
    public GameObject rocketPrefab;

    void OnTriggerEnter(Collider collider)
    {
        NotifyOnPickup();

        GameObject collidingObject = collider.gameObject; //saves a ref to the colliding object's gameobject (for later readability)
                       
        collidingObject.AddComponent<RocketLauncher>(); //adds a rocket launcher script to the colliding game object
        RocketLauncher launcher = collidingObject.GetComponent<RocketLauncher>(); //saves ref to the new rocket launcher
        launcher.nMissiles = nMissiles; //tells the new  launcher how many missiles it should have
        launcher.rocketPrefab = rocketPrefab; //gives the rocketPrefab to the new launcher

        Destroy(this.gameObject); //destroys this script
    }


}
