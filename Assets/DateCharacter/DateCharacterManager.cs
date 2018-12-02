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

		GameObject newCharacter = Instantiate(this.genericCharacterObject, Vector3.zero, new Quaternion()) as GameObject;
		DateCharacter characterComponent = newCharacter.GetComponent<DateCharacter>();

		characterComponent.SetupNewCharacter(DifficultyLevel.Easy, DateCharacterType.None);
    }
}
