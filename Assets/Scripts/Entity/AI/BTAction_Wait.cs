using UnityEngine;
using System.Collections;

public class BTAction_Wait : A_BTAction
{
	private float 	waitTime;
	private float 	timer;

	public BTAction_Wait( PlayerEntity player, float waitTime ) : base(player){
		this.waitTime = waitTime;
	}
	
	public override BTStatus Execute(){
		this.timer += Time.deltaTime;
		if (this.timer < this.waitTime) {
			return BTStatus.RUNNING;
		} else {
			this.timer = 0.0f;
			return BTStatus.FINISHED;
		}

		return BTStatus.FAILED;
	}
}

