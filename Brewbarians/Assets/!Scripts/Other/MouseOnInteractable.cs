using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnInteractable : PlayerNear, IPointerEnterHandler, IPointerExitHandler
{
    public Renderer rend;
    public bool interactable = true;

    private void Update()
    {
        CalcDistance();

        if (isPlayerNear)
            rend.material.SetColor("_BorderColor", Color.white);
        else
            rend.material.SetColor("_BorderColor", Color.red);
    }

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
