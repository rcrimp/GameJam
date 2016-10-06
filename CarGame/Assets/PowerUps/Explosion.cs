using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{

    public GameObject dustcloudPrefab;

    void OnCollisionEnter(Collision collision)
    {
        //gets a contact position of the collision
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DustCloudAnimation()
    {
        for (int i = 0; i < dustcloudPrefab.transform.childCount; i++)
        {
            Transform cloudPart = dustcloudPrefab.transform.GetChild(i).transform;
            cloudPart.rotation = Random.rotation;//randomise it's rotation,

            //scale it up to a random size,
            Vector3 randomSize = 
                //new Vector3(Random.Range(cubeSizeWidthMin, cubesizeWidthMax), 1, 1);
            
            cloudPart.localScale += Vector3.one * scaleRate;
            //scale it back down

        }
    }

    public void LerpScale(float durationInSeconds, Transform transformToScale)
    {
        var originalScale = transformToScale.localScale;
        var targetScale = originalScale + Vector3(1.0f, 0.0f, 1.0f);
        var originalTime = durationInSeconds;

        while (durationInSeconds > 0.0f)
        {
            durationInSeconds -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, durationInSeconds / originalTime);
            yield;

        }
    }
}
