using UnityEngine;
using System.Collections;

public class RandomizeColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Color randomColor = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                Debug.Log(">" + m.name + "<");
                if (m.name.Equals("carmaterial_red (Instance)"))
                {
                    m.color = randomColor;// new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
