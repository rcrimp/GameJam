using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClickToMove : AIState
{
    public ClickToMove(AIControls ai, CarController car)
        : base(ai, car) { /* Nothing */ }

    public override void Update()
    {
        base.Update();

        // Only follow clicks if there is no target
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse click ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Find where the terrain was clicked
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
                ai.GoTo(hitInfo.point);     // Go to location
        }
    }
}

