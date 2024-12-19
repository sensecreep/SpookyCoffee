using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float patienceTime = 10f; // Общее время терпения клиента
    private float remainingPatience; // Оставшееся время терпения
    private Transform targetPoint; // Целевая точка для движения
    private bool hasReceivedOrder = false;
    private bool isPatienceTimerRunning = false; // Таймер терпения

    // Инициализация клиента с целевой точкой
    public void Initialize(Transform target)
    {
        targetPoint = target; // Указываем, куда двигаться
        remainingPatience = patienceTime;
        isPatienceTimerRunning = false;
    }

    private void Update()
    {
        // Движение к целевой точке
        if (targetPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, Time.deltaTime * 10f);

            // Если достигли точки
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                OnReachTarget();
            }
        }
    }

    private void OnReachTarget()
    {
        // Если клиент достиг `queuePoint1` и это первый в очереди
        if (!isPatienceTimerRunning && CustomerManager.Instance.IsFirstInQueue(this))
        {
            isPatienceTimerRunning = true;
            StartCoroutine(CheckPatience());
        }
    }

    public void MoveToPoint(Transform newTarget)
    {
        targetPoint = newTarget; // Обновляем цель движения
        isPatienceTimerRunning = false; // Сбрасываем таймер терпения
        Debug.Log($"{gameObject.name} moving to {newTarget.position}");
    }

    public void ReceiveOrder()
    {
        hasReceivedOrder = true;
        StopAllCoroutines(); // Останавливаем таймер терпения
        Leave();
    }

    private IEnumerator CheckPatience()
    {
        while (remainingPatience > 0)
        {
            remainingPatience -= Time.deltaTime;
            yield return null;
        }

        // Если терпение закончилось и заказ не получен
        if (!hasReceivedOrder)
        {
            CustomerManager.Instance.RemoveCustomer(this); // Уведомляем менеджер очереди
            Money.subMoney();
            Leave();
        }
    }

    private void Leave()
    {
        Destroy(gameObject); // Удаляем клиента
    }

}
