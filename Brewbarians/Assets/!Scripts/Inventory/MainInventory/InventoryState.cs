using UnityEngine;
using UnityEngine.UI;

public enum Windows { Inventory, Map, Recipe }
public class InventoryState : MonoBehaviour
{
    public Windows window;
    public GameObject manager;
    private OpenInventory open;
    private Button thisBut;

    private void Start()
    {
        open = manager.GetComponent<OpenInventory>();
        thisBut = GetComponent<Button>();
        if(window == Windows.Inventory)
        {
            thisBut.Select();
        }
    }

    public void ChangeState()
    {
        switch (window)
        {
            case Windows.Inventory:
                open.toolBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(open.newToolBarPos.x, open.newToolBarPos.y, 0);
                open.windows = Windows.Inventory;                
                break; 
            case Windows.Map:
                open.toolBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(open.toolBarPos.x, open.toolBarPos.y, 0);
                open.windows = Windows.Map;
                break; 
            case Windows.Recipe:
                open.toolBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(open.toolBarPos.x, open.toolBarPos.y, 0);
                open.windows = Windows.Recipe;
                break; 
            default:
                break;
        }
    }
}
