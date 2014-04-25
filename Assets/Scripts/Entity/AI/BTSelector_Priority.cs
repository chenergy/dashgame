using UnityEngine;
using System.Collections;

public class BTSelector_Priority : A_BTSelector
{
	public BTSelector_Priority() : base(){
		// order the children from highest to lowest priority
	}

	public override BTStatus Execute (){
		if (this.children.Count > 0) {
			if (this.lastVisitedIndex < this.children.Count) {
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

			this.status = BTStatus.FINISHED;
			return BTStatus.FINISHED;
		}

		throw new System.NullReferenceException ();

		return BTStatus.FAILED;
	}

	protected override A_BTNode SelectChild (){
		this.lastVisitedIndex++;
		
		if (this.lastVisitedIndex < this.children.Count) {
			return this.children [this.lastVisitedIndex];
		}
		
		return null;
	}
}

