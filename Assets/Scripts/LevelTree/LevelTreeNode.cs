using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelTreeNode : MonoBehaviour, I_LevelTreeable
{
	public string 				sceneName	= "";
	public bool 				isLocked 	= true;
	public I_LevelTreeable 		parent 		= null;
	public I_LevelTreeable[] 	children;

	public I_LevelTreeable GetParent(){
		return this.parent;
	}

	public I_LevelTreeable[] GetChildren(){
		return this.children;
	}

	public void Unlock(){
		if (this.isLocked)
			this.isLocked = false;
	}

	public void Select(){
		if (!this.isLocked) {
			LevelTreeController.SelectNode (this);
		} else {
			//show "cannot select" UI
		}
	}
}

