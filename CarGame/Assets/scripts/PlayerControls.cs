using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerControls : MonoBehaviour {

    CarController car = null;
    public bool serverControlled = false;

	// Use this for initialization
	void Start () {
        car = GetComponent<CarController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (car != null && !serverControlled) {
            float steer = Input.GetAxis("Horizontal");
            float gas = Input.GetAxis("Vertical");
            car.updateInput(steer, gas);
        }
	}
}
