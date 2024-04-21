using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class RubyController : MonoBehaviour
{

    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    public ParticleSystem damageEffect;

    public int score = 0;
    public GameObject scoreText;

    public GameObject gameOverText;
    
    bool gameOver;

    TextMeshProUGUI scoreText_text;
    TextMeshProUGUI gameOverText_text;

    // Quinones added the next 3 references
    bool isSlow;
    float slowTimer;    
    public float timeSlow = 2.0f;
    public OilSlick oS;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        scoreText_text = scoreText.GetComponent<TextMeshProUGUI>();
        gameOverText_text = gameOverText.GetComponent<TextMeshProUGUI>();
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // Quinones added these 2 if statements
        if (isSlow)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer < 0)
            {
                isSlow = false;
                oS.played = false;
            }
        }

        if (isSlow == false)
        {
            speed = 3;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        scoreText_text.text = "Fixed Robots: " + score.ToString() + "/2";

        if (currentHealth < 1)
        {
            gameOver = true;
            gameOverText.SetActive(true);
            gameOverText_text.text = "You lost! Press R to Restart!";
            speed = 0.0000000001f;
        }

        if (score == 2)
        {
            gameOver = true;
            gameOverText.SetActive(true);
            gameOverText_text.text = "You won! Game made by Group 30. Press R to Restart!";
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Hit");

            PlaySound(hitSound);
            Instantiate(damageEffect, transform.position, transform.rotation);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void Awake()
    {
        damageEffect.Stop();
    }

    public void ChangeScore(int scoreAmount)
    {  
        score = (score + scoreAmount);
    }   

    // Quinones added these two blocks of code
    public void ChangeSpeed(int speed2)
    {
        speed = speed2;

        if (speed == 1)
        {
            if (isSlow)
                return;

            isSlow = true;
            slowTimer = timeSlow;
        }
    }

    void slowAhh()
    {
        if (speed == 1)
        {
            isSlow = true;
        }
    }

    // Leana added this code
    public void PowerUp(int invincibleTime)
    {
        invincibleTimer = invincibleTime;
        isInvincible = true;
    }
}