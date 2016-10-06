using UnityEngine;
using System.Collections;

public class AIControls : MonoBehaviour
{
    [Tooltip("The maximum allowable angle in degrees that the AI's facing can differ from a targeted vector when orienting towards it")]
    public float MaximumAngleDelta = 10;

    [Tooltip("The maximum allowable distance that the AI's location can differ from a targeted vector when moving to it")]
    public float MaximumDistanceDelta = 1;

    public Transform target;

    private CarController car;
    private Transform carTrans;
    private Vector3 clickLocation;
    
    private Coroutine MoveCommand;

    void Awake()
    {
        car = GetComponent<CarController>();
        carTrans = transform.GetChild(0);
    }

    void Start()
    {
        if (target != null)
            Follow(target);
    }

    void Update()
    {
        // Only follow clicks if there is no target
        if (target == null && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                GoTo(hitInfo.point);
                clickLocation = hitInfo.point;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (carTrans != null)
        {
            Vector3 from = carTrans.position;
            from.y += 0.5f;

            // Draw line from car to target OR clickLocation
            Vector3 to = target == null ? clickLocation : target.position;
            to.y += 0.5f;

            Gizmos.DrawLine(from, to);
        }
    }

    public void GoTo(Vector3 point)
    {
        if (MoveCommand != null)
            StopCoroutine(MoveCommand);

        MoveCommand = StartCoroutine(GoToPoint(point));
    }

    public void Follow(Vector3[] path)
    {
        StartCoroutine(FollowPath(path));
    }

    public void Follow(Transform target)
    {
        if (MoveCommand != null)
        {
            StopCoroutine(MoveCommand);
            MoveCommand = null;
        }
        MoveCommand = StartCoroutine(Chase(target));
        //StartCoroutine(Chase(target));
    }

    private float SteeringTowards(Vector3 position)
    {
        // Determine which way to turn
        Vector3 desiredDirection = position - carTrans.position;

        // How different are we from desired angle
        float angleFromDesiredDirection = Vector3.Angle(carTrans.forward, desiredDirection);
        if (angleFromDesiredDirection < MaximumAngleDelta)
            return 0;
        else
            return MathExtension.AngleDir(carTrans.forward, desiredDirection, Vector3.up);
    }

    private float GasTowards(Vector3 position)
    {
        // Determine which way to turn
        Vector3 desiredDirection = position - carTrans.position;

        // How close are we to the given facing?
        float angleFromDesiredDirection = Vector3.Angle(carTrans.forward, desiredDirection);

        return angleFromDesiredDirection > 90 ? -1 : 1;
    }

    IEnumerator FollowPath(Vector3[] path)
    {
        // Go to each point in path
        foreach (Vector3 point in path)
            yield return StartCoroutine(GoToPoint(point));
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

            // Orient towards, full gas!
            car.updateInput(angleDirection, 1);

            // Wait til next frame
            yield return null;

            // Track distance covered
            distanceToTarget = Vector3.Distance(carTrans.position, position);
        }

        // Stop applying gas (car will continue to drift)
        car.updateInput(0, 0);
    }

    IEnumerator Chase(Transform target)
    {
        while (true)
        {
            float steeringInput = SteeringTowards(target.position);
            float gasInput = GasTowards(target.position);

            print("SteeringInput: " + steeringInput + ", GasInput: " + gasInput);

            car.updateInput(steeringInput, gasInput);

            yield return null;
        }
    }
}
