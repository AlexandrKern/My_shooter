using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowManager : MonoBehaviour
{
    [System.Serializable]
    public class ArrowPrefabMapping
    {
        public string ItemTag;
        public GameObject ArrowPrefab;
    }

    public List<ArrowPrefabMapping> arrowPrefabMappings;
    public Canvas canvas;
    public ItemSpawner itemSpawner;

    private Dictionary<string, GameObject> arrowPrefabsByTag = new Dictionary<string, GameObject>();
    private List<ItemArrowPair> itemArrowPairs = new List<ItemArrowPair>();

    void Start()
    {
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
        string itemTag = itemTransform.tag;

        if (arrowPrefabsByTag.TryGetValue(itemTag, out GameObject arrowPrefab))
        {
            GameObject newArrow = Instantiate(arrowPrefab, canvas.transform);
            RectTransform arrowRect = newArrow.GetComponent<RectTransform>();

            arrowRect.DOScale(1.2f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            ItemArrowPair newPair = new ItemArrowPair { Item = itemTransform, Arrow = arrowRect };
            itemArrowPairs.Add(newPair);
            UpdateArrow(newPair);
        }
        else
        {
            Debug.LogWarning($"Для предмета с тегом '{itemTag}' не найден соответствующий префаб стрелки!");
        }
    }

    private void UpdateArrow(ItemArrowPair pair)
    {
        Transform item = pair.Item;
        RectTransform arrow = pair.Arrow;
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Vector3 screenPos = cam.WorldToScreenPoint(item.position);

        if (screenPos.z < 0)
        {
            // Если объект за камерой, инвертируем экранные координаты
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        bool isOffScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (isOffScreen)
        {
            arrow.gameObject.SetActive(true);

            Vector3 direction = (screenPos - screenCenter).normalized;
            float borderOffsetX = 50f; 
            float borderOffsetY = 50f; 

            float maxX = Screen.width - borderOffsetX;
            float maxY = Screen.height - borderOffsetY;

            screenPos.x = Mathf.Clamp(screenPos.x, borderOffsetX, maxX);
            screenPos.y = Mathf.Clamp(screenPos.y, borderOffsetY, maxY);

            arrow.position = screenPos;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            arrow.gameObject.SetActive(false);
        }
    }

    private class ItemArrowPair
    {
        public Transform Item;
        public RectTransform Arrow;
    }
}





