using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimeHandler : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Image TimerFill;
    public int duration;
    private int RemainingDuration;

    private void Start()
    {
        Begin(duration);
    }

    private void Begin(int Second)
    {
        RemainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (RemainingDuration >= 0)
        {
            string secondsString = (RemainingDuration % 60).ToString("00");
            timeText.text = secondsString;
            TimerFill.fillAmount = Mathf.InverseLerp(0, duration, RemainingDuration);
            RemainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        print("Time is up");
        // Restart the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetTimer()
    {
        RemainingDuration = duration;
    }
}
