using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Renderer rend;
    public bool interactable = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(interactable)
            rend.material.SetInt("_isOn", 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 0);
    }
}
