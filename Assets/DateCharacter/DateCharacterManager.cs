using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DateCharacterType {Easy, Medium, Hard, FastTalker, SlowTalker, VisionGuarded, Unflirtatious, VisionSparse, None };

public class DateCharacterManager : MonoBehaviour
{
    public static DateCharacterManager instance;

	public Vector3 finalPosition = new Vector3(-0.083f, 1.56f, 3.972f);

    public Sprite[] allBenignEvidenceImages;
    public Sprite[] allBadEvidenceImages;

	public GameObject genericCharacterObject;

    public void Start()
    {
        DateCharacterManager.instance = this;
        this.allBenignEvidenceImages = Resources.LoadAll<Sprite>("EvidenceImage/Benign");
        this.allBadEvidenceImages = Resources.LoadAll<Sprite>("EvidenceImage/Bad");
		GameManager.onGameStateUpdate += this.StateUpdated;

		this.MakeNewCharacter();
    }

	void OnDestroy()
	{
		GameManager.onGameStateUpdate -= this.StateUpdated;
	}

	private void MakeNewCharacter()
	{
		GameObject newCharacter = Instantiate(this.genericCharacterObject, this.finalPosition + new Vector3(-15f, 0f, 0f), new Quaternion()) as GameObject;
		DateCharacter characterComponent = newCharacter.GetComponent<DateCharacter>();

		//Roll for devil spawn
		float devilChance = Random.Range(0.0f, 1.0f);
		if (devilChance <= 0.05f)
		{
			characterComponent.SetupDevil();
		}
		else
		{
			if (ScoreManager.instance.runningTotalPeopleSeen < 2)
			{
				characterComponent.SetupNewCharacter(DifficultyLevel.Easy, DateCharacterType.None);
			}
			else if (ScoreManager.instance.runningTotalPeopleSeen < 5)
			{
				characterComponent.SetupNewCharacter(DifficultyLevel.Medium, DateCharacterType.None);
			}
			else
			{
				DifficultyLevel level = (DifficultyLevel)Random.Range(0, (int)DifficultyLevel.None);
				DateCharacterType dateType = (DateCharacterType)Random.Range(0, (int)DateCharacterType.None);

				characterComponent.SetupNewCharacter(level, dateType);
			}
		}

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
			Application.LoadLevel("GameOverScene");
			return;
		}

		GameManager.instance.currentCharacter.DismissCharacter(verdict);
		GameManager.instance.UpdateGameState(GameState.Transitioning);
	}
}
