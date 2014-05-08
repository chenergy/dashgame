using UnityEngine;
using System.Collections;

public abstract class A_Boss : MonoBehaviour, IEntity
{
	public int 				lives = 3;
	public GameObject 		bossObject;

	protected Animator 		anim;
	protected PlayerEntity	player;
	protected A_BTNode 		ai;
	protected Transform		baseTransform;

	// Use this for initialization
	void Start ()
	{
		this.anim = this.GetComponent<Animator> ();
		this.player = LevelController.ActivePlayer;

		// Parent to the lane
		//this.transform.parent = this.player.transform;
		this.transform.parent = LevelController.LanePositionTransform;
		this.transform.localPosition = Vector3.zero;
		this.transform.localRotation = Quaternion.identity;

		this.InitAI ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.ai.Execute ();
		this.ai.Reset ();

		if (Input.GetMouseButtonDown (0)) {
			//this.OnHit();
			Debug.Log(this.lives);
		}
	}

	public void Die(){
		Debug.Break ();
	}

	protected void OnHit(){
		this.lives--;
		if (lives <= 0) {
			this.Die();
		}
	}

	public int Lives{
		get { return this.lives; }
	}

	protected abstract void InitAI();
}

