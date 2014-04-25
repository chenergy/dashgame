using UnityEngine;
using System.Collections;

public class BTAction_CheckBossLives : A_BTAction
{
	private A_Boss 	boss;
	private int 	minLives;

	public BTAction_CheckBossLives( PlayerEntity player, A_Boss boss, int minLives ) : base(player){
		this.boss 		= boss;
		this.minLives 	= minLives;
	}
	
	public override BTStatus Execute(){
		if (this.boss.Lives >= this.minLives) {
			//Debug.Log("Success");
			return BTStatus.FINISHED;
		}

		return BTStatus.FAILED;
	}
}

