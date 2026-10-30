using UnityEngine;
using TMPro;

public class RaceTrigger : MonoBehaviour
{
    public positionCount positionCounter;
    public Player player;
    public TMP_Text lapText, finishText, timerText;

    public int totalLaps = 3;

    private float elapsedTime, delay = 3f;
    private int currentLap = 0;
    private int validLapsAtRaceStart;
    private bool raceStarted, raceFinished;

    private void Update()
    {
        if (raceFinished)
        {
            delay -= Time.unscaledDeltaTime;

            if (delay <= 0f)
            {
                Time.timeScale = 0f;
            }
        }
        else if (raceStarted)
        {
            elapsedTime += Time.deltaTime;
            currentLap = Mathf.Min(totalLaps, 1 + player.validLapsCompleted - validLapsAtRaceStart);

            int completedLapsThisRace = player.validLapsCompleted - validLapsAtRaceStart;
            if (completedLapsThisRace >= totalLaps)
            {
                currentLap = totalLaps;
                raceFinished = true;
                raceStarted = false;
                delay = 3f;
            }
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);
            timerText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }

        if (lapText != null)
        {
            lapText.text = "Lap: " + currentLap + "/" + totalLaps;
        }

        if (finishText != null)
        {
            if (raceFinished)
            {
                finishText.text = "Total Time: " + string.Format("{0:00}:{1:00}.{2:000}", Mathf.FloorToInt(elapsedTime / 60f), Mathf.FloorToInt(elapsedTime % 60f), Mathf.FloorToInt((elapsedTime * 1000f) % 1000f)) + " | Position: " + positionCounter.currentPosition;
            }
            else
            {
                finishText.text = "";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (player == null)
        {
            player = other.GetComponent<Player>();
        }

        if (player == null)
        {
            return;
        }

        if (!raceStarted)
        {
            raceStarted = true;
            currentLap = 1;
            validLapsAtRaceStart = player.validLapsCompleted;
            return;
        }
    }
}
