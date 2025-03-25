using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour {

	[SerializeField] private float healthAmount;

	private void OnTriggerEnter2D(Collider2D coll){
		if(coll.CompareTag("Player")){
			PlayerHealth playerHealth  = coll.GetComponent<PlayerHealth>();

			if (playerHealth != null)
			{
				playerHealth.addHealth(healthAmount);
				Destroy(gameObject);
			}
			else
				Debug.LogError("No Player Health Component Found!");
		}
	}
}
