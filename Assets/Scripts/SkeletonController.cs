using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public double damage = 0.5;
    private Transform target;
    public Animator animator;

    // Animation-related variable
    private bool isAttackingSkelly = false;
    private bool isWalkingSkelly = false;

    private double health = 2;

    private Hero hero;

    void Start()
    {
        // Set the initial state of the animator
        animator.SetBool("isWalkingSkelly", isWalkingSkelly);
        animator.SetBool("isAttackingSkelly", isAttackingSkelly);

        hero = FindObjectOfType<Hero>();
    }

    void Update()
    {
        // Move towards the player if a target is set
        if (target != null && !isAttackingSkelly)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            isWalkingSkelly = true;
        }
        else
        {
            isWalkingSkelly = false;
        }

        // Update the animator parameter for walking
        animator.SetBool("isWalkingSkelly", isWalkingSkelly);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If colliding with the player, damage the player's health
        if (other.CompareTag("Player"))
        {
            // Assuming you have a Hero script on the player GameObject
            Hero heroScript = other.GetComponent<Hero>();

            if (heroScript != null)
            {
                heroScript.TakeDamage(damage);
            }

            Debug.Log("Skeleton attacks!");

            // Trigger the attack animation
            isAttackingSkelly = true;
            animator.SetBool("isAttackingSkelly", isAttackingSkelly);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Reset the attack state when the player is no longer in range
        if (other.CompareTag("Player"))
        {
            isAttackingSkelly = false;
            animator.SetBool("isAttackingSkelly", isAttackingSkelly);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


    public void TakeDamage(double damage)
    {
        animator.SetTrigger("SkellyHit");

        health -= damage;

        Debug.Log("Skeleton hit! remaining health:" + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (hero != null)
        {
            hero.IncrementDefeatedSkeletons();
        }

        animator.SetTrigger("SkellyDead");
        // Add any death-related logic here, such as playing death animation, dropping items, etc.
        Debug.Log("Skeleton defeated!");

        Invoke("DestroySkeleton", 0.4f);

    }

    private void DestroySkeleton()
    {
        // Actual destruction of the skeleton GameObject
        Destroy(gameObject);
    }
}
