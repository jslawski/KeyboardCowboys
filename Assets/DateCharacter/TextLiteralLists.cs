using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLiteralLists : MonoBehaviour {

	public static TextLiteralLists instance;

	public List<string> Names;
    public List<string> BenignText;
    public List<string> BenignThoughts;
    public List<string> BenignConfessions;
    public List<string> BadThoughts;
    public List<string> BadConfessions;
    public List<string> FlirtWords;
    
	public void Awake() 
	{
		TextLiteralLists.instance = this;
	}

}
