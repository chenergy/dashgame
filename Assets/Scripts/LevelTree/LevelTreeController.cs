using UnityEngine;
using System.Collections;

public class LevelTreeController : MonoBehaviour
{
	public LevelTreeNode 	curLevelNode;
	public GameObject 		startLevelDialogue;
	public GameObject 		map;

	private static LevelTreeController instance = null;

	void Start(){
		if (LevelTreeController.instance == null) {
			LevelTreeController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	public static void SelectNode(LevelTreeNode node){
		instance.curLevelNode = node;
		instance.map.GetComponent<UIDragObject> ().enabled = false;
		instance.startLevelDialogue.SetActive (true);
	}

	public void StartLevel(){
		Application.LoadLevel (curLevelNode.sceneName);
	}

	public void Cancel(){
		instance.curLevelNode = null;
		instance.map.GetComponent<UIDragObject> ().enabled = true;
		instance.startLevelDialogue.SetActive (false);
	}
}

