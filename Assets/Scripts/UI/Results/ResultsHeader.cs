using UnityEngine;
using System.Collections;

public class ResultsHeader : MonoBehaviour
{
	public UILabel header;
	
	public void Init(string text){
		this.header.text = text;
	}
}

