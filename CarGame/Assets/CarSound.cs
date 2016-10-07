using UnityEngine;
using System.Collections;

public class CarSound : MonoBehaviour {

    AudioSource audio;
    Vector3 lastPosition = Vector3.zero;
    public float speed;

    int startingPitch = 2;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        audio.pitch = startingPitch;
	}
	
	// Update is called once per frame
	void Update () {

        speed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;

        audio.pitch = speed;
	}
}
