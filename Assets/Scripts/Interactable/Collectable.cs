using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour, I_Interactable
{
	public string		name;
	public GameObject 	mesh;
	public GameObject	visualPrefab;
	public bool			canAttach;
	public bool			canMagnetize;
	public int 			minSizeForPickup 	= 0;
	public int 			minSizeForIgnore 	= 1;
	public int			mass 				= 1;

	private float 		magnetizeSpeed 		= 1.0f;
	private bool 		alreadyMagnetized 	= false;

	void Update(){
		if (this.canMagnetize) {
			if (LevelController.ActivePlayer.playerCollider.NumExpansions >= this.minSizeForPickup) {
				if (!this.alreadyMagnetized) {
					if (LevelController.ActivePlayer != null) {
						if (LevelController.ActivePlayer.IsMagnetized) {
							if ((this.transform.position - LevelController.ActivePlayer.transform.position).sqrMagnitude < 100) {
								this.alreadyMagnetized = true;
								this.Magnetize ();
							}
						}
					}
				}
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider other){
		if (other.tag == "Ball") {
			PlayerEntity p = other.GetComponent<PlayerCollider>().entity;
			this.AffectPlayer( p );
		}
	}

	public void Magnetize(){
		if (this.canMagnetize) {
			StartCoroutine (this.MagnetizeRoutine ());
		}
	}

	IEnumerator MagnetizeRoutine(){
		float t = 0.0f;

		while (t < 1) {
			t += Time.deltaTime * this.magnetizeSpeed;
			this.transform.position = Vector3.Lerp (this.transform.position, LevelController.ActivePlayer.gobj.transform.position, t);
			yield return new WaitForEndOfFrame ();
		}
	}

	public virtual void AffectPlayer (PlayerEntity p){
		PlayerCollider pc = p.playerCollider;

		if (this.minSizeForIgnore > pc.NumExpansions) {
			if (this.minSizeForPickup <= pc.NumExpansions) {
				if (this.canAttach) {
					AudioController.PlayPickupSound();
					p.AddMass(this.mass);
					UIController.UpdateMass (p.Mass);
					pc.AddCollectable (this.mesh);
				}

				GameObject.Destroy (this.gameObject);
				UIController.UpdateItem (this);
			} else {
				if (LevelController.CanCollide) {
					LevelController.EndGame ();
				}
			}
		}
	}
}

