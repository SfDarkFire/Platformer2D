using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("���� ������")]
    [SerializeField] private GameObject deathMenu;

    [Header("���������")]
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
            Debug.Log("����� ����� � ���� ������");

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
            Debug.Log("����� ���������� � ����� ������");

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
        // ��������� ���������� �������
        Hero playerController = player.GetComponent<Hero>();
        if (playerController != null)
        {
            playerController.SetControl(false);
        }

        // ������������� �������� ������
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        // �������� ������ (�����������)
        // player.SetActive(false);

        // ���������� ���� ������
        player.SetActive(false);

        ShowDeathMenu();

        Debug.Log("����� ����");
    }

    private void ShowDeathMenu()
    {
        if (deathMenu != null)
        {
            deathMenu.SetActive(true);
            GameObject.FindGameObjectWithTag("Background").transform.GetChild(0).gameObject.SetActive(false);

            // �������� ������
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // ������������� �����
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning("DeathMenu �� �������� � ����������!");
        }
    }

    // ������������ � ���������
    private void OnDrawGizmos()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // ������� � �������������
            Gizmos.DrawCube(transform.position, collider.bounds.size);
        }
    }
}
