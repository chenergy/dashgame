using UnityEngine;
using System.Collections;

public class BTAction_MoveToWorldPosition : A_BTAction
{
	private GameObject 	source;
	private Vector3 	target;
	private float 		speed;

	public BTAction_MoveToWorldPosition( PlayerEntity player, GameObject source, Vector3 target, float speed ) : base(player){
		this.source = source;
		this.target = target;
		this.speed = speed;
	}
	
	public override BTStatus Execute(){
		if ((this.source.transform.position - this.target).sqrMagnitude > 0.01f) {
			Vector3 direction = (this.target - this.source.transform.position).normalized;
			this.source.transform.position += Time.deltaTime * this.speed * direction;
			return BTStatus.RUNNING;
		}

		return BTStatus.FINISHED;
	}
}

