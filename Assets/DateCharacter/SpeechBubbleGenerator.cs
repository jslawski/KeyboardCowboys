using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleGenerator : MonoBehaviour {

	public static SpeechBubbleGenerator instance;

	[SerializeField]
	private GameObject speechBubbleObject;
	[SerializeField]
	Text speechBubbleText;

	public void Awake()
	{
		instance = this;
	}

	public void StartTalking()
	{
		StartCoroutine(this.StartSpeechBubbleGeneration());
	}

	public void StopTalking()
	{
		StopAllCoroutines();
	}

	public IEnumerator StartSpeechBubbleGeneration()
	{
		if (GameManager.instance.currentCharacter == null)
		{
			Debug.LogError("SpeechBubbleGenerator.cs: Current character is null!");
		}

		int speechBubbleTextIndex = 0;

		while (true)
		{
			yield return new WaitForSeconds(GameManager.instance.currentCharacter.timeBetweenSpeechBubbles);

			speechBubbleObject.SetActive(true);

			if (GameManager.instance.state == GameState.Thought)
			{
				speechBubbleText.text = GameManager.instance.currentCharacter.allThoughts[speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
			}
			else
			{
				speechBubbleText.text = GameManager.instance.currentCharacter.benignTexts[speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
			}

			//Can't do WaitForSeconds here because player will likely activate thought mode while a speech bubble is displaying
			//In which case, we have to immediately replace it with a thought
			for (float i = 0; i < GameManager.instance.currentCharacter.speechBubbleUptime; i += Time.deltaTime)
			{
				if (GameManager.instance.state == GameState.Thought)
				{
					speechBubbleText.text = GameManager.instance.currentCharacter.allThoughts[speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
				}
				else
				{
					speechBubbleText.text = GameManager.instance.currentCharacter.benignTexts[speechBubbleTextIndex % GameManager.instance.currentCharacter.numBenignTexts];
				}

				yield return null;
			}

			speechBubbleObject.SetActive(false);
			speechBubbleTextIndex++;
		}
	}
}
