using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

	public float levelStartDelay = 2f;
	public float turnDelay = .1f;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private int level = 1;
	private List <Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent <BoardManager> ();
		enemies = new List<Enemy> ();
		InitGame ();
	}

	void InitGame ()
	{
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);

		boardScript.SetupScene (level);
		enemies.Clear ();
	}

	void HideLevelImage ()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	void Update ()
	{
		if (playersTurn || enemiesMoving || doingSetup)
			return;

		StartCoroutine (MoveEnemies ());
	}

	void OnLevelWasLoaded(int index)
	{
		doingSetup = true;
		level++;
		InitGame();
	}

	IEnumerator MoveEnemies ()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);

		if (enemies.Count == 0)
		{
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i=0; i<enemies.Count; i++)
		{
			enemies[i].MoveEnemy ();
			yield return new WaitForSeconds (enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}

	public void AddEnemyToList (Enemy script)
	{
		enemies.Add (script);
	}


	public void GameOver ()
	{
		levelText.text = "After " + level + " days, you starved.";
		levelImage.SetActive (true);
		enabled = false;
	}


}
