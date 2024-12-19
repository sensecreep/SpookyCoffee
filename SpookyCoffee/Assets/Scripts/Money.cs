using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Money : MonoBehaviour
{
    static int money = 0;
    public TextMesh interactionUI;
    static public void addMoney()
    {
        money += 10;
    }
    static public void subMoney()
    {
        money -= 5;
    }
    void Start()
    {
        
    }
    void Update()
    {
        interactionUI.text = money.ToString();
    }
}
