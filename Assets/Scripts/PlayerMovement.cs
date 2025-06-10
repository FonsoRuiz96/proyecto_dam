using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed=5;
    
    private Vector2 input;
    private bool isMoving;
    
    private Animator animator;
    
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    //public void HandleUpdate()
    {
        if (!isMoving)
        {
            if (input.x != 0) input.y = 0;
            
            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX",input.x);
                animator.SetFloat("moveY",input.y);
                
                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                if(IsWalkable(targetPosition)) StartCoroutine(Move(targetPosition));
                
            }
        }
        
        animator.SetBool("isMoving", isMoving);
    }
    
    
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        //input = context.ReadValue<Vector2>();
        //Debug.Log("Has pulsado la Z");
        
        if(context.started)
        { 
            var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            var interactPos = transform.position + facingDir;

            var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
            if (collider != null)
            {
                Debug.Log("Aqui hay un NPC!");
                collider.GetComponent<Interactable>()?.Interact();
            }
        }
    }
    
    
    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;
        
        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        
        isMoving = false;
    }
    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.02f, solidObjectsLayer | interactableLayer) != null) return false;
        return true;
    }
    
    
    
    
}
