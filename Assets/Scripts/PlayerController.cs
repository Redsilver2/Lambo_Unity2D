using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject Sword,pos_sword;
    [SerializeField] private bool canMoveInAir = true;
    // public GameObject gameOverScreen;
    // public GameManager theGameManager;


    private float maxFireDelay = 0;
    private float fireDelay = 0;
    // untuk mengatur kecepatan saat Player bergerak
    [SerializeField] private float speed;
    // untuk komponen Rigidbody2D
    private Rigidbody2D rigidBody;
    // Untuk menyimpan nilai yang mengkondisikan
    //Player saat bergerak ke kanan atau ke kiri
    private float moveInput;
    // Untuk mengkondisikan benar saat Player menghadap ke kanan
    private bool isFacingRight;

    // memberikan nilai seberapa tinggi Player dapat melompat
    [SerializeField] private float jumpForce;
    // menandakan benar jika Player menyentuh pinjakan atau ground
    [SerializeField] private bool isGrounded;
    // memastikan bahwa posisi kaki Player berada di bawah,
    //seperti telapak kaki gitu loh sobat
    [SerializeField] private Transform feetPos;
    // ini digunakan untuk mengatur seberapa besar radius kaki Player sobat
    //"Kurang lebih seperti itu :)"
    [SerializeField] private float circleRadius;
    // Ini digunakan untuk memastikan object
    //yang bertindak / kita jadikan sebagai ground
    [SerializeField] private LayerMask whatIsGround;

    //variabel ini kita panggil
    //untuk menjalankan animasi idle, run, dan jump
    private Animator anim;

    private void Start()
    {
        //inisialisasi komponen Rigidbody2D yang ada pada Player
        rigidBody = GetComponent<Rigidbody2D>();
        //kita set di awal BENAR karena Player menghadap ke kanan
        isFacingRight = true;
        //Inisialisasi komponen Animator yang ada pada Player
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        //Dengan memanggil class Physics2D dan fungsi OverlapCircle
        //yang memiliki 3 parameter ini menandakan bahwa
        //isGrounded akan bernilai benar jika ketiga parameter tersebut terpenuhi
        isGrounded = Physics2D.OverlapCircle(feetPos.position, circleRadius, whatIsGround);

        //Fungsi untuk Player saat melompat
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



            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPosition.y > Screen.height || screenPosition.y < 0){
            died();
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
      if(coll.gameObject.tag == "Batas_Mati")
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


    private void FixedUpdate()
    {
        // Fungsi yang memanage inputan
        //saat Player bergerak ke Kanan atau ke Kiri
        CharacterMovement();
        // Fungsi yang mengatur
        //transisi animasi Player
        //saat idle, run atau jump
        CharacterAnimation();

    }
    public float Scale_karak;
     void Shooting(){

        rigidBody.velocity = Vector2.right * 8f * Mathf.Sign(Scale_karak) +
                             Vector2.up    * rigidBody.velocity.y;

        Instantiate(Sword, pos_sword.transform.position, pos_sword.transform.rotation);
     }
    // void OnMousDown (){
	// 	Instantiate(Sword, pos_sword.transform.position, pos_sword.transform.rotation);

	// }

    private void CharacterMovement()
    {
        //Input.GetAxis adalah sebuah fungsi
        //yang telah di sediakan oleh Unity
        //Untuk melihat keyboard inputannya sobat
        //buka di menu edit terus pilih Project Setting dan pilih Input
        float sign;

        moveInput = Input.GetAxis("Horizontal");
        sign = Mathf.Sign(moveInput);

        if (sign == 1f && isFacingRight == false)
        {
            Flip();
            isFacingRight = true;
        }
        else if (sign == -1f && isFacingRight == true)
        {
            Flip();
            isFacingRight = false;
        }
        // nilai pada sumbu X akan bertambah sesuai dg speed * moveInput
        rigidBody.velocity = Time.deltaTime * (Vector2.right * speed * moveInput +
                             Vector2.up     * rigidBody.velocity.y);
    }

    void CharacterJump()
    {

        if (isGrounded == true &&  Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetTrigger("isJump");
            rigidBody.velocity = Vector2.right * rigidBody.velocity.x + 
                                 Vector2.up    * jumpForce;
        }
    }

    void CharacterAnimation()
    {
        if(anim != null)
            anim.SetBool("isRun", moveInput != 0 && isGrounded == true);
    }

    private void Flip()
    {
        transform.localScale = Vector3.right   * transform.localScale.x * Mathf.Sign(moveInput) +
                               Vector3.up      * transform.localScale.y                         +
                               Vector3.forward * transform.localScale.z;
    }
}
