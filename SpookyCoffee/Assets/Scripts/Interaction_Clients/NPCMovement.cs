using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform pointA; // ������ �����
    public Transform pointB; // ������ �����
    public Transform pointC; // �������������� �����
    public float speed = 2f; // �������� ��������

    private Transform targetPoint; // ������� ����
    private bool isWaiting = false; // ������� �� NPC
    private int ePressCount = 0; // ������� ������� ������� E
    public bool isPointBOccupied = false; // ������ �� ����� B ������ NPC

    void Start()
    {
        targetPoint = isPointBOccupied ? pointC : pointB; // �������� ��������� ����
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
        // �������� � ������� �����
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // �������� �� ���������� �����
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == pointB || targetPoint == pointC)
            {
                isWaiting = true; // ��������������� �� ������ ��� �������������� �����
            }
            else
            {
                targetPoint = isPointBOccupied ? pointC : pointB; // ����������� ���� �� ����� B ��� C
            }
        }
    }

    void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ePressCount++; // ����������� ������� �������
            Debug.Log($"������� E ������ {ePressCount} ���.");

            if (ePressCount >= 2)
            {
                ResetNPC(); // ���������� ��������� � ���������� ��������
            }
        }
    }

    void ResetNPC()
    {
        ePressCount = 0; // ���������� ������� �������
        isWaiting = false; // ���������� ��������
        targetPoint = pointA; // ������������� ���� ������� �� ������ �����
        Debug.Log("NPC ���������� ��������.");
    }
    public void SetPointBOccupied(bool occupied)
    {
        isPointBOccupied = occupied;
    }
}
