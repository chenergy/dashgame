using UnityEngine;
using System.Collections;

public class BTSelector_Random : A_BTSelector
{
	public BTSelector_Random() : base(){
	}

	public override BTStatus Execute (){
		if (this.children.Count > 0) {
			A_BTNode child = this.children [this.lastVisitedIndex];
				
			while (child != null) {
				this.status = child.Execute ();
					
				if (this.status == BTStatus.RUNNING) {
					this.status = BTStatus.RUNNING;
					return BTStatus.RUNNING;
				} else {
					child = this.SelectChild ();
				}
			}
				
			this.status = BTStatus.FINISHED;
			return BTStatus.FINISHED;
		}
		
		throw new System.NullReferenceException ();
		
		return BTStatus.FAILED;
	}

	protected override A_BTNode SelectChild ()
	{
		this.lastVisitedIndex = Random.Range (0, this.children.Count);
		return this.children [this.lastVisitedIndex];
	}
}

