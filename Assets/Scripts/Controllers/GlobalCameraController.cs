using UnityEngine;
using System.Collections;

public class GlobalCameraController : MonoBehaviour
{
	private static GlobalCameraController instance = null;

	private PlayerEntity entity;

	void Awake(){
		if (GlobalCameraController.instance == null) {
			GlobalCameraController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	void LateUpdate(){
		if (this.entity != null) {
			this.transform.position = entity.transform.position + new Vector3 (0, 3, -10);
		}
	}

	public static void FocusOnPlayer(PlayerEntity entity){
		instance.entity = entity;
		instance.transform.rotation = Quaternion.identity;
	}
}

