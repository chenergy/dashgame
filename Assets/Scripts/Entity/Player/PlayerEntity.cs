using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEntity : MonoBehaviour, IEntity
{
	public PlayerCollider 	collider;
	public MeshRenderer		meshRenderer;
	public Animator			animator;

	private PlayerLaneTransform targetLane;
	private int 				currentLane = 1;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.targetLane != null) {
			this.transform.position = this.targetLane.transform.position;
			this.transform.rotation = this.targetLane.transform.rotation;
		}
		/*if (!GameController.IsStopped) {
			this.transform.Translate (Vector3.forward * GameController.GameSpeed * Time.deltaTime);
		}*/
	}

	public void StartMoving(){
		this.targetLane = LevelController.GetLaneTransform (this.currentLane);
		//this.SetNextPath (LevelController.GetNextSectionPath ());
	}

	public void MoveLeft(){
		if (!GameController.IsStopped) {
			//this.transform.position = this.transform.position + Vector3.right * 5.0f;
			if (this.currentLane > 0){
				this.currentLane--;
			}
			this.targetLane = LevelController.GetLaneTransform (this.currentLane);
		}
	}

	public void MoveRight(){
		if (!GameController.IsStopped) {
			//this.transform.position = this.transform.position + Vector3.left * 5.0f;
			if (this.currentLane < 2){
				this.currentLane++;
			}
			this.targetLane = LevelController.GetLaneTransform (this.currentLane);
		}
	}

	public void Jump(){
		Debug.Log ("Jumped");
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
		//LeanTween.pause (this.gameObject);
	}
}

