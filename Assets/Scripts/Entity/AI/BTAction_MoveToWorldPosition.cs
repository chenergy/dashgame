using UnityEngine;
using System.Collections;

public class BTAction_MoveToWorldPosition : A_BTAction
{
	private GameObject 	source;
	private Vector3 	target;
	private float 		duration;

	private float 		parameter;
	private Vector3		startLocation;

	public BTAction_MoveToWorldPosition( PlayerEntity player, GameObject source, Vector3 target, float duration ) : base(player){
		this.source = source;
		this.target = target;
		this.duration = duration;

		this.parameter = 0.0f;
		this.startLocation = this.source.transform.position;
	}

	public override void OnEnter ()
	{
		this.parameter = 0.0f;
		this.startLocation = this.source.transform.position;
	}
	
	public override void OnExit ()
	{
		this.parameter = 0.0f;
	}

	public override BTStatus Execute(){
		if ((this.source.transform.position - this.target).sqrMagnitude > 0.01f) {
			this.source.transform.position = Vector3.Lerp(this.startLocation, this.target, this.parameter);
			this.parameter += Time.deltaTime/this.duration;
			/*
			Vector3 direction = (this.target - this.source.transform.position).normalized;
			this.source.transform.position += Time.deltaTime * this.duration * direction;
			*/
			return BTStatus.RUNNING;
		}

		return BTStatus.FINISHED;
	}
}

