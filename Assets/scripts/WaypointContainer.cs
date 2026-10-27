using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointContainer : MonoBehaviour
{
    public List<Transform> waypoints;

    void Awake()
    {
        if (waypoints == null)
        {
            waypoints = new List<Transform>();
        }

        if (waypoints.Count > 0)
        {
            return;
        }

        foreach (Transform waypoint in transform)
        {
            waypoints.Add(waypoint);
        }
    }
}