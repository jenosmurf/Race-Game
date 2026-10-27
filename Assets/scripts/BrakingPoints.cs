using UnityEngine;

public class BrakingPoints : MonoBehaviour
{
    
    public float brakingpower = 10f;
    private void OnTriggerEnter(Collider other)
    {
        Enemy car = other.GetComponent<Enemy>();
        if (car != null)
        {
            car.brakingZone = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy car = other.GetComponent<Enemy>();
        if (car != null)
        {
            car.brakingZone = false;
        }
    }
}
