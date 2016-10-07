using UnityEngine;
using System.Collections;

public class Burger : Powerup
{
    public GameObject burgerPrefab;

    void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.AddComponent<BecomeABurgerForABit>();
        collider.gameObject.GetComponent<BecomeABurgerForABit>().burgerPrefab = burgerPrefab;
            
        NotifyOnPickup(collider.gameObject);
        Destroy(this.gameObject); //destroys powerup        
    }
}
