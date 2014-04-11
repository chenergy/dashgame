using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEntity : MonoBehaviour, IEntity
{
	public Animator				anim;
	public PlayerCollider 		playerCollider;
	public Transform			followerTransform;
	public GameObject			playerFollower;
	public GameObject 			mesh;

	public float 				maxSpeed 		= 10.0f;
	public float 				explosionForce 	= 50.0f;
	public float 				jumpStrength 	= 1.0f;
	public float 				weight 			= 1.0f;

	private PlayerLaneTransform targetLane;
	private A_Powerup			equippedPowerup;
	private float 				startSpeed;
	private Vector3 			startOffset;

	private int 				currentLane 	= 1;
	private bool 				isJumping 		= false;
	private bool 				isDucking 		= false;
	private bool				isMagnetized	= false;

	// Use this for initialization
	void Start ()
	{
		this.startSpeed = GameController.GameSpeed;
		this.startOffset = Vector3.up;

		if (this.playerFollower != null)
			this.CreateFollower (this.playerFollower);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.targetLane != null) {
			if (!GameController.IsStopped) {
				this.transform.position = Vector3.Lerp (this.transform.position, this.targetLane.transform.position + this.startOffset, Time.deltaTime * 10.0f);
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler (new Vector3 (0, this.targetLane.transform.rotation.eulerAngles.y, 0)), Time.deltaTime * 10);
				this.mesh.transform.Rotate (new Vector3 (this.maxSpeed * Time.deltaTime * (GameController.GameSpeed / this.startSpeed), 0, 0));
			}
		}
	}

	public void StartMoving(){
		this.targetLane = LevelController.GetLaneTransform (this.currentLane);
	}

	public void MoveLeft(){
		if (!GameController.IsStopped) {
			if (this.currentLane > 0){
				this.currentLane--;
			}
			this.targetLane = LevelController.GetLaneTransform (this.currentLane);
		}
	}

	public void MoveRight(){
		if (!GameController.IsStopped) {
			if (this.currentLane < 2){
				this.currentLane++;
			}
			this.targetLane = LevelController.GetLaneTransform (this.currentLane);
		}
	}

	public void MoveDown(){
		if (!GameController.IsStopped) {
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
		if (!GameController.IsStopped) {
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
		this.playerCollider.Expand (0.1f);
	}

	/* CoRoutines */

	// Performs a jump based on the start y location of the jump. Uses the lane's x and z values;
	IEnumerator JumpRoutine(){
		Debug.Log ("StartJump");
		this.isJumping = true;

		bool isGrounded = false;
		float startY = this.mesh.transform.position.y;
		float yJump = this.jumpStrength;
		float curJump = 0.0f;
		float startWeight = this.weight;

		while (!isGrounded) {
			yJump += (Physics.gravity.y * Time.deltaTime * this.weight);
			curJump += yJump;
			this.mesh.transform.position = Vector3.Lerp (this.mesh.transform.position, 
			                                        new Vector3(this.transform.position.x, startY + this.startOffset.y + curJump, this.transform.position.z), 
			                                        Time.deltaTime * this.maxSpeed);

			isGrounded = (this.mesh.transform.position.y <= (this.playerCollider.Radius + this.transform.position.y));
			yield return new WaitForFixedUpdate();
		}

		this.weight = startWeight;
		this.isJumping = false;
		this.mesh.transform.localPosition = Vector3.zero;

		Debug.Log ("EndJump");
	}

	// Does the "SquishDown" animation that is attached to the mesh object. Shrinks the collider.
	IEnumerator DuckRoutine(){
		Debug.Log ("StartDuck");
		this.isDucking = true;

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
		
		Debug.Log ("EndDuck");
	}

	IEnumerator ExecutePowerup(){
		float timer = 0.0f;

		this.equippedPowerup.OnInit ();

		while (timer < this.equippedPowerup.Duration) {
			this.equippedPowerup.OnExecute();
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;

			Debug.Log(timer);
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
		this.playerFollower.GetComponent<PlayerFollower> ().Init (this);
		this.playerFollower.transform.parent = this.transform;
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
}

