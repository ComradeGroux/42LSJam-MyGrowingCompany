using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc_GameSession_Manager : MonoBehaviour
{
	public static sc_GameSession_Manager instance;

	public int playerDeathCount = 0;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoadEndingScene()
	{
		SceneManager.LoadScene("EndingScene"); // Replace "EndingScene" with the name or build index of your ending scene
	}
}
