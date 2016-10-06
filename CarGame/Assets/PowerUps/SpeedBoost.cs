using UnityEngine;
using System.Collections;

public class SpeedBoost : Powerup
{
    public float speedIncrease;

    void OnTriggerEnter(Collider collider)
    {
        NotifyOnPickup(); //notify's any listeners that the powerup has been picked up
     
        collider.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speedIncrease);//adds force to colliding object
        Destroy(this.gameObject); //destroys powerup 
    }
}
