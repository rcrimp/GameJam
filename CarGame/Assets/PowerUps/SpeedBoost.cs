using UnityEngine;
using System.Collections;

public class SpeedBoost : Powerup
{
    public float speedIncrease;

    void OnTriggerEnter(Collider collider)
    {
        NotifyOnPickup();

        Debug.Log(collider.name);
        Debug.Log(Vector3.forward * speedIncrease);       
        collider.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speedIncrease);
        Destroy(this.gameObject);
    }
}
