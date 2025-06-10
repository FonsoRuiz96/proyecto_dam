using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //OBJETOS PERSISTENTES
    public GameObject[] persistentObjects;
    public GameObject[] disableObjects;
    private void Awake()
    {
        if (Instance != null)
        {
            CleanUpAndDestroy();
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            MarkPersistentObjects();
            //DisableSceneObjects();
        }
    }
    
    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
                
            }
        }
        foreach (GameObject obj in disableObjects)
        {
            if (obj != null)
            {   
                Debug.Log(obj.name + " has been disabled");
                obj.SetActive(false);
                
            }
        }
    }

    public void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }
    
    // private void DisableSceneObjects()
    // {
    //     if (disableObjects == null) return;
    //
    //     foreach (GameObject obj in disableObjects)
    //     {
    //         if (obj != null)
    //         {
    //             obj.SetActive(false);
    //         }
    //     }
    // }
}
