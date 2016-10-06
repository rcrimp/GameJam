using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClickToMove : AIState
{
    // TODO: find a path to location

    private Vector3 clickLocation;

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
            {
                clickLocation = hitInfo.point;
                ai.GoTo(hitInfo.point);     // Go to location
            }
        }
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        Gizmos.DrawLine(car.transform.position, clickLocation);
    }
}

