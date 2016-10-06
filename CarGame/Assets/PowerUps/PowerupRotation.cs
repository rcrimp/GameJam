using UnityEngine;
using System.Collections;

public class PowerupRotation : MonoBehaviour {
    /// <summary>
    /// Causes the component to rotate at the given speed
    /// </summary>
    public float RotationSpeed;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0); //rotate components transform
    }
}
