using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private float enemyhealth;
    [SerializeField] private bool drops;

    [Space]
    [SerializeField] private Slider enemyHealthBar;

	[Space]
	[SerializeField] private GameObject thedrop;
	[SerializeField] private AudioClip deathKnell;
    
	private float currhealth;

    // Use this for initialization
    private void Start () {
		currhealth = enemyhealth;
		enemyHealthBar.maxValue = currhealth;
		enemyHealthBar.value = currhealth;
	}

	public void DiDor(float damage){
		currhealth = currhealth - damage;
		enemyHealthBar.value = currhealth;
		if(currhealth<=0){
			makeDead();
		}
	}

	private void makeDead(){
		AudioSource.PlayClipAtPoint(deathKnell, transform.position);
		// Instantiate (enemyDeathFX,transform.position, transform.rotation);

		 if(drops) Instantiate(thedrop,transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
