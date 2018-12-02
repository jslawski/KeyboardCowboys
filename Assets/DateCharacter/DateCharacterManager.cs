using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DateCharacterType {Easy, Medium, Hard, FastTalker, SlowTalker, VisionGuarded, Unflirtatious, None };

public class DateCharacterManager : MonoBehaviour
{
    public static DateCharacterManager instance;

    public Texture[] allBenignEvidenceImages;
    public Texture[] allBadEvidenceImages;

	public GameObject genericCharacterObject;

    public void Start()
    {
        DateCharacterManager.instance = this;
        this.allBenignEvidenceImages = Resources.LoadAll<Texture>("EvidenceImage/Benign");
        this.allBadEvidenceImages = Resources.LoadAll<Texture>("EvidenceImage/Bad");
		GameManager.onGameStateUpdate += this.StateUpdated;

		this.MakeNewCharacter();
    }

	private void MakeNewCharacter()
	{
		GameObject newCharacter = Instantiate(this.genericCharacterObject, new Vector3(15f, 0f, 0f), new Quaternion()) as GameObject;
		DateCharacter characterComponent = newCharacter.GetComponent<DateCharacter>();

		characterComponent.SetupNewCharacter(DifficultyLevel.Easy, DateCharacterType.None);
		GameManager.instance.UpdateGameState(GameState.Transitioning);
	}


	public void StateUpdated(GameState state)
	{
		if (state == GameState.Ready)
		{
			Destroy(GameManager.instance.currentCharacter.gameObject);
			this.MakeNewCharacter();
		}
	}

	public void DismissCurrentCharacter(bool verdict)
	{
		if (GameManager.instance.currentCharacter.isSerialKiller == true && verdict == true)
		{
			GameManager.instance.UpdateGameState(GameState.GameOver);
			return;
		}

		GameManager.instance.currentCharacter.DismissCharacter(verdict);
		GameManager.instance.UpdateGameState(GameState.Transitioning);
	}
}
