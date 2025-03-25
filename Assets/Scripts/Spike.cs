using Boo.Lang;
using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour {

   [SerializeField] private float damage;
   [SerializeField] private float damageRate;
   [SerializeField] private float pushBackForce = 10f;

    private IEnumerator damageEnumerator;
    private static List<Spike> spikesTouched = new List<Spike>();

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
            spikesTouched.Add(this);

            if(spikesTouched.Count == 1 && damageEnumerator == null)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
               
                if (playerHealth != null) {
                    damageEnumerator = TakeDamage(playerHealth);
                    StartCoroutine(damageEnumerator);
                }
            }
        }
	}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spikesTouched.Remove(this);
        }
    }

    private IEnumerator TakeDamage(PlayerHealth health)
	{
        while (health != null && spikesTouched.Count != 0)
        {
            health.addDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
	}

	void pushBack(Transform pushObject){
		
        Vector2 pushDirection = new Vector2(0, (pushObject.position.y - transform.position.y)).normalized;
		pushDirection*=pushBackForce;
		Rigidbody2D pushRB = pushObject.gameObject.GetComponent<Rigidbody2D>();

        if (pushRB != null)
        {
            pushRB.velocity = Vector2.zero;
            pushRB.AddForce(pushDirection, ForceMode2D.Impulse);
        }
	}
}
