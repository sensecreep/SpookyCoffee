using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public string interactionMessage = "������� E"; // ����� ���������
    private bool isLookingAt = false; // ���� ��� ��������, ������� �� ����� �� ������

    private void OnMouseOver()
    {
        isLookingAt = true; // ���������� ����, ���� ����� ��������� �� ������
    }

    private void OnMouseExit()
    {
        isLookingAt = false; // �������� ����, ���� ����� �������� �������� �� ������
    }

    public bool IsLookingAt()
    {
        return isLookingAt; // ����� ��� �������� ��������� �������
    }
}
