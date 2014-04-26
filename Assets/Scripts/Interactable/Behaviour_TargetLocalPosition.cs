using UnityEngine;
using System.Collections;

public class Behaviour_TargetLocalPosition : MonoBehaviour
{
	public bool 		easeOut;
	public Transform 	target;
	public float 		speed = 1.0f;

	private Vector3		startPosition;
	private float		parameter = 0.0f;

	void Start(){
		this.startPosition = this.transform.localPosition;
	}

	void Update ()
	{
		if ((this.transform.position - this.target.position).sqrMagnitude > 0.01f) {
			if (this.easeOut) {
				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, this.target.localPosition, Time.deltaTime * speed);
			} else {
				this.parameter += Time.deltaTime * speed;
				this.transform.localPosition = Vector3.Lerp (this.startPosition, this.target.localPosition, parameter);
			}
		} else {
			this.transform.parent = null;
			this.enabled = false;
			GameObject.Destroy(this.gameObject, 2.0f);
		}
	}
}

