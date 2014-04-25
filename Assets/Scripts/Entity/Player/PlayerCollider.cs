using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollider : MonoBehaviour
{
	public PlayerEntity	entity;
	public GameObject 	mesh;
	public Transform	followerTranform;
	public float 		expansionSpeed = 1.0f;

	private SphereCollider 		sphereCollider;
	private List<GameObject> 	collected;
	private int 				expansionLevel = 0;

	void Start(){
		this.collected = new List<GameObject> ();
		this.sphereCollider = this.collider as SphereCollider;
	}

	// Increase the expansion level of the ball
	public void Expand(float amount){
		// Increase local reference of expansion level
		this.expansionLevel++;

		// Update global reference to expansion level
		LevelController.UpdateExpansionLevel (this.expansionLevel);

		// Execute the visual representation of the expansion
		StartCoroutine(this.ExpandRoutine(amount));
	}

	// The actual local radius of the collider
	public float ColliderRadius{
		get { return this.sphereCollider.radius; }
		set { this.sphereCollider.radius = value; }
	}

	// Find the global size of the collider
	public float Radius{
		get { return this.sphereCollider.radius * this.mesh.transform.localScale.y; }
	}

	// Get the expansion level
	public int NumExpansions {
		get{ return this.expansionLevel; }
	}

	// Do the expansion
	IEnumerator ExpandRoutine(float amount){
		//Vector3 targetSize = this.mesh.transform.localScale + (Vector3.one * amount); // final local scale of the expansion
		Vector3 targetSize = this.mesh.transform.localScale * amount; // final local scale of the expansion
		float diff = (targetSize.x - this.mesh.transform.localScale.x) * 0.5f; // total difference between local scales
		Vector3 followerTargetPosition = new Vector3 (0, this.followerTranform.localPosition.y - diff, this.followerTranform.localPosition.z - diff); // target position of the follower

		while ((this.mesh.transform.localScale - targetSize).sqrMagnitude > 0.1f) {
			// increase size toward target size
			this.mesh.transform.localScale = Vector3.Lerp(this.mesh.transform.localScale, targetSize, Time.deltaTime * this.expansionSpeed); 
			// move follower toward target position
			this.followerTranform.localPosition = Vector3.Lerp(this.followerTranform.transform.localPosition, followerTargetPosition, Time.deltaTime * this.expansionSpeed); 
			yield return new WaitForEndOfFrame ();
		}

		this.ClearCollectedList ();
	}

	// Attach a collectable to the ball
	public void AddCollectable( GameObject collectable ){
		if (collectable != null) {
			Vector3 rand = Random.insideUnitSphere;

			this.collected.Add(collectable);

			collectable.transform.position = this.transform.position + rand * this.Radius;
			collectable.transform.parent = this.transform.parent;
			collectable.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

			// Disable spin
			foreach(Spin s in collectable.GetComponentsInChildren<Spin>()){
				s.enabled = false;
			}
		}
	}

	// Destroy all currently attached collectables
	private void ClearCollectedList(){
		if (collected.Count > 0) {
			foreach(GameObject c in this.collected){
				GameObject.Destroy(c);
			}
		}
		this.collected = new List<GameObject> ();
	}
}

