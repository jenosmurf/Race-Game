using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public carcontroller carController;
    public GameObject BrakeLight, highBeam;
    bool highBeamOn = false;
    public float maxSpeed = 120f, waypointThreshold = 5f, still = 5f;
    public List<Transform> waypoints;
    public WaypointContainer waypointContainer;
    public int validLapsCompleted;
    private readonly HashSet<int> visitedWaypointsThisLap = new HashSet<int>();
    

    void Start()
    {
        
        carController = GetComponent<carcontroller>();
        waypoints = waypointContainer.waypoints;
        carController.currentWaypoint = 0;
        carController.lastWaypointHit = 0;
    }

    void Update()
    {

        float brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
        float steeringInput = carController.maxTurnAngle * Input.GetAxis("Horizontal");
        carController.SetInput(Input.GetAxis("Vertical"), steeringInput, brakeInput, 0f);

        if (BrakeLight != null)
        {
            BrakeLight.SetActive(brakeInput > 0f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            highBeamOn = !highBeamOn;
            highBeam.SetActive(highBeamOn);
        }
        if (carController.speed > maxSpeed)
        {
            carController.SetInput(0, steeringInput, brakeInput, 0f);
        }


        if (HasReachedWaypoint(waypoints[carController.currentWaypoint]))
        {
            carController.lastWaypointHit = carController.currentWaypoint;
            visitedWaypointsThisLap.Add(carController.currentWaypoint);
            carController.currentWaypoint++;
            if (carController.currentWaypoint == waypoints.Count)
            {
                carController.currentWaypoint = 0;
                int requiredCheckpoints = Mathf.CeilToInt(waypoints.Count * 0.8f);
                if (visitedWaypointsThisLap.Count >= requiredCheckpoints)
                {
                    validLapsCompleted++;
                }

                visitedWaypointsThisLap.Clear();
                carController.lapsCompleted++;
            }
            
        }

         if (carController.speed <= 10f)
        {
            still -= Time.deltaTime;
            if (still <= 0f)
            {
                int lastWaypoint = Mathf.Clamp(carController.lastWaypointHit, 0, waypoints.Count - 1);
			    Transform destination = waypoints[lastWaypoint];
                transform.SetPositionAndRotation(destination.position, destination.rotation);
                carController.currentWaypoint = lastWaypoint;
                carController.SetInput(0f, 0f, 0f, 0f);
                still = 5f;
            }
        }
    }

    private bool HasReachedWaypoint(Transform waypoint)
    {
        Debug.DrawRay(waypoint.position, waypoint.right * waypointThreshold, Color.green);
        Debug.DrawRay(waypoint.position, -waypoint.right * waypointThreshold, Color.red);

        Vector3 offset = transform.position - waypoint.position;
        float distancePastWaypoint = Vector3.Dot(offset, waypoint.forward);
        float distanceFromCheckpointSide = Mathf.Abs(Vector3.Dot(offset, waypoint.right));
        return distancePastWaypoint >= 0f && distanceFromCheckpointSide <= waypointThreshold;
    }

    public int GetVisitedWaypointCount()
    {
        return visitedWaypointsThisLap.Count;
    }

    public void ResetVisitedWaypoints()
    {
        visitedWaypointsThisLap.Clear();
    }
}
