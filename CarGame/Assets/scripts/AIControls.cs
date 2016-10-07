using UnityEngine;
using System.Collections;

public enum AIMode
{
    BattleMode,         // AI will automate its behaviour: picking up and using powerups
    ClickToMove,        // AI will move to clicked position
    Avoid,              // AI will avoid other cars
    None                // AI is waiting to take commands via script
}

[RequireComponent(typeof(CarController))]
public class AIControls : MonoBehaviour
{
    [Tooltip("The maximum allowable angle in degrees that the AI's facing can differ from a targeted vector when orienting towards it")]
    public float MaximumAngleDelta = 10;

    [Tooltip("The maximum allowable distance that the AI's location can differ from a targeted vector when moving to it")]
    public float MaximumDistanceDelta = 1.5f;

    [Tooltip("The target that this ai will chase.")]
    public Transform InitialTarget;

    [Tooltip("The maximum velocity at which the ai will count as stationary when braking")]
    public float BrakeVelocity = 0.1f;

    [Tooltip("How much gas to apply when orienting towards a position")]
    public float OrientGas = 0.25f;

    [Tooltip("The distance this ai will try to maintain when avoiding another car")]
    public float SafeDistance = 10;

    [Tooltip("The angle of the frontal arc within which the ai will try to shoot enemies")]
    public float AttackArc = 10;

    [Tooltip("The beahviour the car will exhibit. Use 'None' to instead control the car via script commands")]
    public AIMode InitialMode = AIMode.None;

    private AIState state;              // The behaviour state the AI is currently in
    private Coroutine moveCommand;      // Whether the car is currently moving to a clicked location
    private CarController car;          // The car to control
    private Transform carTrans;         // The car's transform

    void Awake()
    {
        car = GetComponent<CarController>();
        carTrans = transform.GetChild(0);
    }

    // Choose behaviour based on initial mode
    void Start()
    {
        switch (InitialMode)
        {
            case AIMode.BattleMode:
                SetState(new FindPowerUp(this, car));
                break;
            case AIMode.ClickToMove:
                SetState(new ClickToMove(this, car));
                break;
            case AIMode.Avoid:
                SetState(new EvasiveManoeuvres(this, car));
                break;
            case AIMode.None:
                if (InitialTarget != null)              // So racing track test still works
                    Follow(InitialTarget);
                break;
        }
    }

    void Update()
    {
        if (state != null)
            state.Update();
    }

    void OnDrawGizmos()
    {
        if (state != null)
            state.DrawGizmos();
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
        // If currently moving to a click... stop moving to old click
        CancelMoveCommand();
        
        // Move to new click
        moveCommand = StartCoroutine(StopAtPoint(point));
    }

    /// <summary>
    /// AI car moves to each of the points in the given path. No braking occurs after visiting each point, meaning the AI will overshoot
    /// subsequent points if they are close together
    /// </summary>
    public void Follow(Vector3[] path)
    {
        CancelMoveCommand();

        moveCommand = StartCoroutine(FollowPath(path));
    }

    /// <summary>
    /// AI car follows that given transform as it moves about the world. AI car does NOT avoid obstacles when following another object.
    /// </summary>
    public void Follow(Transform target)
    {
        CancelMoveCommand();

        moveCommand = StartCoroutine(Chase(target));
    }

    /// <summary>
    /// AI car attempts to maintain a safe distance between itself and the given target
    /// </summary>
    public void Avoid(Transform target)
    {
        CancelMoveCommand();

        moveCommand = StartCoroutine(MaintainDistance(target));
    }

    /// <summary>
    /// Cancels the move command the ai is currently carrying out
    /// </summary>
    public void CancelMoveCommand()
    {
        if (moveCommand != null)
            StopAllCoroutines();

        moveCommand = null;
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

    // Calculates which direction to accelerate in when moving towards the given position. Should be used in conjunction
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
        if (Vector3.Distance(carTrans.position, point) > MaximumDistanceDelta)
        {
            // Orient towards point
            yield return StartCoroutine(TurnToFace(point));

            // Drive forwards the required distance
            yield return StartCoroutine(DriveTowards(point));
        }
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
            car.updateInput(angleDirection, OrientGas);

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

    IEnumerator MaintainDistance(Transform target)
    {
        while (true)
        {
            Vector3 safeDirection = (carTrans.position - target.position).normalized;
            Vector3 safePoint = target.position + (safeDirection * SafeDistance);

            // Calculate the amount of steering and gas to apply.
            float steeringInput = CalculateSteeringTowards(safePoint);
            float gasInput = CalculateGasTowards(safePoint);

            // Update car input with calculated values
            car.updateInput(steeringInput, gasInput);

            // Wait til next frame
            yield return null;
        }
    }
}
