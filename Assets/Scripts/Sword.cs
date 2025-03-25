using UnityEngine;

public class Sword : MonoBehaviour {
	[SerializeField] private float damagesenjata;
	private Rigidbody2D rigidbody;
   
    private float scale_karak;
    private float lifeTime = 5f;

    // Use this for initialization
    private void Start () {
		SpriteRenderer renderer = GameObject.Find("player").GetComponent<SpriteRenderer>();

        if (renderer != null)
            if (renderer.flipX)
                scale_karak = -1f;
            else
                scale_karak = 1f;
        
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void LateUpdate () {
        rigidbody.velocity = Time.deltaTime * (Vector2.right * 400f * Mathf.Sign(scale_karak) +
			                  Vector2.up    * rigidbody.velocity.y);

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(gameObject);      
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
			if(coll.CompareTag("Batas")|| coll.CompareTag("Ground"))
			{
				Destroy(gameObject);
			}

		    if(coll.gameObject.layer == LayerMask.NameToLayer("Enemy") && coll.CompareTag("Enemy")) { 
			   EnemyHealth enemyHealth = coll.gameObject.GetComponent<EnemyHealth>();
		       enemyHealth.DiDor(damagesenjata);
               Destroy(gameObject);
            }
    }
}
