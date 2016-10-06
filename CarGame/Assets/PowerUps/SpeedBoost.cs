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
        Debug.Log(collider.name);
        Debug.Log(Vector3.forward * speedIncrease);       
        collider.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speedIncrease);
        Destroy(this.gameObject);
    }

    //public IEnumerator Boost(BurgerCarBehaviour collidingObject)
    //{
    //    collidingObject.Speed *= speedIncrease;
    //    yield return new WaitForSeconds(boostDuration);
    //    StartCoroutine(SlowDown(collidingObject));
    //}

    //public IEnumerator SlowDown(BurgerCarBehaviour collidingObject)
    //{
    //    float expiredDuration = 0;
    //    while(expiredDuration < slowDownDuration)
    //    {
    //        expiredDuration += Time.deltaTime;
    //        collidingObject.Speed = Mathf.Lerp(collidingObject.Speed, originalSpeed, expiredDuration/slowDownDuration);
    //        yield return null;
    //    }
    //    collidingObject.Speed = originalSpeed;
    //}
}
