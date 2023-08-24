using System.IO;
using UnityEngine;

public class GiveTools : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] tools;
    public void Start()
    {
        string path = Application.persistentDataPath + "/" + "sceneGD.json";

        if (!File.Exists(path + "sceneGD.json"))
        {
            for(int i = 0; i < tools.Length; i++)
            {
                inventoryManager.AddItem(tools[i]);
            }
        }
    }
}
