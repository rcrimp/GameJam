using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour {

    public GameObject rocketPrefab;
    public GameObject powerUpPrefab;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i);
            CreateRandomPowerup(transform.GetChild(i).position);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void CreateRandomPowerup(Vector3 spawnpoint)
    {
        GameObject powerUp = (GameObject)Instantiate(powerUpPrefab, spawnpoint, Quaternion.identity);

        //give the prefab a random power
        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                CreateSpeedBoost(powerUp);
                break;
            case 1:
                CreateGainMissiles(powerUp);
                break;
            default:
                Debug.Log("Creating an exciting new powerup");
                break;
        }
    }

    public void CreateSpeedBoost(GameObject powerUp) //gives the speed boost effect to the given powerUp
    {
        powerUp.AddComponent<SpeedBoost>();
        SpeedBoost speedBoost = powerUp.GetComponent<SpeedBoost>(); //(Makes the next few lines easier to read)
        speedBoost.boostDuration = 0.5f;
        speedBoost.speedIncrease = 1000000;
        speedBoost.slowDownDuration = 2;
        Debug.Log("SpeedBoost powerup created");
    }

    public void CreateGainMissiles(GameObject powerUp)
    {
        powerUp.AddComponent<GainMissiles>();
        GainMissiles gainMissiles = powerUp.GetComponent<GainMissiles>();//(Makes the next few lines easier to read)
        gainMissiles.nMissiles = 5;
        gainMissiles.rocketPrefab = rocketPrefab;
        Debug.Log("Missile powerup created");
    }
}
