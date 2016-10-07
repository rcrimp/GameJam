using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour {

    //player gets a total of x missiles. 
    //While there are missiles remaining, a missile prefab shows above the car
    //pressing space causes a missile to "shoot" in the same direction the car is facing at the time of keypress.
    //when the last missile is released, the prefab above is removed, as is this script. 

    public int nMissiles;
    public GameObject rocketPrefab;

    public int nRemainingMissiles { get; private set; }
    private GameObject displayRocket;

    private float reloadDelayInSeconds = 0.5f;
    private bool reloading = false;

    // Use this for initialization
    void Start()
    {
        nRemainingMissiles = nMissiles;

        //hack fix: not sure why, but missiles and car's have opposite y facing. this is my hack fix for that.
        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.Euler(0, 180, 0); // this add a 180 degrees Y rotation

        displayRocket = Instantiate(rocketPrefab, (transform.position + new Vector3(0, 2f, 0)), rotation, transform) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!reloading)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }

        if (nRemainingMissiles == 0)
        {
            Destroy(displayRocket);
            Destroy(GetComponent<RocketLauncher>());
        }
    }

    IEnumerator ReloadDelay()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadDelayInSeconds);
        reloading = false;
    }

    public void Shoot()
    {
        nRemainingMissiles--;

        //hack fix: not sure why, but missiles and car's have opposite y facing. this is my hack fix for that.
        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.Euler(0, 180, 0); // this add a 180 degrees Y rotation

        GameObject currentRocket = Instantiate(rocketPrefab, (transform.position + new Vector3(0, 2.5f, 0)), rotation) as GameObject;
        currentRocket.GetComponent<Rigidbody>().isKinematic = false;
        currentRocket.GetComponent<ConstantForce>().enabled = true;
        currentRocket.GetComponent<BoxCollider>().enabled = true;
    }


}