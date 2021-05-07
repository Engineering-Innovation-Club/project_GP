using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FontChangeScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_FontAsset initialFont;
    public TMP_FontAsset hoverFont;

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.font = initialFont;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.font = hoverFont;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.font = initialFont;
    }

    private void OnDisable()
    {
        text.font = initialFont;
    }
}
