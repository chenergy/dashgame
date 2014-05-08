using UnityEngine;
using System.Collections;

public class GlobalCameraController : MonoBehaviour
{
	private static GlobalCameraController instance = null;

	public Camera			skyboxCamera;
	
	private float 			secondsToPan = 2.0f;
	private Transform		baseTransform;
	private Vector3 		baseLocalPositionOffset;

	private Transform		lookTransform;
	private Vector3			lookLocalPositionOffset;

	private Quaternion		baseLocalRotationOffset;
	private Quaternion		lookLocalRotationOffset;

	private bool 			isCameraMoving = false;
	private Vector3			rotationLocks;

	void Awake(){
		if (GlobalCameraController.instance == null) {
			GlobalCameraController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}


	void LateUpdate(){
		if (instance.baseTransform != null) {
			if (!this.isCameraMoving){
				instance.transform.position = new Vector3(instance.baseTransform.TransformPoint (baseLocalPositionOffset).x, 
				                                          instance.baseTransform.position.y + instance.baseLocalPositionOffset.y, 
				                                          instance.baseTransform.TransformPoint (baseLocalPositionOffset).z);
				instance.transform.rotation = Quaternion.Euler (0, instance.baseTransform.rotation.eulerAngles.y, 0);
			}
		}

		skyboxCamera.transform.rotation = instance.transform.rotation;
	}


	public static void SetNewBaseTransform(Transform baseTransform, Vector3 baseTargetLocalOffset = default(Vector3)){
		instance.StopCoroutine ("MoveRoutine");

		//instance.transform.parent 			= baseTransform;
		instance.baseTransform 				= baseTransform;
		instance.baseLocalPositionOffset 	= baseTargetLocalOffset;

		instance.StartCoroutine ("MoveRoutine");
	}


	public static void AddToOffset (Vector3 amount){
		instance.StopCoroutine ("MoveRoutine");

		instance.baseLocalPositionOffset += amount;

		instance.StartCoroutine ("MoveRoutine");
	}


	public static void SetNewLookTransform(Transform lookTransform, float duration, Vector3 lookTransformLocalOffset = default(Vector3)){
		/*
		instance.StopCoroutine ("LookRoutine");

		if (lookTransform == null) {
			instance.lookTransform = instance.transform;
			instance.lookLocalPositionOffset = instance.lookTransform.forward;
		} else {
			instance.lookTransform = lookTransform;
			instance.lookLocalPositionOffset = lookTransformLocalOffset;
		}

		instance.StartCoroutine ("LookRoutine", duration);
		*/
	}


	IEnumerator MoveRoutine(){
		instance.isCameraMoving = true;

		Vector3 startPosition = instance.transform.localPosition;
		Vector3 targetPosition = instance.baseLocalPositionOffset;
		float t = 0.0f;
		
		//while ((startPosition - targetPosition).sqrMagnitude > 0.1f) {
		while (t < 1.0f){
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime / secondsToPan;

			//instance.baseLocalPositionOffset = Vector3.Lerp (startPosition, targetPosition, Mathf.SmoothStep (0.0f, 1.0f, t));
			//instance.transform.LookAt(Vector3.Lerp(this.transform.forward, this.baseTransform.transform.position, Mathf.SmoothStep (0.0f, 1.0f, t)));
			instance.transform.localPosition = Vector3.Lerp (startPosition, targetPosition, Mathf.SmoothStep (0.0f, 1.0f, t));
			//instance.transform.rotation = Quaternion.Lerp (instance.transform.rotation, Quaternion.LookRotation(instance.lookTransform.position - instance.transform.position), Mathf.SmoothStep (0.0f, 1.0f, t));
		}

		instance.transform.localPosition = targetPosition;
		instance.isCameraMoving = false;

		StartCoroutine ("LerpToBaseTransform");
	}

	IEnumerator LerpToBaseTransform(){
		instance.isCameraMoving = true;

		float t = 0.0f;
		while (t < 1.0f) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			instance.transform.position = Vector3.Lerp(instance.transform.position, new Vector3(instance.baseTransform.TransformPoint (baseLocalPositionOffset).x, 
			                                                                                    instance.baseTransform.position.y + instance.baseLocalPositionOffset.y, 
			                                                                                    instance.baseTransform.TransformPoint (baseLocalPositionOffset).z), t);
			instance.transform.rotation = Quaternion.Lerp(instance.transform.rotation, Quaternion.Euler (0, instance.baseTransform.rotation.eulerAngles.y, 0), t);
		}

		instance.isCameraMoving = false;
	}

	IEnumerator LookRoutine( float duration ){
		float t = 0.0f;

		while (t < 1.0f){
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime / duration;

			instance.transform.LookAt( instance.lookTransform.position + lookLocalPositionOffset );
		}

		instance.lookTransform = instance.transform;
		instance.transform.localRotation = Quaternion.identity;
		instance.lookLocalPositionOffset = instance.lookTransform.forward;
	}



	/*
	IEnumerator MoveToRoutine(Vector3 targetLocalPositionOffset){
		Vector3 startOffset = instance.baseLocalPositionOffset;
		float t = 0.0f;

		while ((baseLocalPositionOffset - targetLocalPositionOffset).sqrMagnitude > 0.1f) {
			instance.baseLocalPositionOffset = Vector3.Lerp (startOffset, targetLocalPositionOffset, Mathf.SmoothStep (0.0f, 1.0f, t));

			yield return new WaitForEndOfFrame();
			t += Time.deltaTime / seconds;
		}
	}


	IEnumerator RotateToRoutine(Quaternion target){
		Quaternion startRotation = instance.transform.rotation;
		float t = 0.0f;
		
		while (Quaternion.Angle(this.transform.rotation, target) > 0.1f) {
			instance.transform.rotation = Quaternion.Lerp (startRotation, target, Mathf.SmoothStep (0.0f, 1.0f, t));

			yield return new WaitForEndOfFrame();
			t += Time.deltaTime / seconds;
		}
	}
	*/
}

