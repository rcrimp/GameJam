using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerControls : MonoBehaviour {

    public GameObject cameraObject;
    CarController car = null;
    //public bool multiplayer = false;
	// Use this for initialization
	void Start () {
        car = GetComponent<CarController>();
        gameObject.SetActive(true);
        /*if (multiplayer && !isLocalPlayer)
        {
            cameraObject.SetActive(false);
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        if (true){ //car != null && (!multiplayer || isLocalPlayer)) {
            float steer = Input.GetAxis("Horizontal");
            float gas = Input.GetAxis("Vertical");
            car.updateInput(steer, gas);
        }
	}
}
