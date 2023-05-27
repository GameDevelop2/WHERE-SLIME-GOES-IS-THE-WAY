using System.Collections.Generic;
using UnityEngine;

public class ItemPreview : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private List<SpriteInfo> spriteInfoList;
    private int currentItemIndex;
    private Transform parentTransform;

    struct SpriteInfo
    {
        public Sprite sprite;
        public Vector3 scale;

        public SpriteInfo(Sprite sprite, Vector3 scale)
        {
            this.sprite = sprite;
            this.scale = scale;
        }
    }

    void Awake()
    {
        currentItemIndex = -1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentTransform = transform.parent;
    }
    
    public void InitSpriteList(List<GameObject> itemList)
    {
        spriteInfoList = new List<SpriteInfo>(itemList.Count);
        foreach (GameObject item in itemList)
            spriteInfoList.Add(new SpriteInfo(item.GetComponent<SpriteRenderer>().sprite, item.transform.localScale));
    }

    public void ShowItemPreview(int itemIndex)
    {
        spriteRenderer.sprite = spriteInfoList[itemIndex].sprite;

        transform.SetParent(null);
        transform.localScale = spriteInfoList[itemIndex].scale;
        transform.SetParent(parentTransform);

        currentItemIndex = itemIndex;
    }

    public void HideItemPreview()
    {
        spriteRenderer.sprite = null;
        currentItemIndex = -1;
    }
}
