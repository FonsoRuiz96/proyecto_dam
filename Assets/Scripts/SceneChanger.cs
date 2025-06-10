using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string targetScene;
    
    
    
    public Animator fadeAnimator;
    public float fadeTime;
    public Vector3 newPlayerPosition;
    private Transform player;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Pisaste la trampa!");
            player = collision.transform;
            fadeAnimator.Play("FadeToWhite");
            StartCoroutine(DelayFade(fadeTime));
        }
    }

    IEnumerator DelayFade(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);
        player.position = newPlayerPosition;
        SceneManager.LoadScene(targetScene);
    }
}
