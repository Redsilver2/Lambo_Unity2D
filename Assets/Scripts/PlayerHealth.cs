using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour {
    [SerializeField] private float fullHealth;
    //public GameObject deathFX;
    private float currentHealth;
    [SerializeField] private AudioClip playerHurt;
	private AudioSource playerAS;
	// public GameObject gameOverScreen;
	// public GameManager theGameManager;

	[Space]
    [SerializeField] private Slider heartBar;
    [SerializeField] private Image damageScreen;

    private bool damaged = false;
	private Color damagedColour = new Color(5f,5f,0f,0.5f);
    private	float smoothColour = 5f;

	// Use this for initialization
	void Start () {
		currentHealth = fullHealth;

		//Heart Bar
		heartBar.maxValue=fullHealth;
		heartBar.value=fullHealth;

		playerAS =GetComponent<AudioSource>();

		damaged = false;
	}

	// Update is called once per frame
	void Update () {
		if(damaged){
			damageScreen.color = damagedColour;
		}else{
			damageScreen.color = Color.Lerp(damageScreen.color, Color.clear,smoothColour*Time.deltaTime);
		}
		damaged = false;

	}

	public void addDamage(float damage){
		if(damage<=0) return;
		currentHealth = currentHealth - damage;
		//playerAS.clip =  playerHurt;
		//playerAS.Play(1);
		playerAS.PlayOneShot(playerHurt);
		heartBar.value = currentHealth;
		damaged = true;

		if(currentHealth<=0){
			makeDead();
		}
	}
	public void addHealth(float health){
		currentHealth += health;
		if(currentHealth>fullHealth) currentHealth=fullHealth;
		heartBar.value =currentHealth;
	}
	public void makeDead(){
		//Instantiate(deathFX, transform.position, transform.rotation);

		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		SceneManager.LoadScene("GameOver");
	}
}
