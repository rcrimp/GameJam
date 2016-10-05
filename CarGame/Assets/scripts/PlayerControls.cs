using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    CarController car = null;

	// Use this for initialization
	void Start () {
        car = GetComponentInChildren<CarController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (car != null) {
            float steer = Input.GetAxis("Horizontal");
            float gas = Input.GetAxis("Vertical");
            car.updateInput(steer, gas);
        }
	}
}
