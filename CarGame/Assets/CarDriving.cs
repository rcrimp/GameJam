using UnityEngine;
using System.Collections;

public class CarDriving : MonoBehaviour {

    private float speed;
    private Vector3 direction;
    private bool upbool = false;
    public void Brake() {
        speed = 0;
    }

    public void Accelerate() {
        speed = 50;
    }

    public void SteerLeft() {
        direction = Quaternion.AngleAxis(3, Vector3.up) * direction;
    }

    public void SteerRight() {
        direction = Quaternion.AngleAxis(-3, Vector3.up) * direction;
    }
    public void up() {
        upbool = true;
    }

	// Use this for initialization
	void Start () {
        speed = 0;
        direction = new Vector3(0, 0, 1); // z forward
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos += direction * (speed * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(direction.normalized);

        if (upbool)
        {
            pos.y++;
            upbool = false;
        }
        transform.position = pos;
	}
}
