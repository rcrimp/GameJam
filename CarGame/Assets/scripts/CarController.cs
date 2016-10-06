using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour {
    private float inputVer = 0;
    private float inputHor = 0;

    public void updateInput(float steering, float gas) {
        inputVer = gas;
        inputHor = steering;
    }

    public float idealRPM = 500f;
    public float maxRPM = 1000f;

    public Transform centerOfGravity;

    public WheelCollider wheelFR;
    public WheelCollider wheelFL;
    public WheelCollider wheelRR;
    public WheelCollider wheelRL;

    public float turnRadius = 10f;
    public float torque = 250000f;
    public float brakeTorque = 100f;

    public float AntiRoll = 20000.0f;

    public enum DriveMode { Front, Rear, All };
    public DriveMode driveMode = DriveMode.Rear;

    void Start() {
        GetComponent<Rigidbody>().centerOfMass = centerOfGravity.localPosition;
    }

    void FixedUpdate() {
        Debug.Log ("Speed: " + (wheelRR.radius * Mathf.PI * wheelRR.rpm * 60f / 1000f) + "km/h    RPM: " + wheelRL.rpm);

        
        float scaledTorque = inputVer * torque;

        //if (wheelRL.rpm < idealRPM)
        //    scaledTorque = Mathf.Lerp(scaledTorque / 2f, scaledTorque, wheelRL.rpm / idealRPM);
        //else
        //    scaledTorque = Mathf.Lerp(scaledTorque, 0, (wheelRL.rpm - idealRPM) / (maxRPM - idealRPM));

        DoRollBar(wheelFR, wheelFL);
        DoRollBar(wheelRR, wheelRL);

        wheelFR.steerAngle = inputHor * turnRadius;
        wheelFL.steerAngle = inputHor * turnRadius;

        wheelFR.motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
        wheelFL.motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
        wheelRR.motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;
        wheelRL.motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;
        GetComponent<Rigidbody>().AddForce(0,-100,0);
    }


    void DoRollBar(WheelCollider WheelL, WheelCollider WheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        bool groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                                         WheelL.transform.position);
        if (groundedR)
            GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * antiRollForce,
                                         WheelR.transform.position);
    }
}
