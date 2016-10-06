using UnityEngine;
using System.Collections;

public class BurgerCarBehaviour : MonoBehaviour
{

    public float Speed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.left * Speed * Time.deltaTime;
        transform.position += movement;
        Debug.Log(Speed);
    }
}