using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    CarDriving car = null;

	// Use this for initialization
	void Start () {
        car = GetComponentInChildren<CarDriving>();
    }
	
	// Update is called once per frame
	void Update () {
        if (car != null) {
            // get input
            float steer = Input.GetAxis("Horizontal");
            float accelerate = Input.GetAxis("Vertical");

            if (Input.GetKeyDown("space")) car.up();
            // call car controller methods
            if (steer > 0) car.SteerLeft();
            if (steer < 0) car.SteerRight();
            if (accelerate > 0) car.Accelerate();
            if (accelerate < 0) car.Brake();
        }
	}
}
