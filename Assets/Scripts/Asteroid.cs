using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;

    [SerializeField] Sprite[] asteroidSprites;

    [Header("Stats")]
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;

    [SerializeField] float speed = 50.0f;
    [SerializeField] float maxLifetime = 30.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];  //устанавливаем рандомный спрайт для нашего астероида
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f); //задаём случайное вращение для астероида. [0, 1] * 360 градусов (чисто для визуального разнообразия)
        this.transform.localScale = Vector3.one * this.size; // = new Vector3(this.size, this.size, this.size);

        rigidbody.mass = this.size; //чем больше размер астероида - тем больше его масса
    }

    public void SetTrajectory(Vector2 direction)
    {
        rigidbody.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.maxLifetime);   //Уничтожаем астероид спустя время     
    }

    //реализуем разбиене на 2 части астероида при его соприкосновении со снарядом
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet"){
            if (this.size * 0.5f >= this.minSize){ //разбиене происходит лишь в том случае, если размер получивейся части меньше чем минимальный возможный
                CreateSplit();
                CreateSplit();
            }
            GameManager.Instance.AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }

    }

    private void CreateSplit()
    {
        Vector2 newPosition = this.transform.position;
        newPosition += Random.insideUnitCircle * 0.5f; //радиус шара-области, в которой будет спавниться новый мини-астероид - 0.5 юнитов
        Asteroid halfOfAsteroid = Instantiate(this, newPosition, this.transform.rotation);
        halfOfAsteroid.size = this.size * 0.5f; //соответственно уменьшаем вдвое новый получившийся астероид

        halfOfAsteroid.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);  //задаём случайную траекторию полёта для нового астероида
    }
    

}
