using UnityEngine;
using System.Collections;

public class Burger : Powerup
{
    public GameObject burgerPrefab;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Car")
        {
            collider.gameObject.AddComponent<BecomeABurgerForABit>();
            collider.gameObject.GetComponent<BecomeABurgerForABit>().burgerPrefab = burgerPrefab;

            NotifyOnPickup(collider.gameObject);
            Destroy(this.gameObject); //destroys powerup       
        } 
    }
}
