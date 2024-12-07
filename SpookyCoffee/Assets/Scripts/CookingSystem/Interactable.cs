using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public string interactionMessage = "Нажмите E"; // Текст подсказки
    private bool isLookingAt = false; // Флаг для проверки, смотрит ли игрок на объект

    private void OnMouseOver()
    {
        isLookingAt = true; // Установить флаг, если игрок наводится на объект
    }

    private void OnMouseExit()
    {
        isLookingAt = false; // Сбросить флаг, если игрок перестал смотреть на объект
    }

    public bool IsLookingAt()
    {
        return isLookingAt; // Метод для проверки состояния взгляда
    }
}
