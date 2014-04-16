using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour
{
	public MeshRenderer	meshRenderer;
	public Animator		animator;
	//public float 		closeness = 5.0f;
	public Transform 	targetTransform;

	public void Init(Transform targetTransform){
		this.targetTransform = targetTransform;
	}
	// Update is called once per frame
	void Update ()
	{
		if (!GameController.IsStopped) {
			if (this.targetTransform != null) {
				//this.transform.position = Vector3.Lerp (this.transform.position, this.target.transform.position, Time.deltaTime * this.closeness);
				this.transform.position = this.targetTransform.position;
				this.transform.rotation = this.targetTransform.rotation;
			}
		}
	}
}

