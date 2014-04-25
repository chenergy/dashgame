using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour
{
	public Transform 	start;
	public Transform 	end;
	public float 		speed;

	private Vector3 	direction;
	private Animator 	animator;

	// Use this for initialization
	void Start ()
	{
		this.animator = this.GetComponent<Animator> ();

		this.direction = (end.position - start.position).normalized;
		this.transform.position = this.start.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.position += direction * Time.deltaTime * speed;
	}
}

