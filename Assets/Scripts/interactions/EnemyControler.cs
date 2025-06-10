using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyControler : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject dialogueCanvas;
    

    public void Interact()
    {
        Debug.Log("You will start a batle");
        dialogueCanvas.SetActive(true);
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        
    }
}

