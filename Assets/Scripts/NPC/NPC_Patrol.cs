using System.Collections;
using UnityEngine;

public class NPC_Patrol : MonoBehaviour
{
    public Vector2[] patrolPoints;
    public float speed = 2;
    public float pauseDuration = 1.5f;

    private bool isPaused;
    private int currentPatrolIndex;
    private Vector2 target;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned to NPC.");
            enabled = false;
            return;
        }

        target = patrolPoints[currentPatrolIndex];
        StartCoroutine(SetPatrolPoint());
    }

    void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector3)target - transform.position).normalized;
        if (direction.x > 0 && transform.localScale.x > 0 || direction.x < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
        }
        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            StartCoroutine(SetPatrolPoint());
        }
    }

    IEnumerator SetPatrolPoint()
    {
        isPaused = true;
        anim.Play("Idle");
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];
        isPaused = false;
        anim.Play("Walk");
    }

    public void combat()
    {
        isPaused = true;
        anim.Play("Idle");
        rb.linearVelocity = Vector2.zero;
    }
}
