using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance; // Синглтон для глобального доступа

    public GameObject customerPrefab; // Префаб клиента
    public Transform spawnPoint; // Точка спавна клиентов
    public List<Transform> queuePoints; // Очередь (точки ожидания)

    public float spawnInterval = 5f; // Интервал спавна новых клиентов
    private Queue<Customer> customerQueue = new Queue<Customer>(); // Очередь клиентов
    private int maxCustomers = 3; // Максимальное количество клиентов в очереди

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
        // Создаём нового клиента
        GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        Customer customer = customerObj.GetComponent<Customer>();

        // Найти первую свободную точку в очереди
        if (customerQueue.Count < queuePoints.Count)
        {
            Transform queuePosition = queuePoints[customerQueue.Count];
            customer.Initialize(queuePosition);
            customerQueue.Enqueue(customer);
            Debug.Log($"Customer spawned and assigned to queue point {queuePoints[customerQueue.Count - 1].position}");
        }
        else
        {
            Destroy(customerObj); // Если очередь полна
            Debug.Log("Customer couldn't be spawned, queue is full.");
        }
    }

    public void ServeNextCustomer()
    {
        if (customerQueue.Count > 0)
        {
            // Обслуживаем первого клиента
            Customer servedCustomer = customerQueue.Dequeue();
            servedCustomer.ReceiveOrder();

            // Перемещаем остальных клиентов вперёд
            ReorganizeQueue();
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        if (customerQueue.Contains(customer))
        {
            // Убираем клиента из очереди
            List<Customer> tempQueue = new List<Customer>(customerQueue);
            tempQueue.Remove(customer);
            customerQueue = new Queue<Customer>(tempQueue);

            Debug.Log($"Customer {customer.gameObject.name} removed from queue.");
            ReorganizeQueue(); // Перемещаем остальных
        }
    }

    private void ReorganizeQueue()
    {
        Debug.Log("Reorganizing queue...");
        int index = 0;
        foreach (Customer customer in customerQueue)
        {
            customer.MoveToPoint(queuePoints[index]); // Переместить клиента на следующую точку
            Debug.Log($"Customer moved to queue point {queuePoints[index].position}");
            index++;
        }
    }

    // Проверка: является ли клиент первым в очереди
    public bool IsFirstInQueue(Customer customer)
    {
        if (customerQueue.Count > 0)
        {
            return customerQueue.Peek() == customer; // Первый клиент в очереди
        }
        return false;
    }
}

