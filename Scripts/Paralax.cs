using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    
    // Насколько сильно будет смещаться фон (0 = не движется, 1 = движется как земля).
    // Для дальнего фона используйте малые значения, e.g., 0.1f.
    [SerializeField] private float parallaxEffectX = 0.5f;
    [SerializeField] private float parallaxEffectY = 0.5f;

    // Эти переменные нужны для бесконечного повторения
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        // Получаем ширину и высоту нашего спрайта
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
        textureUnitSizeY = (texture.height / sprite.pixelsPerUnit) * transform.localScale.y;
    }

    void LateUpdate()
    {
        // Насколько камера сдвинулась с прошлого кадра
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Двигаем фон
        transform.position += new Vector3(deltaMovement.x * parallaxEffectX, deltaMovement.y * parallaxEffectY, 0);
        
        // Обновляем позицию камеры для следующего кадра
        lastCameraPosition = cameraTransform.position;

        // --- Логика для "бесконечного" фона ---

        // Проверяем, не ушла ли камера слишком далеко по X
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            // Если да, "телепортируем" фон, чтобы он оказался перед камерой
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x - offsetPositionX, transform.position.y, transform.position.z);
        }

        // То же самое для Y (если у вас вертикальный скроллинг)
        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        {
            float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y - offsetPositionY, transform.position.z);
        }
    }
}