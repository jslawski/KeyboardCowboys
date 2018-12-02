using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesManager : MonoBehaviour {

	public static SensesManager instance;

	[Header("Hearing")]
	[Range(0.0f, 1.0f)]
	public float hearingValue = 1.0f;
	public int hearingDegredationLevel = 0;
	private float hearingDegredationRatePerSecond = 0.05f;
	private float hearingDowngrade1Threshold = 0.70f;
	private float hearingDowngrade2Threshold = 0.30f;
	private float hearingDowngrade1FadeAmount = 0.20f;
	private float hearingDowngrade2FadeAmount = 0.35f;
	private float hearingDowngrade3FadeAmount = 0.5f;
	private float hearingDowngrade1JumbleAmount = -5f;
	private float hearingDowngrade2JumbleAmount = -10f;
	private float hearingDowngrade3JumbleAmount = -15f;

	public delegate void HearingDowngraded(float fadeAmount, float jumbleAmount);
	public static event HearingDowngraded onHearingDowngraded;

	[Header("Vision")]
	[Range(0.0f, 1.0f)]
	public float visionValue = 1.0f;
	private float visionDegredationRatePerSecond = 0.05f;
	private float visionDowngrade1Threshold = 0.70f;
	private float visionDowngrade2Threshold = 0.30f;

	[Header("Speaking")]
	[Range(0.0f, 1.0f)]
	public float speakingMeter = 1.0f;
	private float speakingDegredationRatePerSecond = 0.05f;
	private float speakingDowngrade1Threshold = 0.70f;
	private float speakingDowngrade2Threshold = 0.30f;

	void Awake()
	{
		SensesManager.instance = this;

		GameManager.onGameStateUpdate += this.StateUpdated;
	}

	private void StateUpdated(GameState state)
	{
		StopAllCoroutines();
		if (state == GameState.Thought || state == GameState.Vision || state == GameState.Seduction)
		{
			StartCoroutine(DrainSense());
		}
	}

	private IEnumerator DrainSense()
	{
		while (true)
		{
			switch (GameManager.instance.state)
			{
				case GameState.Thought:
					this.hearingValue -= this.hearingDegredationRatePerSecond * Time.deltaTime;
					break;
				case GameState.Vision:
					this.visionValue -= this.visionDegredationRatePerSecond * Time.deltaTime;
					break;
				case GameState.Seduction:
					this.speakingMeter -= this.speakingDegredationRatePerSecond * Time.deltaTime;
					break;
			}

			this.CheckForDowngrades();

			yield return null;
		}
	}

	private void CheckForDowngrades()
	{
		switch (GameManager.instance.state)
		{
			case GameState.Thought:
				if (this.hearingDegredationLevel == 0 && this.hearingValue <= this.hearingDowngrade1Threshold)
				{
					this.hearingDegredationLevel = 1;
					SensesManager.onHearingDowngraded(this.hearingDowngrade1FadeAmount, this.hearingDowngrade1JumbleAmount);
				}
				else if (this.hearingDegredationLevel == 1 && this.hearingValue <= this.hearingDowngrade2Threshold)
				{
					this.hearingDegredationLevel = 2;
					SensesManager.onHearingDowngraded(this.hearingDowngrade2FadeAmount, this.hearingDowngrade2JumbleAmount);
				}
				else if (this.hearingDegredationLevel == 2 && this.hearingValue <= 0)
				{
					this.hearingDegredationLevel = 3;
					SensesManager.onHearingDowngraded(this.hearingDowngrade3FadeAmount, this.hearingDowngrade3JumbleAmount);
				}
				break;

		}
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
