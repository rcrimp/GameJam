using UnityEngine;
using System.Collections;

public class ClickToExplode : MonoBehaviour {

    public GameObject explosionPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(explosionPrefab, hit.point, Quaternion.identity);
            }
        }
	}
}
