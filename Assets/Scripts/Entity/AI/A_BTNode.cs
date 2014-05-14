using UnityEngine;
using System.Collections;

public enum BTStatus { READY, RUNNING, VISITING, FINISHED, FAILED }

public abstract class A_BTNode {
	protected BTStatus status;

	public A_BTNode(){ }

	public virtual void Reset(){
		if (this.status != BTStatus.RUNNING) {
			this.status = BTStatus.READY;
		}
	}

	public BTStatus Status{
		get { return this.status; }
	}
	
	public abstract void OnEnter();
	public abstract void OnExit();
	public abstract BTStatus Execute ();
}

