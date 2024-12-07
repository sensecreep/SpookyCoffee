using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // ��������� ��������������
    public Text interactionUI; // UI ������� ��� ��������� (Text ���������)

    private GameObject currentObject = null; // ������, �� ������� ������� �����

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider.CompareTag("Interactable")) // �������� �� ������������� ������
            {
                interactionUI.gameObject.SetActive(true); // �������� ���������
                interactionUI.text = "Press E to pick up"; // ������ ����� ���������
                currentObject = hit.collider.gameObject;

                if (Input.GetKeyDown(KeyCode.E)) // ���� ����� E
                {
                    // ���������� ������ ������� �������� ���, ��� �������������� � ����.
                }
            }
            else
            {
                interactionUI.gameObject.SetActive(false); // ������ ���������
                currentObject = null;
            }
        }
        else
        {
            interactionUI.gameObject.SetActive(false); // ������ ���������
            currentObject = null;
        }
    }
}
