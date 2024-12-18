using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance; // �������� ��� ����������� �������

    public GameObject customerPrefab; // ������ �������
    public Transform spawnPoint; // ����� ������ ��������
    public List<Transform> queuePoints; // ������� (����� ��������)

    public float spawnInterval = 5f; // �������� ������ ����� ��������
    private Queue<Customer> customerQueue = new Queue<Customer>(); // ������� ��������
    private int maxCustomers = 3; // ������������ ���������� �������� � �������

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            if (customerQueue.Count < maxCustomers)
            {
                SpawnCustomer();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCustomer()
    {
        // ������ ������ �������
        GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        Customer customer = customerObj.GetComponent<Customer>();

        // ����� ������ ��������� ����� � �������
        if (customerQueue.Count < queuePoints.Count)
        {
            Transform queuePosition = queuePoints[customerQueue.Count];
            customer.Initialize(queuePosition);
            customerQueue.Enqueue(customer);
            Debug.Log($"Customer spawned and assigned to queue point {queuePoints[customerQueue.Count - 1].position}");
        }
        else
        {
            Destroy(customerObj); // ���� ������� �����
            Debug.Log("Customer couldn't be spawned, queue is full.");
        }
    }

    public void ServeNextCustomer()
    {
        if (customerQueue.Count > 0)
        {
            // ����������� ������� �������
            Customer servedCustomer = customerQueue.Dequeue();
            servedCustomer.ReceiveOrder();

            // ���������� ��������� �������� �����
            ReorganizeQueue();
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        if (customerQueue.Contains(customer))
        {
            // ������� ������� �� �������
            List<Customer> tempQueue = new List<Customer>(customerQueue);
            tempQueue.Remove(customer);
            customerQueue = new Queue<Customer>(tempQueue);

            Debug.Log($"Customer {customer.gameObject.name} removed from queue.");
            ReorganizeQueue(); // ���������� ���������
        }
    }

    private void ReorganizeQueue()
    {
        Debug.Log("Reorganizing queue...");
        int index = 0;
        foreach (Customer customer in customerQueue)
        {
            customer.MoveToPoint(queuePoints[index]); // ����������� ������� �� ��������� �����
            Debug.Log($"Customer moved to queue point {queuePoints[index].position}");
            index++;
        }
    }

    // ��������: �������� �� ������ ������ � �������
    public bool IsFirstInQueue(Customer customer)
    {
        if (customerQueue.Count > 0)
        {
            return customerQueue.Peek() == customer; // ������ ������ � �������
        }
        return false;
    }
}

