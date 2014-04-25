using UnityEngine;
using System.Collections;

public abstract class A_BTAction : A_BTNode
{
	protected PlayerEntity player;

	public A_BTAction( PlayerEntity player ) : base(){
		this.player = player;
	}
}

