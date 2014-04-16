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
			instance.transform.position = entity.playerFollower.transform.TransformPoint (offset);
			instance.transform.rotation = Quaternion.Euler (0, entity.transform.rotation.eulerAngles.y, 0);
		}
	}


	public static void FocusOnPlayer(PlayerEntity entity){
		instance.entity = entity;
		instance.transform.rotation = Quaternion.identity;
	}

	public static void PanOut (Vector3 amount){
		instance.StartCoroutine (instance.PanOutRoutine (amount));
	}

	IEnumerator PanOutRoutine(Vector3 amount){
		Vector3 target = amount + offset;
		while ((offset - target).sqrMagnitude > 0.1f) {
			offset += Time.deltaTime * (target - offset).normalized;
			yield return new WaitForEndOfFrame();
		}
	}
}

