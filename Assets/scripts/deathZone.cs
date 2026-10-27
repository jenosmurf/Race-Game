using UnityEngine;

public class deathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Enemy enemy = other.GetComponentInParent<Enemy>();
		if (enemy != null)
		{
			if (enemy.waypoints == null || enemy.waypoints.Count == 0)
			{
				return;
			}

			int lastWaypoint = Mathf.Clamp(enemy.carController.lastWaypointHit, 0, enemy.waypoints.Count - 1);
			Transform destination = enemy.waypoints[lastWaypoint];

			enemy.transform.SetPositionAndRotation(destination.position, destination.rotation);
			enemy.carController.currentWaypoint = lastWaypoint;

			Rigidbody rigidbody = enemy.GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				rigidbody.linearVelocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			}

			if (enemy.carController != null)
			{
				enemy.carController.SetInput(0f, 0f, 0f, 0f);
			}

			return;
		}

		Player player = other.GetComponentInParent<Player>();
		if (player == null || player.waypoints == null || player.waypoints.Count == 0)
        {
            return;
        }

		int playerLastWaypoint = Mathf.Clamp(player.carController.lastWaypointHit, 0, player.waypoints.Count - 1);
		Transform playerDestination = player.waypoints[playerLastWaypoint];

		player.transform.SetPositionAndRotation(playerDestination.position, playerDestination.rotation);
		player.carController.currentWaypoint = playerLastWaypoint;

		Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
		if (playerRigidbody != null)
		{
			playerRigidbody.linearVelocity = Vector3.zero;
			playerRigidbody.angularVelocity = Vector3.zero;
		}
	}
}
