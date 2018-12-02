using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyLevel { Easy, Medium, Hard, None }

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public DateCharacter currentCharacter;

	void Awake()
	{
		GameManager.instance = this;
	}

}
