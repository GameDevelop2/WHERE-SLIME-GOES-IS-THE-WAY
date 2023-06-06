using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectUI : MonoBehaviour
{
    [SerializeField] List<Image> itemPanelImageList;
    [SerializeField] Color selectedColor;
    [SerializeField] Color unselectedColor;

    private int previousSelectedItemIndex; // 이전에 선택한 아이템의 인덱스

    private void Start()
    {
        previousSelectedItemIndex = 0;
        foreach (Image panelImage in itemPanelImageList)
            panelImage.color = unselectedColor;
    }

    public void HighlightSelectedItem(int itemIndex)
    {
        itemPanelImageList[previousSelectedItemIndex].color = unselectedColor;
        itemPanelImageList[itemIndex].color = selectedColor;
        previousSelectedItemIndex = itemIndex;
    }
}
