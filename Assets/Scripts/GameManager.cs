using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("ObjectRefs")]
    [SerializeField] Player player;
    [SerializeField] ParticleSystem explosion;  //ссылка на эффект взрыва

    const string scoreString = "Score: ";
    const string finalScoreString = "Final score: ";

    [Header("GameStats")]
    public int lives = 3;
    public int score = 0;
    public float respawnTime = 3.0f;
    public float timeInvulnerability = 3f;

    [Header("UIStuff")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject loseUI;
    [SerializeField] TextMeshProUGUI finalScoreText;

    public static GameManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position; //устанавливаем место эффекта - место смерти игрока
        this.explosion.Play();  //играем эффект
        this.lives--;
        UpdateLivesText(lives);

        if (this.lives <= 0) {
            GameOver();
        } else {
            Invoke(nameof(Respawn), this.respawnTime);
            }
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position; //устанавливаем место эффекта - место рарзрушения астероида
        this.explosion.Play();  //играем эффект

        if (asteroid.size < 0.75f){  //Чем меньше астероид, тем больше очков получит игрок при его разрушении
            this.score += 100;
        } else if (asteroid.size < 1.2f){
            this.score += 50;
        } else {
            this.score += 25;
        }
        UpdateScoreText(score);

    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;  //обновляем позицию игрока (ставим его в центр поля)
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions"); //на время убираем все столкновения - устанавливаем игроку другой слой (на тот случай, если игрок зареспаунится внутри астероида)
        this.player.gameObject.SetActive(true);  //делаем игрока вновь и активным игровым объектом
        StartCoroutine(MakePlayerInvulnerable());
    }


    private IEnumerator MakePlayerInvulnerable()
    {
        bool blinked = false;
        float elapsed = 0f;
        float duration = timeInvulnerability;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 10 == 0) {  //каждые 10 фреймов делаем спрайт игрока блёклым/обычным (изменяя альфа-канал) - тем самым создавая иллюзию мигания
                if (!blinked)
                {
                    this.player.GetSpriteRenderer().color = new Color(1f, 1f, 1f, 0.5f);
                    blinked = true;
                } else {
                    this.player.GetSpriteRenderer().color = new Color(1f, 1f, 1f, 1f);
                    blinked = false;
                }
            }

            yield return null;
        }
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
        this.player.GetSpriteRenderer().color = new Color(1f, 1f, 1f, 1f); //устанавливаем альфу на 100% на случай если последним миганием она была установлена на 50%
    }

    private void GameOver()
    {
        loseUI.SetActive(true);
        finalScoreText.text = finalScoreString + score.ToString();
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        lives = 3;
        score = 0;
        UpdateScoreText(score);
        UpdateLivesText(lives);
        loseUI.SetActive(false);
        Time.timeScale = 1f;
        Respawn();
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = scoreString + score.ToString();
    }

    private void UpdateLivesText(int lives)
    {
        livesText.text = lives.ToString();
    }
}
