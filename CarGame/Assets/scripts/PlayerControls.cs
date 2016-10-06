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
<<<<<<< HEAD
        gameObject.SetActive(true);
        /*if (multiplayer && !isLocalPlayer)
        {
            cameraObject.SetActive(false);
        }*/
=======
>>>>>>> 9b29720a91d58f8f251cbf62d39f1f4a78caaf60
    }
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
        if (true){ //car != null && (!multiplayer || isLocalPlayer)) {
=======
        if (car != null) {
>>>>>>> 9b29720a91d58f8f251cbf62d39f1f4a78caaf60
            float steer = Input.GetAxis("Horizontal");
            float gas = Input.GetAxis("Vertical");
            car.updateInput(steer, gas);
        }
	}
}
