using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public float radius = 2;
    public float force = 10;

    void Start()
    {
        Collider[] impacted = Physics.OverlapSphere(transform.position, radius);
        Debug.Log(impacted.Length);
        for (int i = 0; i < impacted.Length; i++)
        {
            Debug.Log(impacted[i].name);
            if(impacted[i].attachedRigidbody != null)
                impacted[i].attachedRigidbody.AddExplosionForce(force, transform.position, radius);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void DustCloudAnimation()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cloudPart = transform.GetChild(i).transform;
            cloudPart.rotation = Random.rotation;//randomise it's rotation,

            //scale it up to a random size,
            //Vector3 randomSize = 
                //new Vector3(Random.Range(cubeSizeWidthMin, cubesizeWidthMax), 1, 1);
            
            //cloudPart.localScale += Vector3.one * scaleRate;
            //scale it back down

        }
    }

}
