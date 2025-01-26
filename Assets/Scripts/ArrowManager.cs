using UnityEngine;
using System.Collections.Generic;

public class ArrowManager : MonoBehaviour
{
    public GameObject arrowPrefab; // ������ �������
    public Canvas canvas; // Canvas, � ������� ����� ������������ �������
    public ItemSpawner itemSpawner; // ������ �� ������, ������� ������� ��������

    private List<ItemArrowPair> itemArrowPairs = new List<ItemArrowPair>();

    void Start()
    {
        if (itemSpawner != null)
        {
            itemSpawner.OnItemSpawned += HandleItemSpawned;
        }
    }

    void Update()
    {
        // ��������� ������ �������
        foreach (var pair in itemArrowPairs)
        {
            if (pair.Item == null)
            {
                // ���� ������� ���������, ������� ��� �������
                Destroy(pair.Arrow.gameObject);
                itemArrowPairs.Remove(pair);
                break;
            }

            UpdateArrow(pair);
        }
    }

    private void HandleItemSpawned(Transform itemTransform)
    {
        // ������� ������� ��� ������ �������
        GameObject newArrow = Instantiate(arrowPrefab, canvas.transform);
        ItemArrowPair newPair = new ItemArrowPair { Item = itemTransform, Arrow = newArrow.GetComponent<RectTransform>() };
        itemArrowPairs.Add(newPair);
    }

    private void UpdateArrow(ItemArrowPair pair)
    {
        Transform item = pair.Item;
        RectTransform arrow = pair.Arrow;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(item.position);

        // ���� ������ �� ������, �������� �������
        if (screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)
        {
            arrow.gameObject.SetActive(false);
            return;
        }
        else
        {
            arrow.gameObject.SetActive(true);
        }

        // ������������ ��������� ������� ������ ������
        screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
        screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

        // ���������� ������� � ������ �����
        arrow.position = screenPos;

        // ������������ ����������� � ��������
        Vector3 direction = (item.position - Camera.main.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.rotation = Quaternion.Euler(0, 0, angle - 90);

        // ��������� �������
        float pulseSpeed = 2f;
        float minScale = 0.8f;
        float maxScale = 1.2f;
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * pulseSpeed, 1f));
        arrow.localScale = Vector3.one * scale;
    }

    private class ItemArrowPair
    {
        public Transform Item;
        public RectTransform Arrow;
    }
}

