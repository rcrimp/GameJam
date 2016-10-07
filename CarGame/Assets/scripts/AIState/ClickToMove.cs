using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClickToMove : AIState
{
    private Vector3[] path;

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
                Vector3 clickLocation = hitInfo.point;

                // Calculate a path
                NavMeshPath navMeshPath = new NavMeshPath();
                NavMesh.CalculatePath(car.transform.position, clickLocation, NavMesh.AllAreas, navMeshPath);

                // Save path (so can draw gizmos)
                path = navMeshPath.corners;
                ai.Follow(path);        // Tell ai to follow path
            }
        }
    }

    // Draws lines between each point in path
    public override void DrawGizmos()
    {
        base.DrawGizmos();

        // If there is a path, draw it
        if (path != null)
        {
            // Connect the dots...
            for (int i = 0; i < path.Length - 1; i++)
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}

