using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // [SerializeField] private float speed = 6.0f; // Переименовали в maxSpeed
    //[SerializeField] private float maxSpeed = 6.0f; // Максимальная скорость от *ввода* игрока
    //[SerializeField] private float moveAcceleration = 50f; // Сила, с которой игрок разгоняется
    //[SerializeField] private float jumpForse = 8.0f;
    [Header("Настройки Движения")]
    [SerializeField] private float maxSpeed = 6.0f; // Макс. скорость (остается)
    [SerializeField] private float jumpForse = 20000.0f; // Сила прыжка (остается)

    [Header("Настройки Земли")]
    [SerializeField] private float groundAcceleration = 2000f; // Как быстро разгоняемся на земле
    [SerializeField] private float groundLinearDrag = 0f;    // Трение на земле (чтобы не скользить)

    [Header("Настройки Воздуха")]
    [SerializeField] private float airAcceleration = 30f;    // Как быстро разгоняемся в воздухе (меньше!)
    [SerializeField] private float airLinearDrag = 0f;       // Трение в воздухе (чтобы не "плыть")
    
    private int currentColorState = 0;
    private bool isGrounded = true;
    private bool canMove = true;

    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;

    // Флаг для прыжка, чтобы он срабатывал в FixedUpdate
    private bool jumpRequested = false; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // *** ИЗМЕНЕНО ***
    // Старый метод Run() удален. Вместо него новый метод Move()
private void Move()
    {
        // 1. Получаем ввод (от -1 до 1)
        float moveInput = Input.GetAxis("Horizontal");

        // *** ИЗМЕНЕНО ***
        // 2. Выбираем ускорение в зависимости от состояния
        float currentAcceleration = isGrounded ? groundAcceleration : airAcceleration;
        // *** КОНЕЦ ИЗМЕНЕНИЯ ***


        // 3. Рассчитываем силу для применения
        float horizontalForce = moveInput * currentAcceleration; // Используем новое ускорение

        // 4. Применяем силу
        // (Эта логика остается прежней, она хорошая,
        // т.к. не мешает магнитам разгонять нас сильнее maxSpeed)
        if (moveInput > 0 && rb.velocity.x < maxSpeed) // Идем вправо
        {
            rb.AddForce(new Vector2(horizontalForce, 0f), ForceMode2D.Force);
        }
        else if (moveInput < 0 && rb.velocity.x > -maxSpeed) // Идем влево
        {
            rb.AddForce(new Vector2(horizontalForce, 0f), ForceMode2D.Force);
        }

        // 5. Поворот спрайта (только если есть ввод)
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            rbSprite.flipX = moveInput < 0.0f;
        }
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

    // *** ИЗМЕНЕНО ***
    private void motionTracking()
    {
        // Анимация должна зависеть от *реальной* скорости, а не от *нажатия*
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;
        animator.SetBool("isMoving", isMoving);
    }

    private void Jump()
    {
        // *** ВОТ РЕШЕНИЕ: ***
        // Мы принудительно устанавливаем НИЗКОЕ (воздушное) трение
        // за мгновение до самого прыжка.
        rb.drag = airLinearDrag;

        // Сбрасываем вертикальную скорость для стабильного прыжка
        rb.velocity = new Vector2(rb.velocity.x, 0); 
        
        // Применяем силу
        rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
        
        // Сбрасываем флаг
        jumpRequested = false; 
    }
    private void CheckGround()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        isGrounded = col.Length > 1;
    }

    public void SetControl(bool controlEnabled)
    {
        canMove = controlEnabled;

        if (!controlEnabled)
        {
            // Останавливаем движение (можно оставить, если нужно)
            // rb.velocity = Vector2.zero; // Раскомментируй, если нужно мгновенно стопорить
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

    // *** ИЗМЕНЕНО ***
private void FixedUpdate()
    {
        CheckGround(); // Сначала проверяем, на земле ли мы

        // *** НОВЫЙ БЛОК ***
        // Динамически меняем трение (Linear Drag)
        if (isGrounded)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = airLinearDrag;
        }
        // *** КОНЕЦ НОВОГО БЛОКА ***

        if (!canMove) return;

        // Вызываем движение (применение силы)
        Move();

        // Выполняем прыжок, если он был запрошен в Update
        if (jumpRequested)
        {
            Jump();
        }

    }
    // *** ИЗМЕНЕНО ***
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        
        // Обновляем анимацию и цвет в Update (это не физика)
        if (canMove)
        {
            colorChange();
            motionTracking();
        }
        else
        {
            // Если двигаться нельзя, сбрасываем анимацию движения
            animator.SetBool("isMoving", false);
        }

        // Проверка ввода для прыжка остается в Update,
        // так как GetButtonDown лучше ловить здесь.
        if (canMove && isGrounded && Input.GetButtonDown("Jump"))
        {
            // Мы не прыгаем сразу, а "запрашиваем" прыжок
            jumpRequested = true; 
        }
        
        // if (Input.GetButton("Horizontal")) Run(); // <--- УДАЛЕНО
    }
}