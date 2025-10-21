using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    [SerializeField] private float parallaxEffectX = 0.5f;
    [SerializeField] private float parallaxEffectY = 0.5f;

    private float textureUnitSizeX;
    private float textureUnitSizeY;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        Image image = GetComponent<Image>();
        if (image != null && image.sprite != null)
        {
            Sprite sprite = image.sprite;
            Texture2D texture = sprite.texture;
            if (texture != null)
            {
                // Для UI элементов используем rectTransform вместо transform.localScale
                RectTransform rectTransform = GetComponent<RectTransform>();
                textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * rectTransform.localScale.x;
                textureUnitSizeY = (texture.height / sprite.pixelsPerUnit) * rectTransform.localScale.y;

                // Защита от нулевых значений
                if (textureUnitSizeX <= 0) textureUnitSizeX = 1f;
                if (textureUnitSizeY <= 0) textureUnitSizeY = 1f;
            }
            else
            {
                Debug.LogError("Texture is null on sprite: " + sprite.name);
                textureUnitSizeX = textureUnitSizeY = 100f; // значения по умолчанию для UI
            }
        }
        else
        {
            Debug.LogError("Image or Sprite is null on object: " + gameObject.name);
            textureUnitSizeX = textureUnitSizeY = 100f; // значения по умолчанию для UI
        }
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectX, deltaMovement.y * parallaxEffectY, 0);
        lastCameraPosition = cameraTransform.position;

        // Проверяем на валидность перед вычислениями
        if (textureUnitSizeX > 0 && Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x - offsetPositionX, transform.position.y, transform.position.z);
        }

        // Добавляем проверку на валидность textureUnitSizeY
        if (textureUnitSizeY > 0 && Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        {
            float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
            // Дополнительная проверка на NaN
            if (!float.IsNaN(offsetPositionY))
            {
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y - offsetPositionY, transform.position.z);
            }
        }
    }
}