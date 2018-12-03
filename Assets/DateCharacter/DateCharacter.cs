using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateCharacter : MonoBehaviour {

    public string characterName;

    public bool isSerialKiller;

    //Hidden Secrets
    public Sprite evidenceImage;
    public string evidenceThought;
    public string evidenceConfession;

    public int numBenignTexts;
	public List<string> benignTexts;

	public List<string> allThoughts;

    //Thought reading traits
    public float timeBetweenSpeechBubbles;
    public float speechBubbleUptime;

    //Vision traits
    public int totalNumButtons;
    public int totalNumGreenButtons;

    //Seduction traits
    public int numFlirtWords;
    public List<string> flirtWords;
    public string correctFlirtWord;

	[Header("Character Creator Assets")]
	[SerializeField]
	private SpriteRenderer baseBodySprite;
	[SerializeField]
	private SpriteRenderer handsSprite;
	[SerializeField]
	private SpriteRenderer shirtSprite;
	[SerializeField]
	private SpriteRenderer headpieceSprite;
	[SerializeField]
	private SpriteRenderer eyebrowsSprite;
	[SerializeField]
	private SpriteRenderer eyesSprite;
	[SerializeField]
	private SpriteRenderer noseSprite;
	[SerializeField]
	private SpriteRenderer mouthSprite;

	public void Awake()
	{
		StartCoroutine(this.MoveUpToTable());
	}

	private IEnumerator MoveUpToTable()
	{
		Debug.LogError("ORIGINAL POSITION: " + this.transform.position);

		while (Mathf.Abs(this.transform.position.x - DateCharacterManager.instance.finalPosition.x) > 0.001f)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, DateCharacterManager.instance.finalPosition, 3f * Time.deltaTime);
			yield return null;
		}

		this.transform.position = DateCharacterManager.instance.finalPosition;
		SpeechBubbleGenerator.instance.StartTalking();
		GameManager.instance.UpdateGameState(GameState.Normal);
		StartCoroutine(ScoreManager.instance.BeginDateCountdown());
	}

	public void DismissCharacter(bool saidYes)
	{
		ScoreManager.instance.runningTotalPeopleSeen++;

		if (saidYes)
		{
			ScoreManager.instance.SaidYesToSafePerson();
		}
		else if(isSerialKiller == false)
		{
			ScoreManager.instance.SaidNoToSafePerson();
		}

		ScoreManager.instance.personCounter.text = ScoreManager.instance.runningTotalPeopleSeen.ToString();

		StartCoroutine(this.DismissCharacterCoroutine(saidYes));
	}

	private IEnumerator DismissCharacterCoroutine(bool saidYes)
	{
		SpeechBubbleGenerator.instance.StopTalking();

		while (Mathf.Abs(3f - this.transform.position.x) > 1f)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, DateCharacterManager.instance.finalPosition + new Vector3(3f, 0f, 0f), 1f * Time.deltaTime);
			yield return null;
		}

		GameManager.instance.UpdateGameState(GameState.Ready);
		StopAllCoroutines();
	}

    public void SetupNewCharacter(DifficultyLevel difficultyLevel, DateCharacterType characterType)
    {
		this.SetupVisualCharacteristics();

		this.benignTexts = new List<string>();
		this.flirtWords = new List<string>();

		this.isSerialKiller = (Random.Range(0.0f, 1.0f) <= 0.50f);
        
        this.characterName = TextLiteralLists.instance.Names[Random.Range(0, TextLiteralLists.instance.Names.Count)];

		this.SetupCharacterTraits(difficultyLevel, characterType);

		//NOTE: This is poopy.  If you ever have an istance where numBenignTexts is > than the total number of unique texts, it'll deadlock
		//		I guess it's fine for a game jam though. :P
        string newBenignText = TextLiteralLists.instance.BenignText[Random.Range(0, TextLiteralLists.instance.BenignText.Count)];
		while (this.benignTexts.Count < this.numBenignTexts)
        {
			if (!this.benignTexts.Contains(newBenignText))
            {
                this.benignTexts.Add(newBenignText);
            }

			newBenignText = TextLiteralLists.instance.BenignText[Random.Range(0, TextLiteralLists.instance.BenignText.Count)];
		}
		
        string newFlirtWord = TextLiteralLists.instance.FlirtWords[Random.Range(0, TextLiteralLists.instance.FlirtWords.Count)];
        while (this.flirtWords.Count < this.numFlirtWords)
        {
            if (!this.flirtWords.Contains(newFlirtWord))
            {
                this.flirtWords.Add(newFlirtWord);
            }

			newFlirtWord = TextLiteralLists.instance.FlirtWords[Random.Range(0, TextLiteralLists.instance.FlirtWords.Count)];
		}
        this.correctFlirtWord = this.flirtWords[Random.Range(0, this.flirtWords.Count)];

		string newBenignThought = TextLiteralLists.instance.BenignThoughts[Random.Range(0, TextLiteralLists.instance.BenignThoughts.Count)];
		//numBenignTexts - 1 because we're slotting in 1 evidence thought
		while (this.allThoughts.Count < this.numBenignTexts - 1)
		{
			if (!this.allThoughts.Contains(newBenignThought))
			{
				this.allThoughts.Add(newBenignThought);
			}

			newBenignThought = TextLiteralLists.instance.BenignThoughts[Random.Range(0, TextLiteralLists.instance.BenignThoughts.Count)];
		}

        if (this.isSerialKiller == true)
        {
            this.evidenceImage = DateCharacterManager.instance.allBadEvidenceImages[Random.Range(0, DateCharacterManager.instance.allBadEvidenceImages.Length)];
            this.evidenceThought = TextLiteralLists.instance.EvidenceBadThoughts[Random.Range(0, TextLiteralLists.instance.EvidenceBadThoughts.Count)];
            this.evidenceConfession = TextLiteralLists.instance.BadConfessions[Random.Range(0, TextLiteralLists.instance.BadConfessions.Count)];
        }
        else
        {
            this.evidenceImage = DateCharacterManager.instance.allBenignEvidenceImages[Random.Range(0, DateCharacterManager.instance.allBenignEvidenceImages.Length)];
            this.evidenceThought = TextLiteralLists.instance.EvidenceBenignThoughts[Random.Range(0, TextLiteralLists.instance.EvidenceBenignThoughts.Count)];
            this.evidenceConfession = TextLiteralLists.instance.BenignConfessions[Random.Range(0, TextLiteralLists.instance.BenignConfessions.Count)];
        }

		this.allThoughts.Add(this.evidenceThought);
		//Randomize all thoughts to shuffle in the evidence thought
		for (int i = 0; i < this.allThoughts.Count; i++)
		{
			string temp = this.allThoughts[i];
			int randomIndex = Random.Range(i, this.allThoughts.Count);
			this.allThoughts[i] = this.allThoughts[randomIndex];
			this.allThoughts[randomIndex] = temp;
		}

		GameManager.instance.currentCharacter = this;
	}

	private void SetupVisualCharacteristics()
	{
		this.baseBodySprite.sprite = CharacterCreator.instance.GetBaseBody();
		this.handsSprite.sprite = CharacterCreator.instance.GetHands(this.baseBodySprite.sprite.name);
		this.shirtSprite.sprite = CharacterCreator.instance.GetShirt();
		this.headpieceSprite.sprite = CharacterCreator.instance.GetHeadpiece();
		this.eyebrowsSprite.sprite = CharacterCreator.instance.GetEyebrows();
		this.eyesSprite.sprite = CharacterCreator.instance.GetEyes();
		this.noseSprite.sprite = CharacterCreator.instance.GetNose();
		this.mouthSprite.sprite = CharacterCreator.instance.GetMouth();
	}

	private void SetupCharacterTraits(DifficultyLevel difficultyLevel, DateCharacterType characterType)
	{
		switch (difficultyLevel)
		{
			case DifficultyLevel.Easy:
				this.numBenignTexts = Random.Range(3, 4);
				this.speechBubbleUptime = Random.Range(3f, 5f);
				this.timeBetweenSpeechBubbles = this.speechBubbleUptime + Random.Range(0.5f, 1.0f);
				this.numFlirtWords = Random.Range(3, 5);
				this.totalNumButtons = Random.Range(5, 7);
				this.totalNumGreenButtons = Random.Range(2, 4);
				break;
			case DifficultyLevel.Medium:
				this.numBenignTexts = Random.Range(5, 8);
				this.speechBubbleUptime = Random.Range(3f, 5f);
				this.timeBetweenSpeechBubbles = this.speechBubbleUptime + Random.Range(1f, 3f);
				this.numFlirtWords = Random.Range(5, 8);
				this.totalNumButtons = Random.Range(7, 12);
				this.totalNumGreenButtons = Random.Range(3, 5);
				break;
			case DifficultyLevel.Hard:
				this.numBenignTexts = Random.Range(10, 15);
				this.speechBubbleUptime = Random.Range(0.5f, 1f);
				this.timeBetweenSpeechBubbles = this.speechBubbleUptime + Random.Range(0.1f, 0.5f);
				this.numFlirtWords = Random.Range(10, 15);
				this.totalNumButtons = Random.Range(13, 20);
				this.totalNumGreenButtons = Random.Range(7, 10);
				break;
			default:
				Debug.LogError("DateCharacter.cs: Unknown DifficultyLevel");
				break;
		}

		switch (characterType)
		{
			case DateCharacterType.SlowTalker:
				this.numBenignTexts = Random.Range(3, 5);
				this.speechBubbleUptime = Random.Range(7f, 12f);
				this.timeBetweenSpeechBubbles = this.speechBubbleUptime + Random.Range(3f, 5f);
				break;
			case DateCharacterType.FastTalker:
				this.numBenignTexts = Random.Range(10, 20);
				this.speechBubbleUptime = Random.Range(0.1f, 1f);
				this.timeBetweenSpeechBubbles = this.speechBubbleUptime + Random.Range(0.1f, 0.5f);
				break;
			case DateCharacterType.VisionGuarded:
				this.totalNumButtons = Random.Range(13, 20);
				this.totalNumGreenButtons = Random.Range(7, 10);
				break;
			case DateCharacterType.Unflirtatious:
				this.numFlirtWords = Random.Range(10, 20);
				break;
			default:
				break;
		}
	}

	public void SetupDevil()
	{
		this.baseBodySprite.sprite = Resources.Load<Sprite>("CharacterCreator/BaseBody/devilatyourtable.png");

		this.benignTexts = new List<string>();
		this.flirtWords = new List<string>();

		this.isSerialKiller = true;

		this.characterName = "Devil";

		this.SetupCharacterTraits(DifficultyLevel.Easy, DateCharacterType.None);

		//NOTE: This is poopy.  If you ever have an istance where numBenignTexts is > than the total number of unique texts, it'll deadlock
		//		I guess it's fine for a game jam though. :P
		string newBenignText = TextLiteralLists.instance.DevilBenignText[Random.Range(0, TextLiteralLists.instance.DevilBenignText.Count)];
		while (this.benignTexts.Count < this.numBenignTexts)
		{
			if (!this.benignTexts.Contains(newBenignText))
			{
				this.benignTexts.Add(newBenignText);
			}

			newBenignText = TextLiteralLists.instance.DevilBenignText[Random.Range(0, TextLiteralLists.instance.DevilBenignText.Count)];
		}

		string newFlirtWord = TextLiteralLists.instance.FlirtWords[Random.Range(0, TextLiteralLists.instance.FlirtWords.Count)];
		while (this.flirtWords.Count < this.numFlirtWords)
		{
			if (!this.flirtWords.Contains(newFlirtWord))
			{
				this.flirtWords.Add(newFlirtWord);
			}

			newFlirtWord = TextLiteralLists.instance.FlirtWords[Random.Range(0, TextLiteralLists.instance.FlirtWords.Count)];
		}
		this.correctFlirtWord = this.flirtWords[Random.Range(0, this.flirtWords.Count)];

		string newBenignThought = TextLiteralLists.instance.DevilBadThoughts[Random.Range(0, TextLiteralLists.instance.DevilBadThoughts.Count)];
		//numBenignTexts - 1 because we're slotting in 1 evidence thought
		while (this.allThoughts.Count < this.numBenignTexts - 1)
		{
			if (!this.allThoughts.Contains(newBenignThought))
			{
				this.allThoughts.Add(newBenignThought);
			}

			newBenignThought = TextLiteralLists.instance.DevilBadThoughts[Random.Range(0, TextLiteralLists.instance.DevilBadThoughts.Count)];
		}

		this.evidenceImage = DateCharacterManager.instance.allBadEvidenceImages[Random.Range(0, DateCharacterManager.instance.allBadEvidenceImages.Length)];
		this.evidenceThought = TextLiteralLists.instance.DevilBadThoughts[Random.Range(0, TextLiteralLists.instance.DevilBadThoughts.Count)];
		this.evidenceConfession = TextLiteralLists.DEVIL_CONFESSION;

		this.allThoughts.Add(this.evidenceThought);

		GameManager.instance.currentCharacter = this;
	}
}
