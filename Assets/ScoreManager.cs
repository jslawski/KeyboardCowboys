using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

	public static ScoreManager instance;

	public int minutesPerGame = 3;
	public int secondsPerGame = 0;
	public int minutesPerDate = 0;
	public int secondsPerDate = 30;

	public int score = 0;
	public int runningTotalPeopleSeen = 0;

	public int pointsPerCorrectGuess = 50;
	public int pointsPerIncorrectGuess = 25;

	public int minutesLeftInGame = 3;
	public int secondsLeftInCurrentMinuteGame = 60;
	public int minutesLeftInDate = 0;
	public int secondsLeftInCurrentMinuteDate = 30;

	[SerializeField]
	public TextMeshProUGUI gameTimer;
	[SerializeField]
	private TextMeshProUGUI clockTimer;
	[SerializeField]
	public TextMeshProUGUI personCounter;
	[SerializeField]
	public TextMeshProUGUI scoreText;

	// Use this for initialization
	void Awake ()
	{
		ScoreManager.instance = this;
		StartCoroutine(this.BeginGameCountdown());

	}

	public void SaidYesToSafePerson()
	{
		this.score += this.pointsPerCorrectGuess;
		scoreText.text = "Score: " + this.score.ToString();
	}

	public void SaidNoToSafePerson()
	{
		this.score -= this.pointsPerIncorrectGuess;
		scoreText.text = "Score: " + this.score.ToString();
	}

	public IEnumerator BeginGameCountdown()
	{
		this.SetGameTimerText();

		while (this.minutesLeftInGame > 0 || this.secondsLeftInCurrentMinuteGame > 0)
		{
			yield return new WaitForSeconds(1);

			if (this.secondsLeftInCurrentMinuteGame <= 0)
			{
				this.minutesLeftInGame--;
				this.secondsLeftInCurrentMinuteGame = 60;
			}

			this.secondsLeftInCurrentMinuteGame--;

			this.SetGameTimerText();
		}

		GameManager.instance.UpdateGameState(GameState.GameOver);
		Application.LoadLevel("YouSurvivedScene");
	}

	public IEnumerator BeginDateCountdown()
	{
		this.minutesLeftInDate = this.minutesPerDate;
		this.secondsLeftInCurrentMinuteDate = this.secondsPerDate;

		this.SetDateTimerText();

		while (this.minutesLeftInDate > 0 || this.secondsLeftInCurrentMinuteDate > 0)
		{
			yield return new WaitForSeconds(1);

			if (this.secondsLeftInCurrentMinuteDate <= 0)
			{
				this.minutesLeftInDate--;
				this.secondsLeftInCurrentMinuteDate = 60;
			}

			this.secondsLeftInCurrentMinuteDate--;

			this.SetDateTimerText();
		}

		DateCharacterManager.instance.DismissCurrentCharacter(false);
		this.minutesLeftInDate = this.minutesPerDate;
		this.secondsLeftInCurrentMinuteDate = this.secondsPerDate;
	}

	void SetDateTimerText()
	{
		string timerString = string.Empty;

		if (this.minutesLeftInDate < 10)
		{
			timerString += "0";
		}

		timerString += this.minutesLeftInDate + ":";

		if (this.secondsLeftInCurrentMinuteDate < 10)
		{
			timerString += "0";
		}

		timerString += this.secondsLeftInCurrentMinuteDate;

		this.clockTimer.text = timerString;
	}

	void SetGameTimerText()
	{
		string timerString = string.Empty;

		if (this.minutesLeftInGame < 10)
		{
			timerString += "0";
		}

		timerString += this.minutesLeftInGame + ":";

		if (this.secondsLeftInCurrentMinuteGame < 10)
		{
			timerString += "0";
		}

		timerString += this.secondsLeftInCurrentMinuteGame;

		this.gameTimer.text = timerString;
	}
}
