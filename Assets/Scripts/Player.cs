using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    public Bullet bulletPrefab;

    private bool thrusting;
    private float turnDirection;

    [Header("Stats")]
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float timeInvulnerability = 2.5f;


   private void Awake()
   {
       rigidbody = GetComponent<Rigidbody2D>();
       spriteRenderer = GetComponent<SpriteRenderer>();
   } 

   private void Update()
   {
       thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);  //толкаем игрока вверх

       if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
           turnDirection = 1.0f;
       } else if (Input.GetKey(KeyCode.D) ||Input.GetKey(KeyCode.RightArrow)){
           turnDirection = -1.0f;
       } else {
           turnDirection = 0.0f;
       }  

       if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
           Shoot();
       }    
   }

   private void FixedUpdate()
   {
       if (thrusting){
           rigidbody.AddForce(this.transform.up * speed); //если мы толкаем игрока, то двигаем его вверх
       }

       if (turnDirection != 0.0f){
           rigidbody.AddTorque(turnDirection * turnSpeed);//вращение 
       }
   }

   private void Shoot()
   {
       Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
       bullet.Project(this.transform.up);
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
       if (collision.gameObject.layer == LayerMask.NameToLayer("Asteroid")){
           rigidbody.velocity = Vector3.zero;  //останавливаем движение игрока
           rigidbody.angularVelocity = 0.0f;   //останавливаем вращение игрока
           this.gameObject.SetActive(false);              //делаем игрока неактивным, как игровой объект

           GameManager.Instance.PlayerDied(); 
       }
   }

   public SpriteRenderer GetSpriteRenderer()
   {
        return spriteRenderer;
   }
}
