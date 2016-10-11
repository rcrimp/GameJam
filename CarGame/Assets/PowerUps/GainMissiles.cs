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
        if(collider.gameObject.tag == "Car")
        {
            GameObject collidingObject = collider.gameObject; //saves a ref to the colliding object's gameobject (for later readability)

            //if the colliding thing doesn't have a rocket launcher
            if (collidingObject.GetComponent<RocketLauncher>() == null)
            {
                //give it a rocket launcher
                collidingObject.AddComponent<RocketLauncher>(); //adds a rocket launcher script to the colliding game object
                RocketLauncher launcher = collidingObject.GetComponent<RocketLauncher>(); //saves ref to the new rocket launcher
                launcher.nMissiles = nMissiles; //tells the new  launcher how many missiles it should have
                launcher.rocketPrefab = rocketPrefab; //gives the rocketPrefab to the new launcher
            }
            //otherwise, give the existing launcher more missiles
            else
            {
                collidingObject.GetComponent<RocketLauncher>().nRemainingMissiles += nMissiles;
            }


            NotifyOnPickup(collidingObject);//notify's any listeners that the powerup has been picked up

            Destroy(this.gameObject); //destroys powerup
        }

    }


}
