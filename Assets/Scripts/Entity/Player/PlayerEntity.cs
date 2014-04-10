using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEntity : MonoBehaviour, IEntity
{
	public PlayerCollider 		playerCollider;
	public PlayerFollower		playerFollower;
	public GameObject 			mesh;
	public float 				maxSpeed 		= 10.0f;
	public float 				explosionForce 	= 50.0f;
	public float 				jumpStrength 	= 1.0f;
	public float 				weight 			= 1.0f;

	private int 				currentLane 	= 1;
	private PlayerLaneTransform targetLane;
	private float 				startSpeed;
	private bool 				isJumping 		= false;
	private bool 				isDucking 		= false;
	private Vector3 			startOffset;

	// Use this for initialization
	void Start ()
	{
		this.startSpeed = GameController.GameSpeed;
		this.startOffset = Vector3.up;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.targetLane != null) {
			if (!GameController.IsStopped) {
				this.transform.position = Vector3.Lerp (this.transform.position, this.targetLane.transform.position + this.startOffset, Time.deltaTime * 10.0f);
				this.transform.rotation = Quaternion.Euler (new Vector3 (0, this.targetLane.transform.rotation.eulerAngles.y, 0));
				this.mesh.transform.Rotate (new Vector3 (this.maxSpeed * Time.deltaTime * (GameController.GameSpeed / this.startSpeed), 0, 0));
			}
		}
	}

	public void StartMoving(){
		this.targetLane = LevelController.GetLaneTransform (this.currentLane);
		//this.SetNextPath (LevelController.GetNextSectionPath ());
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
					this.Duck();
				}
			}
		}
	}

	public void MoveUp(){
		if (!GameController.IsStopped) {
			if (!this.isJumping) {
				StartCoroutine (this.JumpRoutine ());
			}
		}
	}

	public void Duck(){
		Debug.Log ("Ducked");
	}
	
	/*
	public void SetNextPath(Vector3[] to){
		LeanTween.moveSpline (this.gameObject, to, 1.0f).setOnComplete (AssignNextSectionPath).setEase(LeanTweenType.linear);
	}

	private void AssignNextSectionPath(){
		Debug.Log ("New path");
		LevelController.GenerateNextLevelSection ();
		this.SetNextPath (LevelController.GetNextSectionPath ());
	}
	*/
	public void Die(){
		foreach (Transform t in this.GetComponentsInChildren<Transform>()) {
			if (t.tag == "Collectable"){
				Collider c = (t.GetComponent<BoxCollider>() == null) ? t.gameObject.AddComponent<BoxCollider>() : t.collider;
				Rigidbody r = (t.GetComponent<Rigidbody>() == null) ? t.gameObject.AddComponent<Rigidbody>() : t.rigidbody;
				r.isKinematic = false;
				r.useGravity = true;
				r.AddForce(this.explosionForce * (r.transform.position - this.transform.position).normalized + Vector3.up);
				r.collider.isTrigger = false;
			}
		}
		//LeanTween.pause (this.gameObject);
	}

	public void Expand(){
		this.playerCollider.Expand (0.1f);
	}

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
			/*
			RaycastHit hit;
			if (Physics.Raycast(this.mesh.transform.position, Vector3.down, out hit, this.playerCollider.radius * 1.05f)){
				Debug.Log(hit.collider.name);
				isGrounded = true;
			}*/
			isGrounded = (this.mesh.transform.position.y <= (this.playerCollider.radius + this.transform.position.y));
			yield return new WaitForFixedUpdate();
		}

		this.weight = startWeight;
		this.isJumping = false;
		this.mesh.transform.localPosition = Vector3.zero;

		Debug.Log ("EndJump");
	}

	public void FallFast(){
		this.weight *= 10;
	}
}

