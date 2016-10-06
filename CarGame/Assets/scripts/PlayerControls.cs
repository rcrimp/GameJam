using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerControls : NetworkBehaviour {

    public GameObject cameraObject;
    CarController car = null;

	// Use this for initialization
	void Start () {
        car = GetComponent<CarController>();
        if (!isLocalPlayer)
        {
            cameraObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (car != null && isLocalPlayer) {
            float steer = Input.GetAxis("Horizontal");
            float gas = Input.GetAxis("Vertical");
            car.updateInput(steer, gas);
        }
	}
}
