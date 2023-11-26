using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    public float speed = 500.0f;
    public float maxLifetime = 10.0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        rigidbody.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.maxLifetime);
    }

    //при столкновении с чем-либо, снаряд будет разрушаться
    private void OnCollisionEnter2D(Collision2D collision)  
    {
        Destroy(this.gameObject);  
    }

}
