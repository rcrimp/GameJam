using UnityEngine;
using System.Collections;

public class TyrePlacement : MonoBehaviour {
    public WheelCollider wheelCollider;

    private float wheelAngle = 0;
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

            if (wheelCollider.GetComponent<ParticleSystem>() != null)
            {
                ParticleSystem.EmissionModule em = wheelCollider.GetComponent<ParticleSystem>().emission;

                em.enabled = true;
                if (
                    //Mathf.Abs(hit.forwardSlip) >= wheelCollider.forwardFriction.extremumSlip ||
                        Mathf.Abs(hit.sidewaysSlip) >= wheelCollider.sidewaysFriction.extremumSlip
                    )
                {
                    em.enabled = true;
                }
                else
                {
                    em.enabled = false;
                }
            }
        } else {
            // no contact with ground, just extend wheel position with suspension distance
            localPosition = Vector3.Lerp(localPosition, -Vector3.up * collider.suspensionDistance, .05f);
        }
        wheelTransform.localPosition = localPosition;
        wheelAngle += (((collider.rpm * 360f) / 60f) * Time.fixedDeltaTime) % 360f;
        wheelTransform.localRotation = Quaternion.Euler(wheelAngle, collider.steerAngle, 0);

    }
}
