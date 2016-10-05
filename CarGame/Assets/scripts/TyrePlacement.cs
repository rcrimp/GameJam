using UnityEngine;
using System.Collections;

public class TyrePlacement : MonoBehaviour {
    public WheelCollider wheelCollider;

    void FixedUpdate() {
        UpdateWheelHeight(this.transform, wheelCollider);
    }

    void UpdateWheelHeight(Transform wheelTransform, WheelCollider collider) {
        Vector3 localPosition = wheelTransform.localPosition;
        WheelHit hit = new WheelHit();
        // see if we have contact with ground
        if (collider.GetGroundHit(out hit)) {
            float hitY = collider.transform.InverseTransformPoint(hit.point).y;
            localPosition.y = hitY + collider.radius;

            //ParticleSystem.EmissionModule em = wheelCollider.GetComponent<ParticleSystem>().emission;
            //em.enabled = true;
            /*if (
                    Mathf.Abs(hit.forwardSlip) >= wheelCollider.forwardFriction.extremumSlip ||
                    Mathf.Abs(hit.sidewaysSlip) >= wheelCollider.sidewaysFriction.extremumSlip
                )
            {
                wheelCollider.GetComponent<ParticleSystem>().enableEmission = true;
            }
            else
            {
                wheelCollider.GetComponent<ParticleSystem>().enableEmission = false;
            }*/
        } else {
            // no contact with ground, just extend wheel position with suspension distance
            localPosition = Vector3.Lerp(localPosition, -Vector3.up * collider.suspensionDistance, .05f);
        }
        wheelTransform.localPosition = localPosition;
        wheelTransform.localRotation = Quaternion.Euler(0, collider.steerAngle, 0);

    }
}
