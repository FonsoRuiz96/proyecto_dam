using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] GameObject dialogueCanvas;

    public int lettersPerSecond;
    
    public event Action OnShowDialog;
    public event Action OnHideDialog;
    
    private Vector2 input;
    
    public static DialogManager Instance {get; private set;}
    
    public string targetScene;
    public Animator fadeAnimator;
    public float fadeTime;
    
    private Dialog dialog;
    private int currentLine;
    private bool isTyping;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        
        OnShowDialog?.Invoke();

        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }
    
    public void HandleUpdate()
    {
        if (Input.GetKeyUp(KeyCode.E) && !isTyping)
        {
            
            if (currentLine < dialog.Lines.Count)
            {
                StopAllCoroutines();
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                dialogueCanvas.SetActive(false);
                if (SceneManager.GetActiveScene().name == "Forest")
                {
                    fadeAnimator.Play("FadeToWhite");
                    StartCoroutine(DelayFade(fadeTime));
                }
                
                OnHideDialog?.Invoke();
                currentLine = 0;
                
                
            }
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        
        isTyping = false;
        dialogText.text = "";
        
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
        isTyping = true;
        currentLine++;
    }
    
    IEnumerator DelayFade(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(targetScene);
    }
    
}
