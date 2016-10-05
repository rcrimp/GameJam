using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour {

    public int MaxNumberOfPowerups;
    public GameObject powerUpPrefab;
    public GameObject track;
    Bounds trackBounds;

	// Use this for initialization
	void Start () {

        trackBounds = track.GetComponent<Renderer>().bounds;
        SpawnPowerUps(MaxNumberOfPowerups);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnPowerUps(int nPowerUps)
    {
        for (int i = 0; i < nPowerUps; i++)
        {
            CreateRandomPowerup();
        }        
    }

    public void CreateRandomPowerup()
    {
        //create powerup prefab at random location within bounds of the track
        float x = trackBounds.center.x + Random.Range(-trackBounds.extents.x, trackBounds.extents.x);
        float z = trackBounds.center.z + Random.Range(-trackBounds.extents.z, trackBounds.extents.z);
        GameObject powerUp = (GameObject)Instantiate(powerUpPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);

        //give the prefab a random power
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                CreateSpeedBoost(powerUp);
                break;
            default:
                Debug.Log("Creating an exciting new powerup");
                break;
        }


    }

    public void CreateSpeedBoost(GameObject powerUp)
    {
        powerUp.AddComponent<SpeedBoost>();
        SpeedBoost speedBoost = powerUp.GetComponent<SpeedBoost>();
        speedBoost.boostDuration = 0.5f;
        speedBoost.speedIncrease = 10;
        speedBoost.slowDownDuration = 2;
        Debug.Log("SpeedBoost");
    }
}
