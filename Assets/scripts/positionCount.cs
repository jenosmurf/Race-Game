using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class positionCount : MonoBehaviour
{
    public List<Transform> Cars = new List<Transform>();
    public int currentPosition;
    public TMP_Text positionText;

    private void Start()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            AddCar(enemy.transform);
        }

        foreach (Player player in FindObjectsOfType<Player>())
        {
            AddCar(player.transform);
        }
    }

    private void Update()
    {
        Cars.RemoveAll(car => car == null);
        Cars.Sort((first, second) =>
        {
            int lapOrder = GetLapsCompleted(second).CompareTo(GetLapsCompleted(first));
            return lapOrder != 0
                ? lapOrder
                : GetCurrentWaypoint(second).CompareTo(GetCurrentWaypoint(first));
        });

        currentPosition = 0;
        for (int i = 0; i < Cars.Count; i++)
        {
            if (Cars[i].GetComponent<Player>() != null)
            {
                currentPosition = i + 1;
                break;
            }
        }

        if (positionText != null)
        {
            positionText.text = "Position: " + currentPosition;
        }
    }

    private static int GetCurrentWaypoint(Transform car)
    {
        Enemy enemy = car.GetComponent<Enemy>();
        if (enemy != null)
        {
            return enemy.carController.currentWaypoint;
        }

        Player player = car.GetComponent<Player>();
        if (player != null)
        {
            return player.carController.currentWaypoint;
        }
        return 0;
    }

    private static int GetLapsCompleted(Transform car)
    {
        Enemy enemy = car.GetComponent<Enemy>();
        if (enemy != null)
        {
            return enemy.carController.lapsCompleted;
        }

        Player player = car.GetComponent<Player>();
        if (player != null)
        {
            return player.carController.lapsCompleted;
        }

        return 0;
    }

    private void AddCar(Transform car)
    {
        if (car != null && !Cars.Contains(car))
        {
            Cars.Add(car);
        }
    }

}