using UnityEngine;
using System.Collections;

public class BecomeABurgerForABit : MonoBehaviour {

    public GameObject burgerPrefab;
    public float timeBurgeredInSeconds = 2;
    GameObject burgerCar;

    void Start()
    {
        transform.Find("WheelRL").gameObject.SetActive(false);
        transform.Find("WheelRR").gameObject.SetActive(false);
        transform.Find("WheelFL").gameObject.SetActive(false);
        transform.Find("WheelFR").gameObject.SetActive(false);
        transform.Find("CarMesh").gameObject.SetActive(false);
       
        StartCoroutine("BeABurger");
    }

    IEnumerator BeABurger()
    {
        burgerCar = Instantiate(burgerPrefab, transform.position, Quaternion.identity, transform) as GameObject;
        Debug.Log("u done been burgered");
        yield return new WaitForSeconds(timeBurgeredInSeconds);
        StopBeingABurger();
    }

    public void StopBeingABurger()
    {
        Destroy(burgerCar);
        transform.Find("WheelRL").gameObject.SetActive(true);
        transform.Find("WheelRR").gameObject.SetActive(true);
        transform.Find("WheelFL").gameObject.SetActive(true);
        transform.Find("WheelFR").gameObject.SetActive(true);
        transform.Find("CarMesh").gameObject.SetActive(true);
        Destroy(this);
    }
}
