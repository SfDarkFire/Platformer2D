using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float jumpForse = 8.0f;
    private int currentColorState = 0;
    private bool isGrounded = true;
    private bool canMove = true;

    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir,speed*Time.deltaTime);

        rbSprite.flipX = dir.x<0.0f;
    }

    private void colorChange()
    {
        if (Input.GetMouseButton(0)) // ЛКМ
        {
            currentColorState = 1;
        }
        else if (Input.GetMouseButton(1)) // ПКМ
        {
            currentColorState = 2;
        }
        else
        {
            currentColorState = 0;
        }

        animator.SetInteger("colorState", currentColorState);
    }

    private void motionTracking()
    {
        bool isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
        animator.SetBool("isMoving", isMoving);
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = col.Length > 1;
    }

    public void SetControl(bool controlEnabled)
    {
        canMove = controlEnabled;

        if (!controlEnabled)
        {
            // Останавливаем движение
            rb.velocity = Vector2.zero;
        }
    }

    public void PauseGame()
    {
        if (canMove)
        {
            SetControl(false);
            GameObject.FindGameObjectWithTag("Background").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Background").transform.GetChild(3).gameObject.SetActive(true);
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            SetControl(true);
            GameObject.FindGameObjectWithTag("Background").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Background").transform.GetChild(3).gameObject.SetActive(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        if (!canMove) return;
        colorChange();
        motionTracking();
        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }
}
