using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private bool isGameOver = false;
    public PlayerController player;
    public Slider hpBar;

    private float dotDamageInterval = 0.1f;
    private float dotDamageTimer = 0f;

    private void OnEnable()
    {
        player.OnScoreChanged += AddScore;
        player.OnGameOver += PlayerDead;
        player.OnHpChanged += HpUpdate;
    }
    private void OnDisable()
    {
        player.OnScoreChanged -= AddScore;
        player.OnGameOver -= PlayerDead;
        player.OnHpChanged -= HpUpdate;
    }
    void Start()
    {
        gameOverText.SetActive(false);
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            dotDamageTimer += Time.deltaTime;
            if (dotDamageTimer >= dotDamageInterval)
            {
                dotDamageTimer = 0f;
                player.TakeDamage(0.2f);
            }
        }
    }

    public void PlayerDead()
    {
        isGameOver = true;
        gameOverText.SetActive(true);
    }

    public void AddScore(int amount)
    {
        scoreText.text = $"SCORE : {amount}";
    }

    public void HpUpdate(float hp)
    {
        if (hpBar != null)
            hpBar.value = hp;
        if (hp == 0)
            hpBar.value = 0f;
    }
}
