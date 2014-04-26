using UnityEngine;
using System.Collections;

public class BTAction_MoveToLocalPosition : A_BTAction
{
	private GameObject 	source;
	private Vector3 	localTarget;
	private float 		speed;

	public BTAction_MoveToLocalPosition( PlayerEntity player, GameObject source, Vector3 localTarget, float speed ) : base(player){
		this.source 		= source;
		this.localTarget 	= localTarget;
		this.speed 			= speed;
	}
	
	public override BTStatus Execute(){
		if ((this.source.transform.localPosition - this.localTarget).sqrMagnitude > 0.01f) {
			Vector3 direction = (this.localTarget - this.source.transform.localPosition).normalized;
			this.source.transform.localPosition += Time.deltaTime * this.speed * direction;
			return BTStatus.RUNNING;
		}

		return BTStatus.FINISHED;
	}
}

