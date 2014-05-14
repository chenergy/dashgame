using UnityEngine;
using System.Collections;

public class BTAction_CreateObjectAndMoveLocal : A_BTAction
{
	private GameObject 	gobj;
	private Transform	parent;
	private Vector3		fromLocal;
	private Vector3		toLocal;
	private float 		duration;
	private bool		arc;
	private GameObject	source;

	private GameObject	instance;
	private float 		parameter;
	private Vector3		prevPosition;

	public BTAction_CreateObjectAndMoveLocal( PlayerEntity player, GameObject gobj, Vector3 fromLocal, Vector3 toLocal, Transform parent, float duration, bool arc = false ) : base(player){
		this.gobj 		= gobj;
		this.fromLocal 	= fromLocal;
		this.toLocal 	= toLocal;
		this.parent 	= parent;
		this.duration 	= duration;
		this.arc 		= arc;
	}

	public BTAction_CreateObjectAndMoveLocal( PlayerEntity player, GameObject gobj, GameObject source, Vector3 toLocal, Transform parent, float duration, bool arc = false ) : base(player){
		this.gobj 		= gobj;
		this.source 	= source;
		this.toLocal 	= toLocal;
		this.parent 	= parent;
		this.duration 	= duration;
		this.arc 		= arc;
	}

	public override void OnEnter ()
	{
		this.parameter = 0.0f;
		this.instance = GameObject.Instantiate (this.gobj, Vector3.zero, Quaternion.identity) as GameObject;
		this.instance.transform.parent = this.parent;

		if (this.source != null) {
			this.fromLocal = this.source.transform.localPosition;
		}

		this.instance.transform.localPosition = this.fromLocal;
		this.prevPosition = this.instance.transform.localPosition;
	}
	
	public override void OnExit ()
	{
		this.parameter = 0.0f;

		if (this.instance != null) {
			this.instance.transform.parent = null;
			this.instance = null;
		}
	}

	public override BTStatus Execute(){
		if (this.instance != null) {
			if ((this.instance.transform.localPosition - this.toLocal).sqrMagnitude > 0.01f) {
				if (this.arc) {
					this.instance.transform.localPosition = this.CalculateBezier (this.fromLocal, this.fromLocal + Vector3.up * 5, this.toLocal + Vector3.up * 5, this.toLocal);
				} else {
					this.instance.transform.localPosition = Vector3.Lerp (this.fromLocal, this.toLocal, parameter);
				}

				this.instance.transform.localRotation = Quaternion.LookRotation(this.instance.transform.localPosition - this.prevPosition, Vector3.up);
				this.prevPosition = this.instance.transform.localPosition;
				this.parameter += Time.deltaTime / this.duration;
			} else {
				this.instance.transform.parent = null;
				return BTStatus.FINISHED;
			}
			return BTStatus.RUNNING;
		} else {
			return BTStatus.FINISHED;
		}
	}

	private Vector3 CalculateBezier(Vector3 P1, Vector3 H1, Vector3 H2, Vector3 P2){
		float t = this.parameter;
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;
		
		Vector3 p = uuu * P1; //first term
		p += 3 * uu * t * H1; //second term
		p += 3 * u * tt * H2; //third term
		p += ttt * P2; //fourth term
		
		return p;
	}
}

