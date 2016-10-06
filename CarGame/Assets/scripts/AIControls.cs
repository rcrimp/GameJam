using UnityEngine;
using System.Collections;

public class AIControls : MonoBehaviour
{
    [Tooltip("The maximum allowable angle in degrees that the AI's facing can differ from a targeted vector when orienting towards it")]
    public float MaximumAngleDelta = 10;

    [Tooltip("The maximum allowable distance that the AI's location can differ from a targeted vector when moving to it")]
    public float MaximumDistanceDelta = 1;

    private CarController car;
    private Transform carTrans;
    private Vector3 target;

    void Awake()
    {
        car = GetComponentInChildren<CarController>();
        carTrans = transform.GetChild(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                GoTo(hitInfo.point);
                target = hitInfo.point;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (carTrans != null)
        {
            Vector3 from = carTrans.position;
            from.y += 0.5f;

            Vector3 to = target;
            to.y += 0.5f;

            Gizmos.DrawLine(from, to);
        }
    }

    public void GoTo(Vector3 point)
    {
        StartCoroutine(GoToPoint(point));
    }

    public void Follow(Vector3[] path)
    {
        // foreach point in path
            // Orient towards
            // Move towards
    }

    IEnumerator GoToPoint(Vector3 point)
    {
        // Orient towards point
        yield return StartCoroutine(TurnToFace(point));

        // Drive forwards the required distance
        yield return StartCoroutine(DriveTowards(point));
    }

    IEnumerator TurnToFace(Vector3 point)
    {
        // Determine which way to turn
        Vector3 desiredDirection = point - carTrans.position;
        float angleDirection = MathExtension.AngleDir(carTrans.forward, desiredDirection, Vector3.up);

        // How close are we to the given facing?
        float angleFromDesiredDirection = Vector3.Angle(carTrans.forward, desiredDirection);
        while (angleFromDesiredDirection > MaximumAngleDelta)
        {
            // Full steering, minimal gas
            car.updateInput(angleDirection, 0.1f);

            // Wait til next frame
            yield return null;

            // Track how close we are to desired angle
            angleFromDesiredDirection = Vector3.Angle(carTrans.forward, desiredDirection);
        }
    }

    IEnumerator DriveTowards(Vector3 position)
    {
        float distanceToTarget = Vector3.Distance(carTrans.position, position);

        // How much distance has been covered?
        while (distanceToTarget > MaximumDistanceDelta)
        {
            Vector3 desiredDirection = position - carTrans.position;
            float angleDirection = MathExtension.AngleDir(carTrans.forward, desiredDirection, Vector3.up);
            print(angleDirection);

            // Orient towards, full gas!
            car.updateInput(angleDirection, 1);

            // Wait til next frame
            yield return null;

            // Track distance covered
            distanceToTarget = Vector3.Distance(carTrans.position, position);
        }

        // Stop applying gas
        car.updateInput(0, 0);
    }
}
