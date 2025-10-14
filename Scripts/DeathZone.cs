using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Меню смерти")]
    [SerializeField] private GameObject deathMenu;

    [Header("Настройки")]
    [SerializeField] private bool killImmediately = true;
    [SerializeField] private float killDelay = 0f;

    private void Start()
    {
        deathMenu = GameObject.FindGameObjectWithTag("Background").transform.GetChild(1).gameObject;
        if (deathMenu != null)
        {
            deathMenu.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок попал в зону смерти");

            if (killImmediately)
            {
                KillPlayer(other.gameObject);
            }
            else
            {
                Invoke("KillPlayer", killDelay);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Игрок столкнулся с зоной смерти");

            if (killImmediately)
            {
                KillPlayer(collision.gameObject);
            }
            else
            {
                Invoke("KillPlayer", killDelay);
            }
        }
    }

    private void KillPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            KillPlayer(player);
        }
    }

    private void KillPlayer(GameObject player)
    {
        // Отключаем управление игроком
        Hero playerController = player.GetComponent<Hero>();
        if (playerController != null)
        {
            playerController.SetControl(false);
        }

        // Останавливаем движение игрока
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        // Скрываем игрока (опционально)
        // player.SetActive(false);

        // Показываем меню смерти
        player.SetActive(false);

        ShowDeathMenu();

        Debug.Log("Игрок умер");
    }

    private void ShowDeathMenu()
    {
        if (deathMenu != null)
        {
            deathMenu.SetActive(true);
            GameObject.FindGameObjectWithTag("Background").transform.GetChild(0).gameObject.SetActive(false);

            // Включаем курсор
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Останавливаем время
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning("DeathMenu не назначен в инспекторе!");
        }
    }

    // Визуализация в редакторе
    private void OnDrawGizmos()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // Красный с прозрачностью
            Gizmos.DrawCube(transform.position, collider.bounds.size);
        }
    }
}
