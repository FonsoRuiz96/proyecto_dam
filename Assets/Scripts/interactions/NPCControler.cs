using UnityEngine;

public class NPCControler : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject dialogueCanvas;
    
    public void Interact()
    {
        Debug.Log("Vas a hablar con este NPC");
        dialogueCanvas.SetActive(true);
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        
    }
}
