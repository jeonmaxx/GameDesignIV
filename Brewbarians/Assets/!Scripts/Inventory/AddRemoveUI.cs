using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveUI : MonoBehaviour
{
    void Update()
    {
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
