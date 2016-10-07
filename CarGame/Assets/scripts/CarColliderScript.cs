using UnityEngine;
using System.Collections;

public class CarColliderScript : MonoBehaviour {

    public Collider next = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider car)
    {
        Debug.Log(car.gameObject.name + "Just entered the box collider ");

        car.GetComponent<AIControls>().Follow(next.transform);//.target = next.transform;
    }
}
