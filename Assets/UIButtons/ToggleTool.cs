using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTool : MonoBehaviour {

	private Button thisButton;
	public GameState stateToChangeTo;
	public Image buttonImage;
	public Sprite inactiveSprite;
	public Sprite activeSprite;

	public GameObject tutorialBox;
	public Text tutorialText;
	private string tutorialString;

	void Start()
	{
		this.thisButton = GetComponent<Button>();
		this.thisButton.onClick.AddListener(this.UpdateGameState);
		GameManager.onGameStateUpdate -= this.StateUpdated;
		GameManager.onGameStateUpdate += this.StateUpdated;

		switch (this.stateToChangeTo)
		{
			case GameState.Thought:
				this.tutorialString = TextLiteralLists.HEARING_TUTORIAL;
				break;
			case GameState.Seduction:
				this.tutorialString = TextLiteralLists.SEDUCTION_TUTORIAL;
				break;
			case GameState.Vision:
				this.tutorialString = TextLiteralLists.VISION_TUTORIAL;
				break;
		}
	}

	void OnDestroy()
	{
		GameManager.onGameStateUpdate -= this.StateUpdated;
	}

	private void UpdateGameState()
	{
		if (GameManager.instance.state == GameState.Transitioning)
		{
			return;
		}

		if (GameManager.instance.state == GameState.Displaying && this.stateToChangeTo == GameState.Vision)
		{
			GameManager.instance.UpdateGameState(GameState.Normal);
			this.buttonImage.sprite = this.inactiveSprite;
			this.tutorialBox.SetActive(false);
		}
		else if (GameManager.instance.state != stateToChangeTo)
		{
			GameManager.instance.UpdateGameState(stateToChangeTo);
			this.buttonImage.sprite = this.activeSprite;
			this.tutorialBox.SetActive(true);
			this.tutorialText.text = this.tutorialString;
		}
		else
		{
			GameManager.instance.UpdateGameState(GameState.Normal);
			this.buttonImage.sprite = this.inactiveSprite;
			this.tutorialBox.SetActive(false);
		}
	}

	private void StateUpdated(GameState state)
	{
		if (state == GameState.Normal || state == GameState.Transitioning)
		{
			this.gameObject.SetActive(true);
			this.buttonImage.sprite = this.inactiveSprite;
			this.tutorialBox.SetActive(false);
		}
		else if (state != this.stateToChangeTo)
		{
			this.gameObject.SetActive(false);
			this.buttonImage.sprite = this.inactiveSprite;
			this.tutorialBox.SetActive(false);
		}

		if (state == GameState.Displaying && this.stateToChangeTo == GameState.Vision)
		{
			this.gameObject.SetActive(true);
			this.buttonImage.sprite = this.activeSprite;
		}
	}
}
