using UnityEngine;
using TMPro;
public class Round : MonoBehaviour
{
// how long a round lasts, and the two HUD texts
[SerializeField] private float roundLength = 90f;

[SerializeField] private TMP_Text timeText;
// the end screen
[SerializeField] private GameOver gameOver;

private float timeLeft; // seconds until the end
private bool over;
void Start()
{
timeLeft = roundLength; // a full clock
}
void Update()
{
    if (over) return; // ended: nothing changes any more
// count down, but never below zero
timeLeft -= Time.deltaTime;
if (timeLeft < 0f) timeLeft = 0f;
ShowHud();
if (timeLeft <= 0f) EndRound("Time's up!");
}

// time's up 2 or another script ends the round
public void EndRound(string message)
{
    if (over) return;
    over = true;
    Time.timeScale = 0f; // stop the world
    gameOver.Show(message);
}

// write both numbers on the screen
void ShowHud()
{
    
    int seconds = Mathf.CeilToInt(timeLeft);
    int minutes = seconds / 60;
    timeText.text = minutes + ":" + (seconds % 60).ToString("00");
}
}