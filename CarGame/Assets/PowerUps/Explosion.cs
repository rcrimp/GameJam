using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public float radius = 2;
    public float force = 10;
    public int durationInSeconds = 1;

    void Start()
    {
        Collider[] impacted = Physics.OverlapSphere(transform.position, radius);
        Debug.Log(impacted.Length);

        //for each thing in the explosion radius, explode it (if it has a rigidbody)
        for (int i = 0; i < impacted.Length; i++)
        {
            Debug.Log(impacted[i].name);
            if(impacted[i].attachedRigidbody != null)
                impacted[i].attachedRigidbody.AddExplosionForce(force, transform.position, radius);
        }

        //set random rotation for each child in the dustcloud prefab
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cloudPart = transform.GetChild(i).transform;
            cloudPart.rotation = Random.rotation;
        }

            StartCoroutine("DustCloudAnimation");
    }

    public IEnumerator DustCloudAnimation()
    {
        float elapsedDuration = 0;
        while(elapsedDuration < durationInSeconds)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform cloudPart = transform.GetChild(i).transform;
                cloudPart.localScale *= 1.04f;//make it grow

                //fade
                float t = elapsedDuration / durationInSeconds;
                float alpha = Mathf.Lerp(0.2f, 0, t);

                var material = transform.GetChild(i).GetComponent<Renderer>().material;
                var color = material.color;
                material.color = new Color(color.r, color.g, color.b, alpha);

            }
            elapsedDuration += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

}
