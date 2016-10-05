using UnityEngine;
using System.Collections;

namespace CarGame
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Pathfinder : MonoBehaviour
    {
        public event PathfinderEventHandler PathFound;
    
        /// <summary>
        /// The navMeshAgent component that this pathfinder uses to find paths
        /// </summary>
        public NavMeshAgent NavMeshAgent { get; private set; }

        void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            // NavMeshAgent is used only to FIND paths, not to traverse them (so it's turned off)
            NavMeshAgent.enabled = false;
        }

        /// <summary>
        /// Finds a path for this AI to the given location. Finding a path may take several frames. Subscribe to 
        /// the PathFound event in order to be notified when the path has been found.
        /// </summary>
        public void FindPath(Vector3 target)
        {
            StartCoroutine(CalculatePath(target));
        }

        /// <summary>
        /// Coroutine for finding a path. Used as a coroutine because a NavMeshAgent may take multiple frames to 
        /// calculate a path.
        /// </summary>
        IEnumerator CalculatePath(Vector3 target)
        {
            // Beging calculating a path
            NavMeshAgent.SetDestination(target);

            // While the path calculation is pending
            while (NavMeshAgent.pathPending)
                yield return null;      // Wait til next frame

            // Path has been calculated, save to path variable
            Vector3[] path = NavMeshAgent.path.corners;

            // Notify listeners that a path has been found
            if (PathFound != null)
                PathFound(this, new PathfinderEventArgs(path));
        }
    }

    /// <summary>
    /// Delegate for pathfinding related events
    /// </summary>
    public delegate void PathfinderEventHandler(object sender, PathfinderEventArgs e);

    public class PathfinderEventArgs
    {
        // The corner vectors of the path found by the pathfinding system
        public Vector3[] Path { get; set; }

        public PathfinderEventArgs(Vector3[] path)
        {
            Path = path;
        }
    }
}