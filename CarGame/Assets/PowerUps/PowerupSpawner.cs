﻿using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour {

    /// <summary>
    /// 
    /// </summary>
    
    public GameObject rocketPrefab;
    public GameObject burgerPrefab;
    public GameObject powerUpPrefab;
    public float powerupRespawnDelay = 5;
    

    // Use this for initialization
    void Start () {

        //creates a powerup for every child of the component's gameobject (i.e. powerup spawn point)
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i);
            CreateRandomPowerup(transform.GetChild(i).position);
        }
    }
	
    //creates a randomly powered powerup at the given location
    public void CreateRandomPowerup(Vector3 spawnpoint)
    {
        //instantiates a power up prefab
        GameObject powerUp = (GameObject)Instantiate(powerUpPrefab, spawnpoint, Quaternion.identity);

        //gives the prefab a random power
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                CreateSpeedBoost(powerUp);
                break;
            case 1:
                CreateGainMissiles(powerUp);
                break;
            case 2:
                CreateBurger(powerUp);
                break;
            default:
                break;
        }

        //starts listening for the expenditure of this new power up.
        powerUp.GetComponent<Powerup>().PickedUp += PowerupSpawner_PickedUp;
    }

    private void PowerupSpawner_PickedUp(object sender, PickupEventArgs e)
    {
        //when a power up is expended, remember its location, wait for a bit, then create another powerup there
        Powerup expendedPowerUp = (Powerup)sender;
        StartCoroutine(SpawnAfterDelay(expendedPowerUp.transform.position, powerupRespawnDelay));       
    }

    public void CreateSpeedBoost(GameObject powerUp) //gives the speed boost effect to the given powerUp
    {
        powerUp.AddComponent<SpeedBoost>();
        SpeedBoost speedBoost = powerUp.GetComponent<SpeedBoost>(); //(Makes the next few lines easier to read)
        speedBoost.speedIncrease = 1000000;
    }

    public void CreateGainMissiles(GameObject powerUp)
    {
        powerUp.AddComponent<GainMissiles>();
        GainMissiles gainMissiles = powerUp.GetComponent<GainMissiles>();//(Makes the next few lines easier to read)
        gainMissiles.nMissiles = 5;
        gainMissiles.rocketPrefab = rocketPrefab;
    }

    public void CreateBurger(GameObject powerUp)
    {
        powerUp.AddComponent<Burger>();
        Burger burger = powerUp.GetComponent<Burger>();
        burger.burgerPrefab = burgerPrefab;
    }
  
    IEnumerator SpawnAfterDelay(Vector3 location, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        CreateRandomPowerup(location);
    }

    //on the event of a powerup collision, tell me the location of the powerup
    
}
