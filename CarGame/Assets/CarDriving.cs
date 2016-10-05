using UnityEngine;
using System.Collections;

public class CarDriving : MonoBehaviour
{
    public float RotateSpeed = 50;
    public float MaxSpeed = 5;
    public float Speed;
    public float AccelerationRate = 1f;

    public Coroutine Turning { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TurnLeft(Vector3 direction)
    {
        // Stop turning if it is happening
        if (Turning != null)
            StopCoroutine(Turning);

        Turning = StartCoroutine(Rotate(direction, -1, 0.5f));
    }

    public void TurnRight(Vector3 direction)
    {
        // Stop turning if it is happening
        if (Turning != null)
            StopCoroutine(Turning);

        Turning = StartCoroutine(Rotate(direction, 1, 0.5f));
    }

    public void Accelerate()
    {
        StartCoroutine(IncreaseSpeed());
    }

    public void Brake()
    {

    }

    IEnumerator IncreaseSpeed()
    {
        while (Speed < MaxSpeed)
        {
            Speed += AccelerationRate * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Rotate(Vector3 direction, int rotationDirection, float maximumDelta)
    {
        Vector3 _direction = (target - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(_direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotateSpeed);

        float angle = Vector3.Angle(transform.forward, direction);
        while (angle > maximumDelta)
        {
            transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime * rotationDirection);

            yield return null;

            angle = Vector3.Angle(transform.forward, direction);
        }

        Turning = null;
    }
}
