using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickFieldPuzzle : MonoBehaviour, IPointerClickHandler
{
    public HandManager handManager;
    public bool clicked;
    public Sprite clickedSprite;
    public PlayerMovement movement;
    public AudioSource audioSource;
    public ToolSoundManager toolSoundManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(handManager.handItem.actionType == ActionType.Water)
        {
            clicked = true;
            audioSource.clip = toolSoundManager.wateringSounds[Random.Range(0, toolSoundManager.wateringSounds.Length)];
            StartCoroutine(PlayAnim("IsWatering", 1.3f));
            transform.GetComponent<SpriteRenderer>().sprite = clickedSprite;
        }
    }

    public IEnumerator PlayAnim(string animName, float time)
    {
        movement.forbidToWalk = true;
        movement.animator.SetBool(animName, true);
        audioSource.Play();
        yield return new WaitForSeconds(time);
        movement.animator.SetBool(animName, false);
        movement.forbidToWalk = false;
        StopCoroutine(PlayAnim(animName, time));
    }
}
