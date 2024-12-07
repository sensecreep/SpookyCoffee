using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // Дистанция взаимодействия
    public Text interactionUI; // UI элемент для подсказки (Text компонент)

    private GameObject currentObject = null; // Объект, на который смотрит игрок

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider.CompareTag("Interactable")) // Проверка на интерактивный объект
            {
                interactionUI.gameObject.SetActive(true); // Показать подсказку
                interactionUI.text = "Press E to pick up"; // Меняем текст подсказки
                currentObject = hit.collider.gameObject;

                if (Input.GetKeyDown(KeyCode.E)) // Если нажал E
                {
                    // Реализуйте логику подбора предмета тут, без взаимодействия с руке.
                }
            }
            else
            {
                interactionUI.gameObject.SetActive(false); // Скрыть подсказку
                currentObject = null;
            }
        }
        else
        {
            interactionUI.gameObject.SetActive(false); // Скрыть подсказку
            currentObject = null;
        }
    }
}
