using UnityEngine;
using UnityEngine.UI;

public class InteractionPointSpawner : MonoBehaviour
{
    public GameObject pointPrefab; // ������ ��� �������� �����
    public LayerMask InterLayerPoint; // ����� ���� ��� ������������� ��������
    public Text interactionUI; // ����� ��� ���������

    private GameObject currentPoint; // ������� �����

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward); // ��� �� ������
        RaycastHit hit;

        // �������� Raycast � ���������, ����� �� ��� � ������ �� ���� InterLayerPoint
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, InterLayerPoint))
        {
            // ���� ������ �� ������� �� �������, ����� ��� Interactable
            if (hit.collider.CompareTag("Interactable"))
            {
                // ���� ����� ��� �� �������
                if (currentPoint == null)
                {
                    SpawnPoint(hit.point); // ������� �����
                    ShowInteractionText(true); // ���������� �����
                }
                else
                {
                    // ���� ����� ����������, ��������� �� �������
                    currentPoint.transform.position = hit.point;
                }
            }
        }
        else
        {
            // ���� ��� �� ����� � ������������� ������
            if (currentPoint != null)
            {
                Destroy(currentPoint); // ���������� �����
                ShowInteractionText(false); // �������� �����
            }
        }
    }

    // ����� ��� �������� �����
    void SpawnPoint(Vector3 position)
    {
        // ��������: ���� ������ �����������, ������� ������
        if (pointPrefab == null)
        {
            return;
        }

        // ������� ����� �� ������� ��������� ����
        currentPoint = Instantiate(pointPrefab, position, Quaternion.identity);
    }

    // ����� ��� ������/������� ������ ���������
    void ShowInteractionText(bool show)
    {
        if (interactionUI != null)
        {
            interactionUI.gameObject.SetActive(show); // ���������� ��� �������� UI ������� � �������
            if (show)
            {
                interactionUI.text = "Press E"; // ����� ����� ���������
            }
        }
    }
}
