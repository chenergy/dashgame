using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour
{
	public MeshRenderer	meshRenderer;
	public Animator		animator;
	public Transform 	targetTransform;

	public void Init(Transform targetTransform){
		this.targetTransform = targetTransform;
		this.transform.parent = targetTransform;
	}
	/*
	// Update is called once per frame
	void Update ()
	{
		if (!LevelController.IsStopped) {
			if (this.targetTransform != null) {
				this.transform.position = this.targetTransform.position;
				this.transform.rotation = this.targetTransform.rotation;
			}
		}
	}
	*/
}

