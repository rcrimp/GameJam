using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AIState
{
    protected AIControls ai;
    protected CarController car;

    public AIState(AIControls ai, CarController car)
    {
        this.ai = ai;
        this.car = car;

        Debug.Log(GetType().ToString());
    }

    /// <summary>
    /// Use Initialize for any initial logic that may cause the AI to change state
    /// </summary>
    public virtual void Initialize() { /* Nothing*/ }

    /// <summary>
    /// Use Update for any logic that the state needs to perform each frame
    /// </summary>
    public virtual void Update() {  /* Nothing */ }

    /// <summary>
    /// Use DrawGizmos for any debug drawing required by each state
    /// </summary>
    public virtual void DrawGizmos() { /* Nothing */ }

    /// <summary>
    /// Use Terminate for any logic to be executed by a state when it is ending.
    /// </summary>
    public virtual void Terminate()
    {
        ai.CancelMoveCommand();
    }
}
