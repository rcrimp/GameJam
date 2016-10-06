using UnityEngine;
using System.Collections;

public class CheckPointActivated : MonoBehaviour {

    public Collider[] CheckPoints;
   	// Use this for initialization
	void Start () {
        Collider previous = null;
        foreach (Collider currentCollider in CheckPoints)
        {
            if (previous != null)
            {
                previous.GetComponent<ColliderScript>().next = currentCollider;
            }
            previous = currentCollider;
        }
        previous.GetComponent<ColliderScript>().next = CheckPoints[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
