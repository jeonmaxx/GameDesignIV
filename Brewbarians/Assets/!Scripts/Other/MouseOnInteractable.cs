using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Renderer rend;

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 0);
    }
}
