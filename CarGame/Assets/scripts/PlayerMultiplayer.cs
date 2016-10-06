using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMultiplayer : NetworkBehaviour {

    public GameObject cameraObject;

    // Use this for initialization
    void Start () {
	    if (!isLocalPlayer) {
            cameraObject.SetActive(false);
            GetComponent<PlayerControls>().serverControlled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
