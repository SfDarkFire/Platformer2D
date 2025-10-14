using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VictoryDoor : MonoBehaviour
{
    [Header("��������� �����")]
    private float activationDistance = 2f; // ��������� ���������
    private KeyCode interactKey = KeyCode.E; // ������� ��������������

    [Header("���� ������")]
    [SerializeField] private GameObject victoryMenu; // ������ �� UI ���� ������

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

        // �������� ���� ������ ��� ������
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

        // ����� ����� �������� ���������� ��������� (��������� �����)
        if (playerInRange)
        {
            // ����� �������������� ����� ����� �����
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
        // ���������� ���� ������
        if (victoryMenu != null)
        {
            victoryMenu.SetActive(true);
        }

        // ��������� ���������� �������
        if (playerController != null)
        {
            playerController.SetControl(false);
        }

        // �������� ������
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        player.SetActive(false);

        // ������������� ����� (�����������)
        Time.timeScale = 0f;
    }

    // ������������ ������� ��������� � ���������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �������������� ������ ����� �������
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
