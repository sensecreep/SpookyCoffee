using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // Дистанция взаимодействия
    public TMP_Text interactionUI; // UI элемент для подсказки (Text компонент)

    private GameObject currentObject = null; // Объект, на который смотрит игрок
    bool isEq = false;
    GameObject coffeeCup;
    GameObject coffee;
    GameObject milk;
    GameObject holder;
    GameObject pitch;
    GameObject cup;
    GameObject holderInCoffee;
    GameObject pitchInCoffee;

    private void Start()
    {
        coffeeCup = GameObject.Find("CoffeeCup");
        coffeeCup.SetActive(false);
        coffee = GameObject.Find("Coffee");
        coffee.SetActive(false);
        milk = GameObject.Find("Milk");
        milk.SetActive(false);
        holder = GameObject.Find("Holder");
        holder.SetActive(false);
        pitch = GameObject.Find("Pitch");
        pitch.SetActive(false);
        cup = GameObject.Find("Cup");
        cup.SetActive(false);
        holderInCoffee = GameObject.Find("HolderInCoffee");
        holderInCoffee.SetActive(false);
        pitchInCoffee = GameObject.Find("PitchInCoffee");
        pitchInCoffee.SetActive(false);
    }

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

                if (Input.GetKeyDown(KeyCode.E) && !isEq) // Если нажал E
                {
                    PickUp(currentObject);
                }
            }
            else if (hit.collider.CompareTag("Customer")) // Проверка на интерактивный объект
            {
                if (Input.GetKeyDown(KeyCode.E) && coffeeCup.activeSelf)
                {
                    CoffeeServe(hit.collider.gameObject);
                }
            }
            else if (hit.collider.CompareTag("Machine")) // Проверка на интерактивный объект
            {
                interactionUI.gameObject.SetActive(true); // Показать подсказку
                interactionUI.text = "Press E to interact"; // Меняем текст подсказки
                currentObject = hit.collider.gameObject;
                Debug.Log(currentObject);

                if (Input.GetKeyDown(KeyCode.E) && isEq) // Если нажал E
                {
                    Inter(currentObject);
                }
            }
            else
            {
                interactionUI.gameObject.SetActive(false); // Скрыть подсказку
                currentObject = null;
            }

        }
        if (Input.GetKeyDown(KeyCode.Q) && isEq)
        {
            Drop();
        }
    }
    void CoffeeServe(GameObject cust)
    {
        isEq = false;
        coffeeCup.SetActive(false);
        Customer customer = cust.GetComponent<Customer>();
        CustomerManager.Instance.RemoveCustomer(customer);
        Money.addMoney();
        Destroy(cust);
    }
    void PickUp(GameObject obj)
    {
        isEq = true;
        if (obj.name == "CoffeeStuck")
        {
            coffee.SetActive(true);
        }
        else if (obj.name == "MilkStuck")
        {
            milk.SetActive(true);
        }
        else if (obj.name == "CupTable")
        {
            cup.SetActive(true);
        }

    }
    void Inter(GameObject obj)
    {
        if (obj.name == "HolderTable" && coffee.activeSelf)
        {
            coffee.SetActive(false);
            holder.SetActive(true);
        }
        else if (obj.name == "PitchTable" && milk.activeSelf)
        {
            milk.SetActive(false);
            pitch.SetActive(true);
        }
        else if (obj.name == "coffeegrinder" && holder.activeSelf)
        {
            isEq = false;
            holder.SetActive(false);
            holderInCoffee.SetActive(true);
        }
        else if (obj.name == "CoffeeMachine" && pitch.activeSelf)
        {
            isEq = false;
            pitch.SetActive(false);
            pitchInCoffee.SetActive(true);
        }
        else if (obj.name == "coffeegrinder" && cup.activeSelf)
        {
            holderInCoffee.SetActive(false);
        }
        else if (obj.name == "CoffeeMachine" && cup.activeSelf && !holderInCoffee.activeSelf)
        {
            pitchInCoffee.SetActive(false);
            cup.SetActive(false);
            coffeeCup.SetActive(true);
        }
    }
    void Drop()
    {
        coffeeCup.SetActive(false);
        coffee.SetActive(false);
        milk.SetActive(false);
        holder.SetActive(false);
        pitch.SetActive(false);
        cup.SetActive(false);
        isEq = false;
    }
     
}
