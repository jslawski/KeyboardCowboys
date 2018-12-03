using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLiteralLists : MonoBehaviour {

	public static TextLiteralLists instance;

	public List<string> Names;
    public List<string> BenignText;
    public List<string> BenignThoughts;
	public List<string> BenignSecrets;
    public List<string> BadSecrets;
    public List<string> FlirtWords;

	public List<string> DevilBenignText;
	public List<string> DevilBadThoughts;

	public const string VISION_DEGRADED_1_TIP = "Looks like you're vision's taking a beating from that power...What a shame.";
	public const string VISION_DEGRADED_2_TIP = "Keep it up and you'll be completely blind!  Now that would be funny...";
	public const string VISION_DEGRADED_3_TIP = "Good luck seeing ANYTHING now, much less your suitor's true nature...";

	public const string HEARING_DEGRADED_1_TIP = "Is there a ringing in your ears, or did they just crack a bit from overusing that power?";
	public const string HEARING_DEGRADED_2_TIP = "Hello?  Can you still hear me?  Listen too much and you won't be able to very soon...";
	public const string HEARING_DEGRADED_3_TIP = "Enjoy it, my voice is going to be the last thing you'll ever actually hear for the rest of your life.";

	public const string SPEAKING_DEGRADED_1_TIP = "These powers come at a price. And in the case of that power, I went for the classic 'steal your voice' price.";
	public const string SPEAKING_DEGRADED_2_TIP = "What's that?  Having trouble communicating? Well you did say you wanted the authentic dating experience.";
	public const string SPEAKING_DEGRADED_3_TIP = "Better start learnin' sign language, girl, cuz you're as mute as a silent movie.";

	public const string DEVIL_CONFESSION = "I may not look it, but I'm actually the devil, reaper of souls and sower of chaos. I'm a nice guy, though";

	public const string VISION_TUTORIAL = "Click to reveal all hidden secrets";
	public const string HEARING_TUTORIAL = "Wait to hear hidden thoughts";
	public const string SEDUCTION_TUTORIAL = "Type to magically flirt out a confession";

	public void Awake() 
	{
		TextLiteralLists.instance = this;
	}

}
