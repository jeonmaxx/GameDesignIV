using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Seed")]
public class Seed : ScriptableObject
{
    [Header("Plant Objects")]
    public GameObject Ph01;
    public GameObject Ph02;
    public GameObject Ph03;
    public Item Product;
}
