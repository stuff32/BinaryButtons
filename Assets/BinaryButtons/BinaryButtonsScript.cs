using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.Linq;

public class BinaryButtonsScript : MonoBehaviour {

	public KMAudio Audio;
	public KMBombInfo Info;
	public KMBombModule Module;
	public TextMesh[] Texts;
	public KMSelectable[] Buttons;
	public string Response;
	bool hasChecked;
	string answer;

	private int moduleId;
	private static int moduleIdCounter;


	// Use this for initialization
	void Start () {

		moduleId = ++moduleIdCounter;

		Response = "";
		answer = Calculate();

		Debug.LogFormat("[Binary Buttons #{0}]: The answer is {1}.", moduleId, answer);

		for (int i = 0; i < Buttons.Length; i++)
		{
			int j = i;
			Buttons[i].OnInteract += () => HandleButton(j);

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	bool HandleButton(int btn)
    {
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Buttons[btn].transform);
		Buttons[btn].AddInteractionPunch();

		if (hasChecked == false)
        {
			if (Texts[btn].text == "0")
			{
				Texts[btn].text = "1";
			}
			else if (Texts[btn].text == "1")
			{
				Texts[btn].text = "0";
			}
		}


		if (btn == 5)
        {
			if (hasChecked == false)
            {
				Response += Texts[0].text;
				Response += Texts[1].text;
				Response += Texts[2].text;
				Response += Texts[3].text;
				Response += Texts[4].text;
				hasChecked = true;
			}
			if (answer == Response)
            {
				Module.HandlePass();
				Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, Buttons[btn].transform);
				Debug.LogFormat("[Binary Buttons #{0}]: Correct number was submitted!", moduleId);
			}
            else
            {
				Module.HandleStrike();
				hasChecked = false;
				Debug.LogFormat("[Binary Buttons #{0}]: Sequence {1} was entered, which was incorrect, strike!", moduleId, Response);
				Response = "";
			}
			
        }

		return false;
    }

	private string Calculate()
    {
		if (Info.IsIndicatorOn(Indicator.BOB) && Info.GetBatteryCount() > 2 && Info.IsPortPresent(Port.Parallel))
			return "00000";
		if (Info.GetBatteryCount() == 0 && Info.GetPortCount() > 3)
			return "01101";
		int numLit = Info.GetOnIndicators().Count();
		if (numLit >= 1 && Info.IsPortPresent(Port.PS2))
			return "01011";
		if (Info.GetBatteryCount() >= 2)
			return "01110";
		return "11010";
		
        

	}

	
}
