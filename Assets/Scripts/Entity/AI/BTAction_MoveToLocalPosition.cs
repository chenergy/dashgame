using UnityEngine;
using System.Collections;

public class BTAction_MoveToLocalPosition : A_BTAction
{
	private GameObject 	source;
	private Vector3 	localTarget;
	private float 		duration;

	private float 		parameter;
	private Vector3		startLocation;

	public BTAction_MoveToLocalPosition( PlayerEntity player, GameObject source, Vector3 localTarget, float duration ) : base(player){
		this.source = source;
		this.localTarget = localTarget;
		this.duration = duration;
	}

	public override void OnEnter ()
	{
		this.parameter = 0.0f;
		this.startLocation = this.source.transform.localPosition;
	}

	public override void OnExit ()
	{
		this.parameter = 0.0f;
	}

	public override BTStatus Execute(){
		if ((this.source.transform.localPosition - this.localTarget).sqrMagnitude > 0.01f) {
			this.source.transform.localPosition = Vector3.Lerp(this.startLocation, this.localTarget, this.parameter);
			this.parameter += Time.deltaTime/this.duration;
			/*
			Vector3 direction = (this.localTarget - this.source.transform.localPosition).normalized;
			this.source.transform.localPosition += Time.deltaTime * this.duration * direction;
			*/
			return BTStatus.RUNNING;
		}

		return BTStatus.FINISHED;
	}
}

