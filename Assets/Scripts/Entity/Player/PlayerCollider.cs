using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollider : MonoBehaviour
{
	public GameObject 	mesh;
	public Transform	followerTranform;
	public float 		expansionSpeed = 1.0f;

	private SphereCollider 		sphereCollider;
	private List<GameObject> 	collected;
	private int 				numExpansions = 0;

	void Start(){
		this.collected = new List<GameObject> ();
		this.sphereCollider = this.collider as SphereCollider;
	}

	public void Expand(float amount){
		this.numExpansions++;
		StartCoroutine(this.ExpandRoutine(amount));
	}

	public float ColliderRadius{
		get { return this.sphereCollider.radius; }
		set { this.sphereCollider.radius = value; }
	}

	public float Radius{
		get { return this.sphereCollider.radius * this.mesh.transform.localScale.y; }
	}

	public int NumExpansions {
		get{ return this.numExpansions; }
	}

	IEnumerator ExpandRoutine(float amount){
		Vector3 targetSize = this.mesh.transform.localScale + (Vector3.one * amount);
		float diff = (targetSize.x - this.mesh.transform.localScale.x) * 0.5f;
		Vector3 followerTargetPosition = new Vector3 (0, this.followerTranform.localPosition.y - diff, this.followerTranform.localPosition.z - diff);
		//Vector3 followerTargetPosition = (this.followerTranform.position - this.mesh.transform.position).normalized * (this.Radius + amount);

		while ((this.mesh.transform.localScale - targetSize).sqrMagnitude > 0.1f) {
			this.mesh.transform.localScale = Vector3.Lerp(this.mesh.transform.localScale, targetSize, Time.deltaTime * this.expansionSpeed);
			this.followerTranform.localPosition = Vector3.Lerp(this.followerTranform.transform.localPosition, followerTargetPosition, Time.deltaTime * this.expansionSpeed);
			yield return new WaitForEndOfFrame ();
		}

		this.ClearCollectedList ();
	}

	public void AddCollectable( GameObject collectable ){
		if (collectable != null) {
			Vector3 rand = Random.insideUnitSphere;

			this.collected.Add(collectable);

			collectable.transform.position = this.transform.position + rand * this.Radius;
			collectable.transform.parent = this.transform.parent;
			collectable.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			
			if (collectable.GetComponent<Spin> () != null) {
				collectable.GetComponent<Spin> ().enabled = false;
			}
		}
	}

	private void ClearCollectedList(){
		if (collected.Count > 0) {
			foreach(GameObject c in this.collected){
				GameObject.Destroy(c);
			}
		}
		this.collected = new List<GameObject> ();
	}
}

