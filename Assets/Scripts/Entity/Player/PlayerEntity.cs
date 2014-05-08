using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEntity : MonoBehaviour, IEntity
{
	public Animator				anim;
	public PlayerCollider 		playerCollider;
	public Transform			followerTransform;
	public GameObject			playerFollower;
	public GameObject 			gobj;

	public float 				maxSpeed 		= 10.0f;
	public float 				explosionForce 	= 50.0f;
	public float 				jumpStrength 	= 1.0f;
	public float 				weight 			= 1.0f;
	public int					expansionLimit	= 50;

	private PlayerLaneTransform targetLane;
	private A_Powerup			equippedPowerup;
	private float 				startSpeed;
	private Vector3 			startOffset;

	private int 				currentLane 	= 1;
	private int					mass 			= 0;
	private bool 				isJumping 		= false;
	private bool 				isDucking 		= false;
	private bool				isMagnetized	= false;
	private HeightLevel 		heightLevel		= HeightLevel.MIDDLE;

	// Use this for initialization
	void Start ()
	{
		this.startSpeed = LevelController.GameSpeed;
		this.startOffset = Vector3.up;

		if (this.playerFollower != null) {
			this.CreateFollower (this.playerFollower);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.targetLane != null) {
			if (!LevelController.IsStopped) {
				/*if ((this.transform.localPosition - Vector3.up).sqrMagnitude > 0.01f){
					this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, Vector3.up, Time.deltaTime * 10);
				}*/
				this.transform.localPosition = Vector3.up;


				/*
				this.transform.position = Vector3.Lerp (this.transform.position, this.targetLane.transform.position + new Vector3 (0, this.playerCollider.Radius, 0), Time.deltaTime * 10.0f);
				if (Quaternion.Angle(this.transform.rotation, this.targetLane.transform.rotation) > 0.5f){
					this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.Euler (new Vector3 (0, this.targetLane.transform.rotation.eulerAngles.y, 0)), Time.deltaTime * 10);
				}
				*/

				this.gobj.transform.Rotate (new Vector3 (this.maxSpeed * Time.deltaTime * (LevelController.GameSpeed / this.startSpeed), 0, 0));
			}
		}
	}

	public void StartMoving(){
		this.targetLane = LevelController.GetLaneTransform (this.currentLane);
		this.transform.parent = this.targetLane.transform;
	}

	public void MoveLeft(){
		if (!LevelController.IsStopped) {
			if (this.currentLane > 0){
				this.currentLane--;
			}
			this.targetLane = LevelController.GetLaneTransform (this.currentLane);
			this.transform.parent = this.targetLane.transform;
		}
	}

	public void MoveRight(){
		if (!LevelController.IsStopped) {
			if (this.currentLane < 2){
				this.currentLane++;
			}
			this.targetLane = LevelController.GetLaneTransform (this.currentLane);
			this.transform.parent = this.targetLane.transform;
		}
	}

	public void MoveDown(){
		if (!LevelController.IsStopped) {
			if (this.isJumping) {
				this.FallFast();
			}else{
				if (!this.isDucking) {
					StartCoroutine (this.DuckRoutine ());
				}
			}
		}
	}

	public void MoveUp(){
		if (!LevelController.IsStopped) {
			if (!this.isJumping) {
				if (this.isDucking){
					this.isDucking = false;
					this.anim.SetBool ("isDucking", false);
					StopCoroutine("DuckRoutine");
				}
				StartCoroutine (this.JumpRoutine ());
			}
		}
	}

	public void Die(){
		foreach (Transform t in this.GetComponentsInChildren<Transform>()) {
			if (t.tag == "Collectable"){
				t.parent = null;
				Collider c = (t.GetComponent<BoxCollider>() == null) ? t.gameObject.AddComponent<BoxCollider>() : t.collider;
				Rigidbody r = (t.GetComponent<Rigidbody>() == null) ? t.gameObject.AddComponent<Rigidbody>() : t.rigidbody;
				r.isKinematic = false;
				r.useGravity = true;
				r.AddForce(this.explosionForce * (r.transform.position - this.transform.position).normalized + Vector3.up);
				r.collider.isTrigger = false;
			}
		}
	}

	public void Expand(){
		// Create the transition section
		LevelController.GenerateTransitionSection ();

		this.playerCollider.Expand (2.0f);
	}

	/* CoRoutines */

	// Performs a jump based on the start y location of the jump. Uses the lane's x and z values;
	IEnumerator JumpRoutine(){
		this.isJumping = true;

		bool 	isGrounded 		= false;
		float 	startY 			= this.gobj.transform.position.y;
		float 	yJump 			= this.jumpStrength;
		float 	curJump 		= 0.0f;
		float 	startWeight 	= this.weight;

		int 	bufferFrames 	= 3;
		int 	curFrame 		= 0;

		while (!isGrounded) {
			yJump += (Physics.gravity.y * Time.deltaTime * this.weight);
			curJump += yJump;
			this.gobj.transform.position = Vector3.Lerp (this.gobj.transform.position, 
			                                        new Vector3(this.transform.position.x, startY + this.startOffset.y + curJump, this.transform.position.z), 
			                                        Time.deltaTime * this.maxSpeed);

			if (curFrame > bufferFrames)
				isGrounded = (this.gobj.transform.position.y <= (this.playerCollider.Radius + this.transform.position.y));

			yield return new WaitForFixedUpdate();
			curFrame++;
		}

		this.weight = startWeight;
		this.isJumping = false;
		this.gobj.transform.localPosition = Vector3.zero;
	}

	// Does the "SquishDown" animation that is attached to the mesh object. Shrinks the collider.
	IEnumerator DuckRoutine(){
		this.isDucking = true;
		this.playerCollider.ColliderRadius = 0.5f;
		this.anim.SetBool ("isDucking", true);

		while (true) {
			if (this.anim.GetCurrentAnimatorStateInfo (0).IsName ("SquishDown")) {
				if (this.anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.95f) {
					break;
				}
			}
			yield return new WaitForEndOfFrame ();
		}

		this.anim.SetBool ("isDucking", false);
		this.isDucking = false;
		this.playerCollider.ColliderRadius = 0.75f;
	}

	IEnumerator ExecutePowerup(){
		float timer = 0.0f;

		this.equippedPowerup.OnInit ();

		while (timer < this.equippedPowerup.Duration) {
			this.equippedPowerup.OnExecute();
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
		}

		this.equippedPowerup.OnComplete ();
		this.equippedPowerup = null;
	}

	public void FallFast(){
		this.weight *= 10;
	}

	public void CreateFollower(GameObject prefab){
		PlayerFollower follower = prefab.GetComponent<PlayerFollower> ();
		this.playerFollower = GameObject.Instantiate (prefab, this.followerTransform.position, this.followerTransform.rotation) as GameObject;
		this.playerFollower.GetComponent<PlayerFollower> ().Init (this.followerTransform);
	}

	public void AddPowerup(A_Powerup powerup){
		if (this.equippedPowerup != null) {
			StopCoroutine("ExecutePowerup");
			this.equippedPowerup.OnComplete();
		}

		this.equippedPowerup = null;
		this.equippedPowerup = powerup;
		StartCoroutine ("ExecutePowerup");
	}

	public bool IsMagnetized{
		get { return this.isMagnetized; }
		set { this.isMagnetized = value; }
	}

	public bool IsJumping{
		get { return this.isJumping; }
	}

	public void AddMass( int mass ){
		this.mass += mass;

		if (this.mass % this.expansionLimit == 0) {
			this.Expand ();


			if (this.playerCollider.NumExpansions == 1){
				LevelController.CreateBoss();
			} else {
				GlobalCameraController.AddToOffset (new Vector3 (0, 2, -4));
			}
		}
	}

	public int Mass{
		get { return this.mass; }
	}

	public HeightLevel HeightLevel{
		get { return this.heightLevel; }
		set { this.heightLevel = value; }
	}

	public int CurrentLane{
		get { return this.currentLane; }
	}
}

