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
	public int visionDegredationLevel = 0;
	private float visionDegredationRatePerSecond = 0.05f;
	private float visionDowngrade1Threshold = 0.70f;
	private float visionDowngrade2Threshold = 0.30f;
	private float visionDowngrade1RotateSpeed = 0.2f;
	private float visionDowngrade2RotateSpeed = 0.5f;
	private float visionDowngrade3RotateSpeed = 0.75f;

	public delegate void VisionDowngraded(float rotateSpeed);
	public static event VisionDowngraded onVisionDowngraded;

	[Header("Speaking")]
	[Range(0.0f, 1.0f)]
	public float speakingValue = 1.0f;
	public int speakingDegredationLevel = 0;
	private float speakingDegredationRatePerSecond = 0.05f;
	private float speakingDowngrade1Threshold = 0.70f;
	private float speakingDowngrade2Threshold = 0.30f;
	private float speakingDowngrade1FadeAmount = 0.3f;
	private float speakingDowngrade2FadeAmount = 0.5f;
	private float speakingDowngrade3FadeAmount = 0.7f;
	private float speakingDowngrade1JumbleAmount = -7f;
	private float speakingDowngrade2JumbleAmount = -12f;
	private float speakingDowngrade3JumbleAmount = -15f;

	public delegate void SpeakingDowngraded(float fadeAmount, float jumbleAmount);
	public static event SpeakingDowngraded onSpeakingDowngraded;

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
					this.speakingValue -= this.speakingDegredationRatePerSecond * Time.deltaTime;
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
					TipManager.instance.tipText = TextLiteralLists.HEARING_DEGRADED_1_TIP;
					SensesManager.onHearingDowngraded(this.hearingDowngrade1FadeAmount, this.hearingDowngrade1JumbleAmount);
				}
				else if (this.hearingDegredationLevel == 1 && this.hearingValue <= this.hearingDowngrade2Threshold)
				{
					this.hearingDegredationLevel = 2;
					TipManager.instance.tipText = TextLiteralLists.HEARING_DEGRADED_2_TIP;
					SensesManager.onHearingDowngraded(this.hearingDowngrade2FadeAmount, this.hearingDowngrade2JumbleAmount);
				}
				else if (this.hearingDegredationLevel == 2 && this.hearingValue <= 0)
				{
					this.hearingDegredationLevel = 3;
					TipManager.instance.tipText = TextLiteralLists.HEARING_DEGRADED_3_TIP;
					SensesManager.onHearingDowngraded(this.hearingDowngrade3FadeAmount, this.hearingDowngrade3JumbleAmount);
				}
				break;

			case GameState.Seduction:
				if (this.speakingDegredationLevel == 0 && this.speakingValue <= this.speakingDowngrade1Threshold)
				{
					this.speakingDegredationLevel = 1;
					TipManager.instance.tipText = TextLiteralLists.SPEAKING_DEGRADED_1_TIP;
					SensesManager.onSpeakingDowngraded(this.speakingDowngrade1FadeAmount, this.speakingDowngrade1JumbleAmount);
				}
				else if (this.speakingDegredationLevel == 1 && this.speakingValue <= this.speakingDowngrade2Threshold)
				{
					this.speakingDegredationLevel = 2;
					TipManager.instance.tipText = TextLiteralLists.SPEAKING_DEGRADED_2_TIP;
					SensesManager.onSpeakingDowngraded(this.speakingDowngrade2FadeAmount, this.speakingDowngrade2JumbleAmount);
				}
				else if (this.speakingDegredationLevel == 2 && this.speakingValue <= 0)
				{
					this.speakingDegredationLevel = 3;
					TipManager.instance.tipText = TextLiteralLists.SPEAKING_DEGRADED_3_TIP;
					SensesManager.onSpeakingDowngraded(this.speakingDowngrade3FadeAmount, this.speakingDowngrade3JumbleAmount);
				}
				break;
			case GameState.Vision:
				if (this.visionDegredationLevel == 0 && this.visionValue <= this.visionDowngrade1Threshold)
				{
					this.visionDegredationLevel = 1;
					TipManager.instance.tipText = TextLiteralLists.VISION_DEGRADED_1_TIP;
					SensesManager.onVisionDowngraded(this.visionDowngrade1RotateSpeed);
				}
				else if (this.visionDegredationLevel == 1 && this.visionValue <= this.visionDowngrade2Threshold)
				{
					this.visionDegredationLevel = 2;
					TipManager.instance.tipText = TextLiteralLists.VISION_DEGRADED_2_TIP;
					SensesManager.onVisionDowngraded(this.visionDowngrade2RotateSpeed);
				}
				else if (this.visionDegredationLevel == 2 && this.visionValue <= 0)
				{
					this.visionDegredationLevel = 3;
					TipManager.instance.tipText = TextLiteralLists.VISION_DEGRADED_3_TIP;
					SensesManager.onVisionDowngraded(this.visionDowngrade3RotateSpeed);
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
