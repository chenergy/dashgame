using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	private static AudioController instance = null;

	void Awake(){
		if (AudioController.instance == null) {
			AudioController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}
}
