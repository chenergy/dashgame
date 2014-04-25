using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class A_BTSelector : A_BTNode
{
	protected List<A_BTNode> 	children;
	protected int 				lastVisitedIndex;

	public A_BTSelector() : base(){
		this.children = new List<A_BTNode> ();
		this.lastVisitedIndex = 0;
	}

	public override void Reset ()
	{
		base.Reset ();
		foreach (A_BTNode bta in this.children) {
			bta.Reset();
		}
	}

	public void AddChild (A_BTNode child){
		this.children.Add (child);
	}

	protected abstract A_BTNode SelectChild();
}

