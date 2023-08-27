using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class ItemString
{
    public string name;
    public bool added;

    public ItemString(string Name, bool Added)
    {
        name = Name;
        added = Added;
    }
}

public class ItemChanged : MonoBehaviour
{
    private TextMeshProUGUI textBox;
    public GameObject uiPrefab;
    private bool readyToStart;
    public AudioSource audioSource;
    private GameObject newestObj;

    public AudioClip[] addSound;

    private void Start()
    {
        StartCoroutine(SceneStarts());
    }

    public void TriggerItemChange(ItemString itemString)
    {
        if (readyToStart)
        {
            newestObj = Instantiate(uiPrefab, transform);
            textBox = newestObj.GetComponentInChildren<TextMeshProUGUI>();
            if (itemString.added)
            {
                textBox.text = itemString.name + " added";
            }
            else
            {
                textBox.text = itemString.name + " removed";
            }
            audioSource.clip = addSound[UnityEngine.Random.Range(0, addSound.Length)];
            audioSource.Play();
        }

    }
 

    public IEnumerator SceneStarts()
    {
        yield return new WaitForSeconds(1);
        readyToStart = true;
        StopCoroutine(SceneStarts());
    }
}
