using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

    public GameObject explosionPrefab;

    void OnCollisionEnter(Collision collision)
    {
        //gets a contact position of the collision
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;

        Instantiate(explosionPrefab, position, Quaternion.identity);
        Destroy(gameObject);
    }
}
