using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform pointA; // Первая точка
    public Transform pointB; // Вторая точка
    public Transform pointC; // Альтернативная точка
    public float speed = 2f; // Скорость движения

    private Transform targetPoint; // Текущая цель
    private bool isWaiting = false; // Ожидает ли NPC
    private int ePressCount = 0; // Счетчик нажатий клавиши E
    public bool isPointBOccupied = false; // Занята ли точка B другим NPC

    void Start()
    {
        targetPoint = isPointBOccupied ? pointC : pointB; // Выбираем начальную цель
    }

    void Update()
    {
        if (!isWaiting)
        {
            MoveToTarget();
        }
        else
        {
            CheckPlayerInput();
        }
    }

    void MoveToTarget()
    {
        // Движение к целевой точке
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Проверка на достижение точки
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == pointB || targetPoint == pointC)
            {
                isWaiting = true; // Останавливаемся на второй или альтернативной точке
            }
            else
            {
                targetPoint = isPointBOccupied ? pointC : pointB; // Переключаем цель на точку B или C
            }
        }
    }

    void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ePressCount++; // Увеличиваем счетчик нажатий
            Debug.Log($"Клавиша E нажата {ePressCount} раз.");

            if (ePressCount >= 2)
            {
                ResetNPC(); // Сбрасываем состояние и продолжаем движение
            }
        }
    }

    void ResetNPC()
    {
        ePressCount = 0; // Сбрасываем счетчик нажатий
        isWaiting = false; // Продолжаем движение
        targetPoint = pointA; // Устанавливаем цель обратно на первую точку
        Debug.Log("NPC продолжает движение.");
    }
    public void SetPointBOccupied(bool occupied)
    {
        isPointBOccupied = occupied;
    }
}
