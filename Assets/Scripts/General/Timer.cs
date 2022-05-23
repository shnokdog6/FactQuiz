using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public delegate void TimerHandler();
    public event TimerHandler timeOut;

    public float timerRemaining = 60.0f;
   
    private float timeRemaining;
    private bool timerIsRunning = false;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                timeOut?.Invoke();
            }
        }
    }

    public void restart()
    {
        timeRemaining = timerRemaining;
        timerIsRunning = true;
    }

    public void start()
    {
        timerIsRunning = true;
    }
    public void stop()
    {
        timerIsRunning = false;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        textMesh.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
