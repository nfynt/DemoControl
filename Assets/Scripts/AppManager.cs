using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour {

	public static AppManager Instance;

	public GameObject canvas;
	public GameObject errorMsg;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (this.gameObject);

		DontDestroyOnLoad (this.gameObject);
		errorMsg.SetActive (false);
	}

	/// <summary>
	/// Logged in successfully. Called from App42 success callback.
	/// </summary>
	public void LoggedInSuccess()
	{
		canvas.SetActive (false);
	}

	/// <summary>
	/// Incorrects username or password.
	/// </summary>
	public void IncorrectUsernameOrPassword()
	{
		errorMsg.SetActive (true);
	}
}
