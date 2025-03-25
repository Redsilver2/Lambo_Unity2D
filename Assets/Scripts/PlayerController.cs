using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Sword,pos_sword;

    private float maxFireDelay = 0.25f;
    private float fireDelay = 0;

    [SerializeField] private float speed;

    private Rigidbody2D rigidBody;
    private float moveInput;

    [SerializeField] private float jumpForce;

    [SerializeField] private bool isGrounded;

    [SerializeField] private Transform feetPos;

    [SerializeField] private float circleRadius;
    [SerializeField] private LayerMask whatIsGround;

    private Animator anim;
    private SpriteRenderer renderer;

    private float jumpDelay = 1f;
    private float Scale_karak;
    private bool isFlipped = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rigidBody.mass = 0.000001f;
        isFlipped = renderer.flipX;
    }


    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, circleRadius, whatIsGround);

        CharacterJump();

        if (Input.GetKeyDown(KeyCode.Space) && fireDelay <= 0f)
        {
            Shooting();
            fireDelay = maxFireDelay;
        }

        if(fireDelay > 0f)
            fireDelay -= Time.deltaTime;
        else
            fireDelay = 0f;

        if(jumpDelay > 0f)
            jumpDelay -= Time.deltaTime;
        else
            jumpDelay = 0f;

		if (transform.position.y  < -20){
           died();
        }

    }


    private void LateUpdate()
    {
        CharacterMovement();
        CharacterAnimation();
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
      if(coll.gameObject.CompareTag("Batas_Mati"))
      {
         died();
      }
    }

    void Die(){
        Debug.Log("Game Over");
		SceneManager.LoadScene("Menu");
    }

    void died()
    {
      SceneManager.LoadScene("GameOver");
    }

     void Shooting(){
        Instantiate(Sword, pos_sword.transform.position, pos_sword.transform.rotation);
     }

    private void CharacterMovement()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (moveInput > 0f)
            isFlipped = false;
        else if (moveInput < 0f)
            isFlipped = true;

        renderer.flipX = isFlipped;
        rigidBody.velocity = Time.deltaTime * (Vector2.right * speed * moveInput +
                                               Vector2.down  * speed / 1.5f);
    }

    void CharacterJump()
    {
        if (isGrounded == true && jumpDelay <= 0f && Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetTrigger("isJump");
            rigidBody.AddForce(Time.deltaTime * (Vector2.right   * rigidBody.velocity.x + 
                                 Vector2.up    * jumpForce));

            jumpDelay = 1.5f;
        }
    }

    void CharacterAnimation()
    {
        bool isRunning = false;

        if (moveInput > 0 && isGrounded == true)
            isRunning = true;

        if (anim != null)
            anim.SetBool("isRun", isRunning);
    }
}
