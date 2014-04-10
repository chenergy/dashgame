using UnityEngine;
using System.Collections;

public class GlobalCameraController : MonoBehaviour
{
	private static GlobalCameraController instance = null;

	public Vector3 offset;
	private PlayerEntity entity;

	void Awake(){
		if (GlobalCameraController.instance == null) {
			GlobalCameraController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}


	void LateUpdate(){
		if (instance.entity != null) {
			/*instance.transform.position = new Vector3 (entity.playerFollower.transform.TransformPoint (new Vector3 (offset.x, 0, 0)).x, 
			                                          entity.playerFollower.transform.position.y + offset.y, 
			                                          entity.playerFollower.transform.TransformPoint (new Vector3 (0, 0, offset.z)).z);
			                                          */
			instance.transform.position = entity.playerFollower.transform.TransformPoint (offset)/* + new Vector3 (0, entity.playerFollower.transform.position.y, 0)*/;
			//if (Mathf.Abs(entity.transform.rotation.eulerAngles.y - instance.transform.rotation.eulerAngles.y) > 0.5f){
				instance.transform.rotation = Quaternion.Euler (0, entity.transform.rotation.eulerAngles.y, 0);
			//}
		}
	}


	public static void FocusOnPlayer(PlayerEntity entity){
		instance.entity = entity;
		instance.transform.rotation = Quaternion.identity;
		//instance.transform.parent = entity.transform;
	}
}

