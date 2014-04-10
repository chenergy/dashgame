using UnityEngine;
using System.Collections;

public class PlayerBallRoller : MonoBehaviour
{
	/*
	public GameObject	mesh;
	public PlayerEntity entity;

	public float 		maxSpeed = 10.0f;
	public float 		explosionForce = 50.0f;


	private Transform	target;

	// Update is called once per frame
	void LateUpdate ()
	{
		if (!GameController.IsStopped) {
			this.transform.position = this.target.position + Vector3.up;
			this.transform.Rotate (new Vector3 (this.maxSpeed * Time.deltaTime, 0, 0));
		}
	}

	public void StartRolling(){
		this.target = entity.GetTargetTransform ();
	}

	public void UpdateTarget(){
		if (!GameController.IsStopped) {
			this.target = this.entity.GetTargetTransform();
		}
	}

	public void Explode(){
		foreach (Rigidbody r in this.GetComponentsInChildren<Rigidbody>()) {
			if (r.tag == "Collectable"){
				r.isKinematic = false;
				r.useGravity = true;
				r.AddForce(this.explosionForce * (r.transform.position - this.transform.position).normalized + Vector3.up);
				r.collider.isTrigger = false;
			}
		}
	}

	public void Jump(){

	}

	IEnumerator JumpRoutine(){

	}
	*/
}

