using UnityEngine;
using System.Collections;

public class AIControls : MonoBehaviour
{
    [Tooltip("The maximum allowable angle in degrees that the AI's facing can differ from a targeted vector")]
    public float MaximumAngleDelta = 10;

    //[Tooltip("The maximum allowable distance that the AI's location can differ from a targeted vector")]
    //public float MaximumDistanceDelta = 1;

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

        // Measure how far to travel
        float distanceToTravel = Vector3.Distance(carTrans.position, point);

        // Drive forwards the required distance
        yield return StartCoroutine(DriveForwards(distanceToTravel));
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

    IEnumerator DriveForwards(float distance)
    {
        // Position at which the car begins
        Vector3 startPosition = carTrans.position;

        // How much distance has been covered?
        float coveredDistance = 0;
        while (coveredDistance > distance)
        {
            // No steering, full gas!
            car.updateInput(0, 1);

            // Wait til next frame
            yield return null;

            // Track distance covered
            coveredDistance += Vector3.Distance(startPosition, carTrans.position);
        }
    }
}
