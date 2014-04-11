using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour
{
	public MeshRenderer	meshRenderer;
	public Animator		animator;
	//public float 		closeness = 5.0f;
	public PlayerEntity target;

	public void Init(PlayerEntity target){
		this.target = target;
	}
	// Update is called once per frame
	/*void Update ()
	{
		if (!GameController.IsStopped) {
			if (this.target != null){
				//this.transform.position = Vector3.Lerp (this.transform.position, this.target.transform.position, Time.deltaTime * this.closeness);
			}
		}
	}
	*/

}

