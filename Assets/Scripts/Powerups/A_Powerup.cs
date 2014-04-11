using UnityEngine;
using System.Collections;

public abstract class A_Powerup : I_Powerup
{
	private float duration = 1.0f;

	public A_Powerup( float duration ){
		this.duration = duration;
	}

	public float Duration {
		get { return this.duration; }
	}

	public abstract void OnInit ();
	public abstract void OnExecute ();
	public abstract void OnComplete ();
}

