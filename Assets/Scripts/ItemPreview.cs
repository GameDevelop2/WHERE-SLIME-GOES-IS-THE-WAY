using System.Collections.Generic;
using UnityEngine;

public class ItemPreview : MonoBehaviour
{
    [SerializeField] private Color multipliedColor;

    private SpriteRenderer spriteRenderer;
    private List<SpriteInfo> spriteInfoList;
    private int currentItemIndex;
    private Transform parentTransform;

    struct SpriteInfo
    {
        public Sprite sprite;
        public Color color;
        public Vector3 scale;

        public SpriteInfo(Sprite sprite, Color color, Vector3 scale)
        {
            this.sprite = sprite;
            this.color = color;
            this.scale = scale;
        }
    }

    void Awake()
    {
        currentItemIndex = -1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentTransform = transform.parent;
    }

    public void InitSpriteList(List<ItemInfo> itemInfoList)
    {
        spriteInfoList = new List<SpriteInfo>(itemInfoList.Count);
        foreach (ItemInfo item in itemInfoList)
            spriteInfoList.Add(new SpriteInfo(item.sprite, item.color, item.prefab ? item.prefab.transform.localScale : Vector3.one));
    }

    public void ShowItemPreview(int itemIndex)
    {
        spriteRenderer.sprite = spriteInfoList[itemIndex].sprite;
        spriteRenderer.color = spriteInfoList[itemIndex].color * multipliedColor;

        transform.SetParent(null);
        transform.localScale = spriteInfoList[itemIndex].scale;
        transform.SetParent(parentTransform);

        currentItemIndex = itemIndex;
    }
}
