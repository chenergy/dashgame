using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultsUIController : MonoBehaviour
{
	public UITable 		tableObject;
	public GameObject 	resultsCollectablePrefab;
	public GameObject	resultsTotalPrefab;
	public GameObject	resultsHeaderPrefab;
	public GameObject	resultsDividerPrefab;

	private int 		numInTable = 2;
	
	// Use this for initialization
	void Start ()
	{
		Dictionary<string, int> collectables = GameController.GetCollectableNum ();

		foreach (string c in collectables.Keys) {
			this.AddResultToTable(c, collectables[c], GameController.GetCollectableMass(c));
		}

		this.AddDivider ();
		this.AddTotalToTable ();
	}

	public void GoToStart(){
		GameController.SetStartMenu ();
		Application.LoadLevel ("start-menu");
	}

	private void AddResultToTable( string collectable, int num, int mass ){
		GameObject newResult = GameObject.Instantiate (this.resultsCollectablePrefab) as GameObject;
		this.ReAdjust (newResult);

		ResultsCollectable rc = newResult.GetComponent<ResultsCollectable> ();
		rc.Init (collectable, num, mass * num);

		tableObject.Reposition ();
	}

	private void AddTotalToTable(){
		GameObject newTotal = GameObject.Instantiate (this.resultsTotalPrefab) as GameObject;
		this.ReAdjust (newTotal);
	}

	private void AddDivider(){
		GameObject divider = GameObject.Instantiate (this.resultsDividerPrefab) as GameObject;
		this.ReAdjust (divider);
	}

	private void AddHeader(string text){
		GameObject header = GameObject.Instantiate (this.resultsHeaderPrefab) as GameObject;

		ResultsHeader rh = header.GetComponent<ResultsHeader> ();
		rh.Init (text);

		this.ReAdjust (resultsDividerPrefab);
	}

	private void ReAdjust(GameObject gobj){
		gobj.name = numInTable.ToString ();
		gobj.transform.parent = this.tableObject.transform;
		gobj.transform.localPosition = Vector3.zero;
		gobj.transform.localScale = Vector3.one;
		tableObject.Reposition ();
		this.numInTable++;
	}
}

