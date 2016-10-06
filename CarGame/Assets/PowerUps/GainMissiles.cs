using UnityEngine;
using System.Collections;

public class GainMissiles : MonoBehaviour {

    //player gets a total of x missiles. 
    //While there are missiles remaining, a missile prefab shows above the car
    //pressing space causes a missile to "shoot" in the same direction the car is facing at the time of keypress.
    //when the last missile is released, the prefab above is removed, as is this script. 

    public int nMissiles;
    public GameObject rocketPrefab;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {        
    }

    void OnTriggerEnter(Collider collider)
    {
        GetComponent<Renderer>().enabled = false;

        GameObject collidingObject = collider.gameObject;
        //collidingObject.transform.parent.gameObject.AddComponent<RocketLauncher>();
               
        collidingObject.AddComponent<RocketLauncher>();
        RocketLauncher launcher = collidingObject.GetComponent<RocketLauncher>();
        launcher.nMissiles = nMissiles;
        launcher.rocketPrefab = rocketPrefab;

        Destroy(this.gameObject);
    }
}
