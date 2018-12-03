using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeechBubbleGenerator : MonoBehaviour {

	public static SpeechBubbleGenerator instance;

	[SerializeField]
	private GameObject speechBubbleObject;
	[SerializeField]
	private TextMeshProUGUI speechBubbleText;
	[SerializeField]
	private TMP_Text fontText;
	[SerializeField]
	TMP_FontAsset normalFont;
	[SerializeField]
	TMP_FontAsset horrorFont;

	[SerializeField]
	private Image bubbleImage;
	[SerializeField]
	private Sprite speechBubbleSprite;
	[SerializeField]
	private Sprite thoughtBubbleSprite;

	private float currentFadeAmount = 0f;
	private float currentJumbleAmount = 0f;

	private float newFadeAmount = 0f;
	private float newJumbleAmount = 0f;

	int speechBubbleTextIndex = 0;

	public void Awake()
	{
		instance = this;

		this.fontText = this.speechBubbleObject.GetComponentInChildren<TMP_Text>();
	}

	public void Start()
	{
		GameManager.onGameStateUpdate += this.StateUpdated;
		SensesManager.onHearingDowngraded += this.HearingDowngraded;
	}

	void OnDestroy()
	{
		GameManager.onGameStateUpdate -= this.StateUpdated;
	}

	public void StartTalking()
	{
		StartCoroutine(this.StartSpeechBubbleGeneration());
	}

	public void StopTalking()
	{
		this.speechBubbleObject.SetActive(false);
		StopAllCoroutines();
	}

	private void HearingDowngraded(float fadeAmount, float jumbleAmount)
	{
		this.newFadeAmount = fadeAmount;
		this.newJumbleAmount = jumbleAmount;
	}

	private void StateUpdated(GameState state)
	{
		if (state == GameState.Transitioning)
		{
			StopTalking();
		}

		//Only Trigger if a downgrade happened
		if (state == GameState.Normal && this.currentFadeAmount != this.newFadeAmount)
		{
			this.currentFadeAmount = this.newFadeAmount;
			this.currentJumbleAmount = this.newJumbleAmount;
			TipManager.instance.DisplayTip();
		}
	}

	public IEnumerator StartSpeechBubbleGeneration()
	{
		if (GameManager.instance.currentCharacter == null)
		{
			Debug.LogError("SpeechBubbleGenerator.cs: Current character is null!");
		}

		while (true)
		{
			yield return new WaitForSeconds(GameManager.instance.currentCharacter.timeBetweenSpeechBubbles);

			speechBubbleObject.SetActive(true);

			this.fontText.characterSpacing = this.currentJumbleAmount;
			this.speechBubbleText.materialForRendering.SetFloat("_FaceDilate", this.currentFadeAmount);

			if (GameManager.instance.state == GameState.Thought)
			{
				this.speechBubbleText.text = GameManager.instance.currentCharacter.allThoughts[this.speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
				this.bubbleImage.sprite = this.thoughtBubbleSprite;
				this.fontText.font = this.horrorFont;
			}
			else
			{
				this.speechBubbleText.text = GameManager.instance.currentCharacter.benignTexts[this.speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
				this.bubbleImage.sprite = this.speechBubbleSprite;
				this.fontText.font = this.normalFont;
				this.fontText.characterSpacing = this.currentJumbleAmount;
				this.speechBubbleText.materialForRendering.SetFloat("_FaceDilate", this.currentFadeAmount);
			}

			//Can't do WaitForSeconds here because player will likely activate thought mode while a speech bubble is displaying
			//In which case, we have to immediately replace it with a thought
			for (float i = 0; i < GameManager.instance.currentCharacter.speechBubbleUptime; i += Time.deltaTime)
			{
				if (GameManager.instance.state == GameState.Thought)
				{
					this.speechBubbleText.text = GameManager.instance.currentCharacter.allThoughts[this.speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
					this.bubbleImage.sprite = this.thoughtBubbleSprite;
					this.fontText.font = this.horrorFont;
				}
				else
				{
					this.speechBubbleText.text = GameManager.instance.currentCharacter.benignTexts[this.speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
					this.bubbleImage.sprite = this.speechBubbleSprite;
					this.fontText.font = this.normalFont;
					this.fontText.characterSpacing = this.currentJumbleAmount;
					this.speechBubbleText.materialForRendering.SetFloat("_FaceDilate", this.currentFadeAmount);
				}

				yield return null;
			}

			speechBubbleObject.SetActive(false);
			speechBubbleTextIndex++;
		}
	}
}
