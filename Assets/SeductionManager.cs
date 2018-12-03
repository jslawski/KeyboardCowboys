using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeductionManager : MonoBehaviour {

	public static SeductionManager instance;

	private static KeyCode[] validKeyCodes;

	private List<string> currentlyDisplayedWords;

	[SerializeField]
	private RandomObjectGenerator generator;

	private int currentCorrectCharacterIndex = 0;
	private List<string> currentContenders;

	public GameObject flirtyTextParent;

	public GameObject confessionBubble;

	private float currentFadeAmount = 0f;
	private float currentJumbleAmount = 0f;

	private float newFadeAmount = 0f;
	private float newJumbleAmount = 0f;

	// Use this for initialization
	void Start ()
	{
		//In a game jam, desparate times call for desparate measures
		SeductionManager.validKeyCodes = new KeyCode[] {
			KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
			KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
			KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
			KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
			KeyCode.Y, KeyCode.Z };
		
		SeductionManager instance = this;
		GameManager.onGameStateUpdate += this.StateUpdated;
		SensesManager.onSpeakingDowngraded += this.SpeakingDowngraded;
	}

	private void SpeakingDowngraded(float fadeAmount, float jumbleAmount)
	{
		this.newFadeAmount = fadeAmount;
		this.newJumbleAmount = jumbleAmount;
	}

	private void StateUpdated(GameState state)
	{
		//Downgrade when returning to normal state if needed
		if (state == GameState.Normal && this.currentJumbleAmount != this.newJumbleAmount)
		{
			this.currentFadeAmount = this.newFadeAmount;
			this.currentJumbleAmount = this.newJumbleAmount;
		}

		if (state == GameState.Normal && this.flirtyTextParent.transform.childCount == 0)
		{
			this.generator.numberOfObjectsToGenerate = GameManager.instance.currentCharacter.numFlirtWords;
			GetComponent<RandomObjectGenerator>().GenerateRandomObjects();
			this.SetTexts();

			this.currentlyDisplayedWords = new List<string>(GameManager.instance.currentCharacter.flirtWords);
			this.currentContenders = new List<string>(GameManager.instance.currentCharacter.flirtWords);
		}
		else if (state == GameState.Seduction)
		{
			//We already did everything we need to with seduction.  Don't let them open it again.
			if (this.confessionBubble.activeSelf == true)
			{
				GameManager.instance.UpdateGameState(GameState.Normal);
				return;
			}
			this.flirtyTextParent.SetActive(true);
			this.SetTexts();
		}
		else if (state == GameState.Transitioning)
		{
			this.generator.ClearObjects();
			this.confessionBubble.SetActive(false);
		}
		else if (state == GameState.Confessing)
		{
			StartCoroutine(this.Confess());
		}
		else
		{
			this.flirtyTextParent.SetActive(false);
		}
	}

	private IEnumerator Confess()
	{
		//Play a sound here
		yield return new WaitForSeconds(0.5f);

		this.confessionBubble.SetActive(true);

		GameManager.instance.UpdateGameState(GameState.Normal);
	}

	private void SetTexts()
	{
		for (int i = 0; i < GameManager.instance.currentCharacter.numFlirtWords; i++)
		{
			this.generator.instantiatedObjects[i].GetComponent<TextMeshProUGUI>().text = GameManager.instance.currentCharacter.flirtWords[i];

			TextMeshProUGUI[] allUguis = this.generator.instantiatedObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI current in allUguis)
			{
				current.materialForRendering.SetFloat("_FaceDilate", this.currentFadeAmount);
			}

			TMP_Text[] allTexts = this.generator.instantiatedObjects[i].GetComponentsInChildren<TMP_Text>();
			foreach (TMP_Text current in allTexts)
			{
				current.characterSpacing = this.currentJumbleAmount;
			}
		}
	}

	private void Update()
	{
		if (GameManager.instance.state != GameState.Seduction)
		{
			return;
		}

		foreach (KeyCode code in SeductionManager.validKeyCodes)
		{
			if (Input.GetKeyDown(code))
			{
				this.HandleInputForKey(code.ToString());
			}
		}
	}

	private void HandleInputForKey(string key)
	{
		char charKey = key[0];
		bool correctKeyPressed = false;

		for (int i = 0; i < GameManager.instance.currentCharacter.numFlirtWords; i++)
		{
			if (!this.currentlyDisplayedWords.Contains(GameManager.instance.currentCharacter.flirtWords[i]))
			{
				continue;
			}

			if (!this.currentContenders.Contains(GameManager.instance.currentCharacter.flirtWords[i]))
			{
				continue;
			}

			if (this.currentCorrectCharacterIndex >= GameManager.instance.currentCharacter.flirtWords[i].Length)
			{
				continue;
			}

			if (GameManager.instance.currentCharacter.flirtWords[i].ToUpper()[this.currentCorrectCharacterIndex] == charKey)
			{
				correctKeyPressed = true;
				this.generator.instantiatedObjects[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text += charKey;

				if (this.currentlyDisplayedWords.Contains(this.generator.instantiatedObjects[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text))
				{
					if (this.generator.instantiatedObjects[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text == GameManager.instance.currentCharacter.correctFlirtWord)
					{
						GameManager.instance.UpdateGameState(GameState.Confessing);
						break;
					}

					this.currentlyDisplayedWords.Remove(this.generator.instantiatedObjects[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text);
					this.generator.instantiatedObjects[i].SetActive(false);
				}
			}
			else
			{
				this.currentContenders.Remove(GameManager.instance.currentCharacter.flirtWords[i]);
				this.generator.instantiatedObjects[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = string.Empty;
			}
		}

		if (correctKeyPressed == true)
		{
			this.currentCorrectCharacterIndex++;
		}
		else
		{
			this.currentContenders = new List<string>(GameManager.instance.currentCharacter.flirtWords);

			//Clear correct values
			foreach (GameObject textObject in this.generator.instantiatedObjects)
			{
				textObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = string.Empty;
			}

			if (this.currentCorrectCharacterIndex > 0)
			{
				this.currentCorrectCharacterIndex = 0;
				this.HandleInputForKey(key);
			}
		}
	}
}
