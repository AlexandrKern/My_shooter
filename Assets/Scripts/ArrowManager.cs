using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // ���������� DOTween

public class ArrowManager : MonoBehaviour
{
    [System.Serializable]
    public class ArrowPrefabMapping
    {
        public string ItemTag; // ��� ��������
        public GameObject ArrowPrefab; // ������ ������� ��� ����� ��������
    }

    public List<ArrowPrefabMapping> arrowPrefabMappings; // ������ ����� � ��������������� ��������
    public Canvas canvas; // Canvas, � ������� ����� ������������ �������
    public ItemSpawner itemSpawner; // ������ �� ������, ������� ������� ��������

    private Dictionary<string, GameObject> arrowPrefabsByTag = new Dictionary<string, GameObject>();
    private List<ItemArrowPair> itemArrowPairs = new List<ItemArrowPair>();

    void Start()
    {
        // ��������� ������� �������� �� �����
        foreach (var mapping in arrowPrefabMappings)
        {
            if (!arrowPrefabsByTag.ContainsKey(mapping.ItemTag))
            {
                arrowPrefabsByTag[mapping.ItemTag] = mapping.ArrowPrefab;
            }
        }

        if (itemSpawner != null)
        {
            itemSpawner.OnItemSpawned += HandleItemSpawned;
        }
    }

    void Update()
    {
        for (int i = itemArrowPairs.Count - 1; i >= 0; i--)
        {
            var pair = itemArrowPairs[i];
            if (pair.Item == null)
            {
                Destroy(pair.Arrow.gameObject);
                itemArrowPairs.RemoveAt(i);
                continue;
            }

            UpdateArrow(pair);
        }
    }

    private void HandleItemSpawned(Transform itemTransform)
    {

        string itemTag = itemTransform.tag; // �������� ��� ��������

        if (arrowPrefabsByTag.TryGetValue(itemTag, out GameObject arrowPrefab))
        {
            // ������� ������� ��� ������ �������
            GameObject newArrow = Instantiate(arrowPrefab, canvas.transform);
            RectTransform arrowRect = newArrow.GetComponent<RectTransform>();

            // ����������� �������� ��������� � ������� DOTween
            arrowRect.DOScale(1.2f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            // ������ ���� � ����� ������������� �������
            ItemArrowPair newPair = new ItemArrowPair { Item = itemTransform, Arrow = arrowRect };
            itemArrowPairs.Add(newPair);
            UpdateArrow(newPair); // ����� ������������� �������
        }
        else
        {
            Debug.LogWarning($"��� �������� � ����� '{itemTag}' �� ������ ��������������� ������ �������!");
        }
    }

    private void UpdateArrow(ItemArrowPair pair)
    {
        Transform item = pair.Item;
        RectTransform arrow = pair.Arrow;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(item.position);

        // ���� ������ �� ������, �������� �������
        if (screenPos.z > 0 && screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height)
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
        Vector3 direction = item.position - Camera.main.transform.position;
        direction.z = 0;

        // ��������� ���� ��������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������������� ������� �������
        arrow.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private class ItemArrowPair
    {
        public Transform Item;
        public RectTransform Arrow;
    }
}




