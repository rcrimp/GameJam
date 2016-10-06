using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour {

    //player gets a total of x missiles. 
    //While there are missiles remaining, a missile prefab shows above the car
    //pressing space causes a missile to "shoot" in the same direction the car is facing at the time of keypress.
    //when the last missile is released, the prefab above is removed, as is this script. 

    public int nMissiles;
    public GameObject rocketPrefab;

    private int nRemainingMissiles;
    private GameObject displayRocket;


    // Use this for initialization
    void Start()
    {
        nRemainingMissiles = nMissiles;
        displayRocket = Instantiate(rocketPrefab, (transform.position + new Vector3(0, 2.5f, 0)), Quaternion.identity, transform) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            nRemainingMissiles--;
        }

        if (nRemainingMissiles == 0)
        {
            Destroy(displayRocket);
            Destroy(GetComponent<RocketLauncher>());
        }
    }

    public void Shoot()
    {
        GameObject currentRocket = Instantiate(rocketPrefab, (transform.position + new Vector3(0, 2.5f, 0)), Quaternion.identity, transform) as GameObject;
        currentRocket.GetComponent<ConstantForce>().enabled = true;
        currentRocket.GetComponent<BoxCollider>().enabled = true;
    }
}