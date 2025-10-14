using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticBlock : MonoBehaviour
{
    private float attractionForce = 3f;
    private float repulsionForce = 3f;
    private float activationRadius = 4f;
    [SerializeField] private bool isBlueBlock = true; // true - �������, false - �������

    private Color gizmoColor = Color.cyan;

    private bool isActive = false;
    [SerializeField] private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // ������������� ���� � ����������� �� ���� �����
        if (isBlueBlock)
        {
            gizmoColor = Color.cyan;
        }
        else
        {
            gizmoColor = new Color(1f, 0.5f, 0.5f); // ������-�������
        }
    }

    void Update()
    {
        CheckForInput();

        if (isActive && player != null)
        {
            ApplyMagneticForce();
        }
    }

    void CheckForInput()
    {
        if (player == null) Debug.Log("Plus404");

        if (player == null) return;

        // ��������� ���������� �� ������
        Vector3 playerTransformFix = player.transform.position;
        playerTransformFix.y += 0.7f;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransformFix);

        if (distanceToPlayer <= activationRadius)
        {
            if (isBlueBlock && Input.GetMouseButton(0)) // ��� ��� ��������
            {
                isActive = true;
            }
            else if (!isBlueBlock && Input.GetMouseButton(1)) // ��� ��� ��������
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
        else
        {
            isActive = false;
        }
    }

    void ApplyMagneticForce()
    {
        Vector3 playerTransformFix = player.transform.position;
        playerTransformFix.y += 0.7f;
        Vector2 direction = playerTransformFix - transform.position;
        float distance = direction.magnitude;

        if (distance > 0.1f) // ������ �� ������� �� ����
        {
            // ����������� �����������
            direction.Normalize();

            // ��������� ���� � ����������� �� ���� �����
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                if (isBlueBlock)
                {
                    // ���������� (���� ���������� � �����)
                    playerRb.AddForce(-direction * attractionForce, ForceMode2D.Force);
                }
                else
                {
                    // ������������ (���� ���������� �� �����)
                    playerRb.AddForce(direction * repulsionForce, ForceMode2D.Force);
                }
            }
        }
    }

    // ������������ ������� �������� � ���������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
