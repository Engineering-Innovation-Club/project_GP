using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverArrowScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool hover;
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        hover = false;
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
        arrow.SetActive(true);

        arrow.transform.position = new Vector3(GetComponent<RectTransform>().position.x - (GetComponent<RectTransform>().rect.size.x / 2) - 50, transform.position.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        arrow.SetActive(false);
    }
}
