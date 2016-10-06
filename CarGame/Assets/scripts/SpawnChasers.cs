using UnityEngine;
using System.Collections;

public class SpawnChasers : MonoBehaviour {

    public int count;
    public GameObject chaser;

	// Use this for initialization
	void Start () {
        GameObject first = null;
        GameObject prev = null;
        for (int i = 0; i < count; i++)
        {
            GameObject curr = Instantiate(chaser);
            curr.transform.position = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));
            if (first == null) first = curr;
            if (prev != null)
            {
                prev.GetComponent<AIControls>().target = curr.transform;
            }
            prev = curr;
        }
        prev.GetComponent<AIControls>().target = first.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
