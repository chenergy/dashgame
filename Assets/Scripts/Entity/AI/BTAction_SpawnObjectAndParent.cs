using UnityEngine;
using System.Collections;

public class BTAction_SpawnObjectAndParent : A_BTAction
{
	private GameObject 	gobj;
	private Transform	parent;

	public BTAction_SpawnObjectAndParent( PlayerEntity player, GameObject gobj, Transform parent ) : base(player){
		this.gobj 		= gobj;
		this.parent 	= parent;
	}
	
	public override BTStatus Execute(){
		if (this.gobj != null) {
			GameObject instance = GameObject.Instantiate(this.gobj, this.parent.position, Quaternion.identity) as GameObject;
			instance.transform.parent = this.parent;
			instance.transform.localPosition = Vector3.zero;
			return BTStatus.FINISHED;
		}

		return BTStatus.FAILED;
	}
}

