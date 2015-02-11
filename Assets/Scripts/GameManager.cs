using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

	public BoardManager boardScript;

	private int level = 3;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent <BoardManager> ();
		InitGame ();
	}

	void InitGame ()
	{
		boardScript.SetupScene (level);
	}

}
