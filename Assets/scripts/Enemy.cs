using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public carcontroller carController;
    public WaypointContainer waypointContainer;
    public BrakingPoints brakingPoints;
    public List<Transform> waypoints;
    public GameObject BrakeLight, highBeam;
    public float waypointThreshold, currentAngle, gasInput = 1f, maxAngle = 45f, maxSpeed = 120f, lateralRandomness = 2f, currentLateralOffset, still = 5f;
    public bool brakingZone;

    void Start()
    {
        carController = GetComponent<carcontroller>();
        waypoints = waypointContainer.waypoints;
        carController.currentWaypoint = 0;
        carController.lastWaypointHit = 0;
        currentLateralOffset = Random.Range(-lateralRandomness, lateralRandomness);
    }

    void Update()
    {
        if (HasReachedWaypoint(waypoints[carController.currentWaypoint]))
        {
            carController.lastWaypointHit = carController.currentWaypoint;
            carController.currentWaypoint++;
            if (carController.currentWaypoint == waypoints.Count)
            {
                carController.currentWaypoint = 0;
                carController.lapsCompleted++;
            }
            currentLateralOffset = Random.Range(-lateralRandomness, lateralRandomness);
        }

        Vector3 targetPosition = waypoints[carController.currentWaypoint].position;
        targetPosition += waypoints[carController.currentWaypoint].right * currentLateralOffset;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        currentAngle = Vector3.SignedAngle(fwd, targetPosition - transform.position, Vector3.up);
        gasInput = Mathf.Clamp01((maxAngle - Mathf.Abs(carController.speed * 0.01f * currentAngle)) / maxAngle);
        if (brakingZone)
        {
            gasInput = -gasInput * (Mathf.Clamp01(carController.speed / maxSpeed) * 2 - 1f);
        }
        Debug.DrawRay(transform.position, targetPosition - transform.position, Color.red);

        if (brakingZone)
        {
            BrakeLight.SetActive(true);
            carController.SetInput(0, currentAngle, brakingPoints.brakingpower, 0);
        }
        else
        {
            BrakeLight.SetActive(false);
            carController.SetInput(1, currentAngle, 0, 0);
        }

        if (brakingZone && carController.speed < 30f)
        {
            brakingZone = false;
        }

        if (carController.speed > maxSpeed)
        {
            carController.SetInput(0, currentAngle, 0, 0);
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

    private void OnTriggerEnter(Collider other)
    {
        BrakingPoints point = other.GetComponent<BrakingPoints>();
        if (point != null)
        {
            brakingPoints = point;
            brakingZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BrakingPoints point = other.GetComponent<BrakingPoints>();
        if (point != null)
        {
            brakingZone = false;
            brakingPoints = null;
        }
    }

}