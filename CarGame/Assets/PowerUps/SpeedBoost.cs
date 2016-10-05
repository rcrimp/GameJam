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
        Debug.Log("THERE'S A COLLISION");
        //increase the speed of the thing that collides with me

        //saves a ref to the behaviour script of the collider. if there isn't one, value is null
        BurgerCarBehaviour collidingObject = collider.gameObject.GetComponent<BurgerCarBehaviour>();

        //if there's a behaviourScript
        if (collidingObject != null)
        {
            Debug.Log("THERE'S A BEHAVIOUR SCRIPT");
            originalSpeed = collidingObject.Speed;
            StartCoroutine(Boost(collidingObject));
        }
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
