using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_EndingScene_Manager : MonoBehaviour
{
	public Text deathCountText;
	public Text timer;

	private void Start()
	{
		deathCountText.text = "Player died: " + sc_GameSession_Manager.instance.playerDeathCount + " times.";
		//timer.text = "Your final time is: " + (Time.time - sc_GameSession_Manager.instance.startTime) + " seconds.";
		timer.text = "Your final time is: " + Time.time.ToString() + " seconds.";
	}
}
