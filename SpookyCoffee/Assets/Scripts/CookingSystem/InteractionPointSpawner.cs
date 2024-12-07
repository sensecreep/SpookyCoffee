using UnityEngine;
using UnityEngine.UI;

public class InteractionPointSpawner : MonoBehaviour
{
    public GameObject pointPrefab; // Префаб для создания точки
    public LayerMask InterLayerPoint; // Маска слоя для интерактивных объектов
    public Text interactionUI; // Текст для подсказки

    private GameObject currentPoint; // Текущая точка

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward); // Луч из камеры
        RaycastHit hit;

        // Проводим Raycast и проверяем, попал ли луч в объект на слое InterLayerPoint
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, InterLayerPoint))
        {
            // Если объект на который мы смотрим, имеет тег Interactable
            if (hit.collider.CompareTag("Interactable"))
            {
                // Если точка еще не создана
                if (currentPoint == null)
                {
                    SpawnPoint(hit.point); // Создаем точку
                    ShowInteractionText(true); // Показываем текст
                }
                else
                {
                    // Если точка существует, обновляем ее позицию
                    currentPoint.transform.position = hit.point;
                }
            }
        }
        else
        {
            // Если луч не попал в интерактивный объект
            if (currentPoint != null)
            {
                Destroy(currentPoint); // Уничтожаем точку
                ShowInteractionText(false); // Скрываем текст
            }
        }
    }

    // Метод для создания точки
    void SpawnPoint(Vector3 position)
    {
        // Проверка: если префаб отсутствует, выводим ошибку
        if (pointPrefab == null)
        {
            return;
        }

        // Спавним точку на позиции попадания луча
        currentPoint = Instantiate(pointPrefab, position, Quaternion.identity);
    }

    // Метод для показа/скрытия текста подсказки
    void ShowInteractionText(bool show)
    {
        if (interactionUI != null)
        {
            interactionUI.gameObject.SetActive(show); // Показываем или скрываем UI элемент с текстом
            if (show)
            {
                interactionUI.text = "Press E"; // Пишем текст подсказки
            }
        }
    }
}
