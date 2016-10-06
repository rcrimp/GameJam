using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{

    public float boostDuration;
    public float speedIncrease;
    public float slowDownDuration;
    private float originalSpeed;   

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject collidingObject = collider.gameObject;
        ConstantForce force = collidingObject.AddComponent<ConstantForce>();
        force.relativeForce.Set(0, 0, -1000f);
    }

    public IEnumerator Boost(BurgerCarBehaviour collidingObject)
    {
        collidingObject.Speed *= speedIncrease;
        Debug.Log("BOOST");
        yield return new WaitForSeconds(boostDuration);
        StartCoroutine(SlowDown(collidingObject));
    }

    public IEnumerator SlowDown(BurgerCarBehaviour collidingObject)
    {
        float expiredDuration = 0;
        while(expiredDuration < slowDownDuration)
        {
            expiredDuration += Time.deltaTime;
            collidingObject.Speed = Mathf.Lerp(collidingObject.Speed, originalSpeed, expiredDuration/slowDownDuration);
            yield return null;
        }
        collidingObject.Speed = originalSpeed;
    }
}
