﻿using UnityEngine;

public class BatasBawah : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll){
        if(coll.CompareTag("Player")){
            PlayerHealth playerHealth = coll.GetComponent<PlayerHealth>();
            
            if(playerHealth != null)
                 playerHealth.makeDead();
        }
    }
}
