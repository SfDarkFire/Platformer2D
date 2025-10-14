using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VictoryDoor : MonoBehaviour
{
    [Header("Настройки двери")]
    private float activationDistance = 2f; // Дистанция активации
    private KeyCode interactKey = KeyCode.E; // Клавиша взаимодействия

    [Header("Меню победы")]
    [SerializeField] private GameObject victoryMenu; // Ссылка на UI меню победы

    private GameObject player;
    private Hero playerController;
    private bool playerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<Hero>();
        }

        // Скрываем меню победы при старте
        victoryMenu = GameObject.FindGameObjectWithTag("Background").transform.GetChild(2).gameObject;
        if (victoryMenu != null)
        {
            victoryMenu.SetActive(false);
        }
    }

    void Update()
    {
        CheckPlayerDistance();

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ActivateVictory();
        }
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        playerInRange = distance <= activationDistance;

        // Здесь можно добавить визуальную подсказку (подсветку двери)
        if (playerInRange)
        {
            // Дверь подсвечивается когда игрок рядом
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void ActivateVictory()
    {
        Debug.Log("Victory1");
        // Активируем меню победы
        if (victoryMenu != null)
        {
            victoryMenu.SetActive(true);
        }

        // Отключаем управление игроком
        if (playerController != null)
        {
            playerController.SetControl(false);
        }

        // Включаем курсор
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        player.SetActive(false);

        // Останавливаем время (опционально)
        Time.timeScale = 0f;
    }

    // Визуализация радиуса активации в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Альтернативный способ через триггер
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
