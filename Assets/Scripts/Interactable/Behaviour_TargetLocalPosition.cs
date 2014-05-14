using UnityEngine;
using System.Collections;

public class Behaviour_TargetLocalPosition : MonoBehaviour
{
	public GameObject	movingObject;
	public Transform 	target;
	public GameObject	onCompleteParticle;
	public bool 		easeIn 				= false;
	public bool 		easeOut 			= false;
	public bool			destroyOnComplete 	= false;
	public float 		arcScale 			= 1.0f;
	public float 		duration 			= 1.0f;
	
	//protected Vector3	startPosition;
	//protected float		parameter = 0.0f;
	//protected bool 		hasCompleted = false;
	
	
	public void MoveStraight( Vector3 fromGlobal, Vector3 toGlobal ){
		this.movingObject.transform.position = fromGlobal;
		Vector3 toLocal = this.transform.InverseTransformPoint (toGlobal);

		LeanTween.moveLocal (this.movingObject, toLocal, this.duration).setOnComplete (this.OnComplete);
	}


	public void MoveArc( Vector3 fromGlobal, Vector3 toGlobal ){
		Vector3 P1 = this.transform.InverseTransformPoint (fromGlobal);
		//GameObject.CreatePrimitive (PrimitiveType.Cube).transform.position = P1;
		Vector3 P2 = this.transform.InverseTransformPoint (toGlobal);
		//GameObject.CreatePrimitive (PrimitiveType.Cube).transform.position = P2;
		Vector3 H1 = P1 + Vector3.up * arcScale;
		Vector3 H2 = P2 + Vector3.up * arcScale;
		
		LeanTween.moveLocal (this.movingObject, new Vector3[] {P1, H1, H2, P2}, this.duration).setOrientToPath(true).setOnComplete (this.OnComplete);
	}

	/*
	IEnumerator ArcToLocation(){
		this.isJumping = true;
		
		bool 	isGrounded 		= false;
		float 	startY 			= this.gobj.transform.position.y;
		float 	yJump 			= this.jumpStrength;
		float 	curJump 		= 0.0f;
		float 	startWeight 	= this.weight;
		
		int 	bufferFrames 	= 3;
		int 	curFrame 		= 0;
		
		while (!isGrounded) {
			yJump += (Physics.gravity.y * Time.deltaTime * this.weight);
			curJump += yJump;
			this.gobj.transform.position = Vector3.Lerp (this.gobj.transform.position, 
			                                             new Vector3(this.transform.position.x, startY + this.startOffset.y + curJump, this.transform.position.z), 
			                                             Time.deltaTime * this.maxSpeed);
			
			if (curFrame > bufferFrames)
				isGrounded = (this.gobj.transform.position.y <= (this.playerCollider.Radius + this.transform.position.y));
			
			yield return new WaitForEndOfFrame();
			curFrame++;
		}
		
		this.weight = startWeight;
		this.isJumping = false;
		this.gobj.transform.localPosition = Vector3.zero;
	}
	*/

	private void OnComplete(){
		if (this.onCompleteParticle != null) {
			GameObject.Destroy (GameObject.Instantiate (this.onCompleteParticle, this.transform.position, Quaternion.identity), 2.0f);
		}
		
		this.transform.parent = null;
		//this.hasCompleted = true;
		
		if (this.destroyOnComplete) {
			GameObject.Destroy (this.gameObject);
		}
	}
}

