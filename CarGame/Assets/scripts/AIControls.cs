using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarController))]
public class AIControls : MonoBehaviour
{
    [Tooltip("The maximum allowable angle in degrees that the AI's facing can differ from a targeted vector when orienting towards it")]
    public float MaximumAngleDelta = 10;

    [Tooltip("The maximum allowable distance that the AI's location can differ from a targeted vector when moving to it")]
    public float MaximumDistanceDelta = 1;

    [Tooltip("The target that this ai will chase. If none is specified, the ai will move to a clicked location")]
    public Transform target;

    [Tooltip("The maximum velocity at which the ai will count as stationary when braking")]
    public float BrakeVelocity = 0.1f;

    private AIState state;              // The behaviour state the AI is currently in
    private Vector3 clickLocation;      // ClickLocation (for Gizmo drawing)
    private Coroutine moveCommand;      // Whether the car is currently moving to a clicked location
    private CarController car;          // The car to control
    private Transform carTrans;         // The car's transform

    void Awake()
    {
        car = GetComponent<CarController>();
        carTrans = transform.GetChild(0);
    }

    // Starts following target
    void Start()
    {
        // Follow target if there is one
        if (target != null)
            Follow(target);
    }

    // Moves to click location
    void Update()
    {
        // Only follow clicks if there is no target
        if (target == null && Input.GetMouseButtonDown(0))
        {
            // Get mouse click ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Find where the terrain was clicked
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Go to location
                GoTo(hitInfo.point);
                clickLocation = hitInfo.point;  // Remeber clicked location (for Gizmo drawing)
            }
        }
    }

    // Draws line from car to target point
    void OnDrawGizmos()
    {
        // If game is in play
        if (carTrans != null)
        {
            // Draw line from car to target OR clickLocation
            Vector3 to = target == null ? clickLocation : target.position;
            Gizmos.DrawLine(carTrans.position, to);
        }
    }

    /// <summary>
    /// Changes the AI's state to the given behaviour state
    /// </summary>
    public void SetState(AIState state)
    {
        // Notify old state that it is being terminated
        if (this.state != null)
            this.state.Terminate();

        // Overwrite old state with new state
        this.state = state;

        // Initialize the new state
        this.state.Initialize();
    }

    /// <summary>
    /// AI car will orient towards and then drive in the direction of the given point. The AI will begin braking ONCE THE 
    /// POINT HAS BEEN REACHED, meaning it will overshoot the point by some amount.
    /// </summary>
    public void GoTo(Vector3 point)
    {
        // If currently moving to a click...
        if (moveCommand != null)
            StopCoroutine(moveCommand); // Stop moving to old click

        moveCommand = StartCoroutine(StopAtPoint(point));
    }

    /// <summary>
    /// AI car moves to each of the points in the given path. No braking occurs after visiting each point, meaning the AI will overshoot
    /// subsequent points if they are close together
    /// </summary>
    public void Follow(Vector3[] path)
    {
        moveCommand = StartCoroutine(FollowPath(path));
    }

    /// <summary>
    /// AI car follows that given transform as it moves about the world. AI car does NOT avoid obstacles when following another object.
    /// </summary>
    public void Follow(Transform target)
    {
        moveCommand = StartCoroutine(Chase(target));
    }

    // Calculates how much to steer in a particular direction when orienting towards the given point
    private float CalculateSteeringTowards(Vector3 position)
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

    // Calculates which direction to accelerate in when moving towards the given position. SHould be used in conjunction
    // with CalculateSteeringTowards
    private float CalculateGasTowards(Vector3 position)
    {
        // Determine which way to turn
        Vector3 desiredDirection = position - carTrans.position;

        // How close are we to the given facing?
        float angleFromDesiredDirection = Vector3.Angle(carTrans.forward, desiredDirection);

        return angleFromDesiredDirection > 90 ? -1 : 1;
    }

    //Calculates how much reverse direction gas to apply when trying to come to a stop
    private float CalculateGasBrake()
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        Vector3 localVel = transform.InverseTransformDirection(velocity);

        float gasInput = 0;
        if (localVel.z > BrakeVelocity)
            gasInput = -1;
        else if (localVel.z < -BrakeVelocity)
            gasInput = 1;

        return gasInput;
    }

    // Orients towards then moves to each point in path
    IEnumerator FollowPath(Vector3[] path)
    {
        // Go to each point in path
        foreach (Vector3 point in path)
            yield return StartCoroutine(GoToPoint(point));
    }

    // Orients towards then moves to the given point
    IEnumerator GoToPoint(Vector3 point)
    {
        // Orient towards point
        yield return StartCoroutine(TurnToFace(point));

        // Drive forwards the required distance
        yield return StartCoroutine(DriveTowards(point));        
    }

    // Orients towards, moves to the given point, then comes to a stop
    IEnumerator StopAtPoint(Vector3 point)
    {
        // Drive to point
        yield return StartCoroutine(GoToPoint(point));

        // Stop when arrived
        yield return StartCoroutine(Stay());
    }

    // Orients towards the given point
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

    // Drives towards the given point whilst orienting towards it
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

    // Tries to keep velocity at 0
    IEnumerator Stay()
    {
        while(true)
        {
            // Calculate which direction to acclerate in (the opposite direction to which the car is moving)
            float gasInput = CalculateGasBrake();

            // Apply gas in calculated direction
            car.updateInput(0, gasInput);

            // Wait til next frame
            yield return null;
        }
    }

    // Drives towards whilst orienting towards the given transform
    IEnumerator Chase(Transform target)
    {
        while (true)
        {
            // Calculate the amount of steering and gas to apply.
            float steeringInput = CalculateSteeringTowards(target.position);
            float gasInput = CalculateGasTowards(target.position);

            // Update car input with calculated values
            car.updateInput(steeringInput, gasInput);

            // Wait til next frame
            yield return null;
        }
    }
}
