using UnityEngine;
using System.Collections;

public class Player : MovingObject
{
	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;

	private Animator animator;
	private int food;

	protected override void Start ()
	{
		animator = GetComponent <Animator> ();
		food = GameManager.Instance.playerFoodPoints;

		base.Start ();
	}

	void Update ()
	{
		if (GameManager.Instance.playersTurn == false) return;

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0)
			vertical = 0;

		if (horizontal != 0 || vertical != 0)
		{
			AttemptMove <Wall> (horizontal, vertical);
		}
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
		food--;

		base.AttemptMove <T> (xDir, yDir);

		RaycastHit2D hit;

		CheckIfGameOver ();

		GameManager.Instance.playersTurn = false;
	}

	protected override void OnCantMove<T> (T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("plaerChop");
	}

	void Restart ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	void OnDisable ()
	{
		GameManager.Instance.playerFoodPoints = food;
	}

	void CheckIfGameOver ()
	{
		if (food <= 0)
			GameManager.Instance.GameOver ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Exit"))
		{
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}
		else if (other.CompareTag ("Food"))
		{
			food += pointsPerFood;
			other.gameObject.SetActive (false);
		}
		else if (other.CompareTag ("Soda"))
		{
			food += pointsPerSoda;
			other.gameObject.SetActive (false);
		}
	}

	public void LoseFood (int loss)
	{
		animator.SetTrigger ("playerHit");
		food -= loss;
		CheckIfGameOver ();
	}
}
