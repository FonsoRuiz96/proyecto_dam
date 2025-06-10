using UnityEngine;

public class CultistCombat : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject dialogueCanvas;
    
    public NPC_Patrol cultist;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Se iniciará combate!");
            
            cultist=GameObject.Find("ExplorerCult").GetComponent<NPC_Patrol>();
            cultist.combat();
            // dialogueCanvas.SetActive(true);
            // StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }
    }
}
