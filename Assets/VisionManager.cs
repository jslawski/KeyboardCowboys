using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolarCoordinates;

public class VisionManager : MonoBehaviour {

	public static VisionManager instance;

	[SerializeField]
	private RandomObjectGenerator generator;
	[SerializeField]
	private GameObject parentObject;

	[SerializeField]
	private GameObject evidenceImage;

	int runningTotalGreenClicked = 0;

	float currentRotationSpeed = 0f;
	float newRotationSpeed = 0f;

	// Use this for initialization
	void Start () {
		VisionManager.instance = this;
		GameManager.onGameStateUpdate += this.StateUpdated;
		SensesManager.onVisionDowngraded += this.VisionDowngraded;
	}

	private void VisionDowngraded(float rotationSpeed)
	{
		this.newRotationSpeed = rotationSpeed;
	}

	private void StateUpdated(GameState state)
	{
		StopAllCoroutines();

		if (state == GameState.Normal && this.currentRotationSpeed != this.newRotationSpeed)
		{
			TipManager.instance.DisplayTip();
			this.currentRotationSpeed = this.newRotationSpeed;
		}

		if (state == GameState.Normal && this.parentObject.transform.childCount == 0)
		{
			this.evidenceImage.GetComponent<Image>().sprite = GameManager.instance.currentCharacter.evidenceImage;
			this.SetupPassword();
		}
		else if (state == GameState.Vision)
		{
			if (this.runningTotalGreenClicked == GameManager.instance.currentCharacter.totalNumGreenButtons)
			{
				GameManager.instance.UpdateGameState(GameState.Displaying);
			}
			this.parentObject.SetActive(true);
			StartCoroutine(this.ApplyRotation());
		}
		else if (state == GameState.Transitioning)
		{
			this.generator.ClearObjects();
			this.evidenceImage.SetActive(false);
			this.runningTotalGreenClicked = 0;
		}
		else if (state == GameState.Displaying)
		{
			this.evidenceImage.SetActive(true);
		}
		else
		{
			this.parentObject.SetActive(false);
			this.evidenceImage.SetActive(false);
		}
	}

	private void SetupPassword()
	{
		this.generator.numberOfObjectsToGenerate = GameManager.instance.currentCharacter.totalNumButtons;
		this.generator.GenerateRandomObjects();

		//Setup green buttons
		for (int i = 0; i < GameManager.instance.currentCharacter.totalNumGreenButtons; i++)
		{
			this.generator.instantiatedObjects[i].GetComponent<PasswordButton>().isGreen = true;
		}
	}

	public void GreenClicked(PasswordButton greenButtonClicked)
	{
		this.runningTotalGreenClicked++;
		greenButtonClicked.thisButton.interactable = false;

		if (this.runningTotalGreenClicked == GameManager.instance.currentCharacter.totalNumGreenButtons)
		{
			for (int i = 0; i < this.generator.instantiatedObjects.Count; i++)
			{
				this.generator.instantiatedObjects[i].GetComponent<PasswordButton>().thisButton.interactable = false;
			}
				StartCoroutine(RevealEvidence());
		}
	}

	public void RedClicked()
	{
		for (int i = 0; i < this.generator.instantiatedObjects.Count; i++)
		{
			this.generator.instantiatedObjects[i].GetComponent<PasswordButton>().thisButton.interactable = true;
			this.runningTotalGreenClicked = 0;
		}
	}

	private IEnumerator RevealEvidence()
	{
		yield return new WaitForSeconds(0.5f);

		GameManager.instance.UpdateGameState(GameState.Displaying);
	}

	private IEnumerator ApplyRotation()
	{
		PolarCoordinate orientationDirection = new PolarCoordinate(this.parentObject.transform.up);

		while (true)
		{
			//float newAngle = orientationDirection.angleInDegrees + (this.currentRotationSpeed * Time.deltaTime);
			RectTransform rectTransform = this.parentObject.GetComponent<RectTransform>();
			rectTransform.Rotate(0.0f, 0.0f, this.currentRotationSpeed);
			yield return null;
		}
	}
}
