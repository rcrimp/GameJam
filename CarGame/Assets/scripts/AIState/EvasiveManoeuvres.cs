using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EvasiveManoeuvres : AIState
{
    public EvasiveManoeuvres(AIControls ai, CarController car)
        : base(ai, car) { /* Nothing */ }

    // Try to keep a certain distance away from enemies
}
