using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float patienceTime = 10f; // ����� ����� �������� �������
    private float remainingPatience; // ���������� ����� ��������
    private Transform targetPoint; // ������� ����� ��� ��������
    private bool hasReceivedOrder = false;
    private bool isPatienceTimerRunning = false; // ������ ��������

    // ������������� ������� � ������� ������
    public void Initialize(Transform target)
    {
        targetPoint = target; // ���������, ���� ���������
        remainingPatience = patienceTime;
        isPatienceTimerRunning = false;
        Debug.Log($"{gameObject.name} initialized at position {targetPoint.position}");
    }

    private void Update()
    {
        // �������� � ������� �����
        if (targetPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, Time.deltaTime * 10f);

            // ���� �������� �����
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                OnReachTarget();
            }
        }
    }

    private void OnReachTarget()
    {
        // ���� ������ ������ `queuePoint1` � ��� ������ � �������
        if (!isPatienceTimerRunning && CustomerManager.Instance.IsFirstInQueue(this))
        {
            isPatienceTimerRunning = true;
            StartCoroutine(CheckPatience());
        }
    }

    public void MoveToPoint(Transform newTarget)
    {
        targetPoint = newTarget; // ��������� ���� ��������
        isPatienceTimerRunning = false; // ���������� ������ ��������
        Debug.Log($"{gameObject.name} moving to {newTarget.position}");
    }

    public void ReceiveOrder()
    {
        hasReceivedOrder = true;
        StopAllCoroutines(); // ������������� ������ ��������
        Leave();
    }

    private IEnumerator CheckPatience()
    {
        Debug.Log($"{gameObject.name} started patience timer.");
        while (remainingPatience > 0)
        {
            remainingPatience -= Time.deltaTime;
            yield return null;
        }

        // ���� �������� ����������� � ����� �� �������
        if (!hasReceivedOrder)
        {
            Debug.Log($"{gameObject.name} ran out of patience and left.");
            CustomerManager.Instance.RemoveCustomer(this); // ���������� �������� �������
            Leave();
        }
    }

    private void Leave()
    {
        Debug.Log($"{gameObject.name} leaving.");
        Destroy(gameObject); // ������� �������
    }

}
