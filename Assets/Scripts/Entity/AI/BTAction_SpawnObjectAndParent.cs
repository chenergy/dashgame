using UnityEngine;
using System.Collections;

public class BTAction_SpawnObjectAndParent : A_BTAction
{
	private GameObject 	gobj;
	private Transform	parent;
	private Vector3		localPosition;

	public BTAction_SpawnObjectAndParent( PlayerEntity player, GameObject gobj, Transform parent, Vector3 localPosition = default(Vector3) ) : base(player){
		this.gobj 			= gobj;
		this.parent 		= parent;
		this.localPosition 	= localPosition;
	}
	
	public override BTStatus Execute(){
		if (this.gobj != null) {
			GameObject instance 				= GameObject.Instantiate(this.gobj, this.parent.position, Quaternion.identity) as GameObject;
			instance.transform.parent 			= this.parent;
			instance.transform.localPosition 	= this.localPosition;
			return BTStatus.FINISHED;
		}

		return BTStatus.FAILED;
	}
}

