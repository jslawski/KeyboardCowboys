using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerdictButton : MonoBehaviour {

	[SerializeField]
	private bool verdict;
	private Button thisButton;

	void Awake() {
		this.thisButton = GetComponent<Button>();
		this.thisButton.onClick.AddListener(this.SendVerdict);
	}

	private void SendVerdict()
	{
		DateCharacterManager.instance.DismissCurrentCharacter(this.verdict);
	}
}
