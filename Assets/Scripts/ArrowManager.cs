using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Подключаем DOTween

public class ArrowManager : MonoBehaviour
{
    [System.Serializable]
    public class ArrowPrefabMapping
    {
        public string ItemTag; // Тег предмета
        public GameObject ArrowPrefab; // Префаб стрелки для этого предмета
    }

    public List<ArrowPrefabMapping> arrowPrefabMappings; // Список тегов и соответствующих префабов
    public Canvas canvas; // Canvas, в котором будут отображаться стрелки
    public ItemSpawner itemSpawner; // Ссылка на скрипт, который спавнит предметы

    private Dictionary<string, GameObject> arrowPrefabsByTag = new Dictionary<string, GameObject>();
    private List<ItemArrowPair> itemArrowPairs = new List<ItemArrowPair>();

    void Start()
    {
        // Заполняем словарь префабов по тегам
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

        string itemTag = itemTransform.tag; // Получаем тег предмета

        if (arrowPrefabsByTag.TryGetValue(itemTag, out GameObject arrowPrefab))
        {
            // Создаем стрелку для нового объекта
            GameObject newArrow = Instantiate(arrowPrefab, canvas.transform);
            RectTransform arrowRect = newArrow.GetComponent<RectTransform>();

            // Настраиваем анимацию пульсации с помощью DOTween
            arrowRect.DOScale(1.2f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            // Создаём пару и сразу позиционируем стрелку
            ItemArrowPair newPair = new ItemArrowPair { Item = itemTransform, Arrow = arrowRect };
            itemArrowPairs.Add(newPair);
            UpdateArrow(newPair); // Сразу позиционируем стрелку
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

        Vector3 screenPos = Camera.main.WorldToScreenPoint(item.position);

        // Если объект на экране, скрываем стрелку
        if (screenPos.z > 0 && screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height)
        {
            arrow.gameObject.SetActive(false);
            return;
        }
        else
        {
            arrow.gameObject.SetActive(true);
        }

        // Ограничиваем положение стрелки краями экрана
        screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
        screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

        // Перемещаем стрелку в нужное место
        arrow.position = screenPos;

        // Рассчитываем направление к предмету
        Vector3 direction = item.position - Camera.main.transform.position;
        direction.z = 0;

        // Вычисляем угол поворота
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Устанавливаем поворот стрелки
        arrow.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private class ItemArrowPair
    {
        public Transform Item;
        public RectTransform Arrow;
    }
}




