using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour {

    /// <summary>
    /// 
    /// </summary>
    
    public GameObject rocketPrefab;
    public GameObject powerUpPrefab;
    public float powerupRespawnDelay = 5;
    

    // Use this for initialization
    void Start () {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i);
            CreateRandomPowerup(transform.GetChild(i).position);
        }
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
                break;
        }

        powerUp.GetComponent<Powerup>().PickedUp += PowerupSpawner_PickedUp;
    }

    private void PowerupSpawner_PickedUp(object sender, System.EventArgs e)
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
  
    IEnumerator SpawnAfterDelay(Vector3 location, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        CreateRandomPowerup(location);
    }

    //on the event of a powerup collision, tell me the location of the powerup
    
}
