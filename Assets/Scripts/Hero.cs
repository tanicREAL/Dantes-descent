using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    [SerializeField] float movSpeed = 1f;
    public Animator animator;
    public Rigidbody2D rb;

    public Camera mainCamera;
    public float cameraFollowSpeed = 5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    public GameObject sword;
    public float swordRadius = 1.5f;


    // Attack-related variables
    public float attackCooldown = 1f;
    private float nextAttackTime = 0.1f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackDamage = 1f;

    private int defeatedSkeletons = 0;

    public double maxHealth = 5;
    private double currentHealth;

    private bool isGameOver = false;
    [SerializeField] GameObject gameOverUIPanel;
    [SerializeField] GameObject youWonUIPanel;


    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Starting with " + currentHealth + "HP");
       


    }

    void Update()
    {
        if (!isGameOver)
        {
            HandleMovementInput();
            HandleAttackInput();
            FollowPlayer();
            UpdateSwordPosition();
        }

    }

    void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized * movSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (movement != Vector3.zero)
        {
            animator.SetBool("IsRunning", true);

            if (horizontalInput > 0)
                GetComponent<SpriteRenderer>().flipX = false;
            else if (horizontalInput < 0)
                GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void HandleAttackInput()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DealDamage();

                // Trigger the attack animation
                animator.SetBool("IsAttacking", true);

                // Set the cooldown for the next attack
                nextAttackTime = Time.time + 1f / attackCooldown;
            }

            else animator.SetBool("IsAttacking", false);
        }
    }

    void FollowPlayer()
    {
        if (mainCamera != null)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

            float clampedX = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            float clampedY = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
            targetPosition = new Vector3(clampedX, clampedY, targetPosition.z);

            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
        }
    }

    void UpdateSwordPosition()
    {
        if (mainCamera != null && sword != null)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 directionToMouse = mousePosition - transform.position;
            directionToMouse.z = 0f;

            Vector3 swordPosition = transform.position + directionToMouse.normalized * swordRadius;
            sword.transform.position = swordPosition;

            sword.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToMouse.normalized);
        }
    }


    // Function to be called from the attack animation event
    void DealDamage()
    {

        Collider2D[] hitSkeletons = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D skeletonCollider in hitSkeletons)
        {
            SkeletonController skeletonController = skeletonCollider.GetComponent<SkeletonController>();

            if (skeletonController != null)
            {
                skeletonController.TakeDamage(attackDamage);

            }
        }

        // Add your logic to deal damage to enemies here
        Debug.Log("Attacking!");
    }


    public void IncrementDefeatedSkeletons()
    {
        defeatedSkeletons++;

        Debug.Log("Remaining skeletons: " + (6 - defeatedSkeletons));
        // Check if the victory condition is met
        if (defeatedSkeletons >= 6)
        {
            Victory();
        }
    }


    public void TakeDamage(double skellyDamage)
    {
        if (!isGameOver)
        {
            currentHealth -= skellyDamage;

            // Example: You can add more logic here such as checking for death, updating UI, etc.
            Debug.Log("Player took damage. Current health: " + currentHealth);
        }

        if(currentHealth <= 0)
        {
            isGameOver = true;
            GameOver();
            
        }
        
    }

    private void Victory()
    {
        Debug.Log("You Won!");
        ShowYouWonUI();
    }

    private void ShowYouWonUI()
    {
        if (youWonUIPanel != null)
        {
            // Activate your "You Won" UI panel and any other UI elements
            youWonUIPanel.SetActive(true);
            // Add any other actions you want to take on victory
        }
        else
        {
            Debug.LogError("youWonUIPanel is not assigned in the inspector!");
        }
    }


    private void GameOver()
    {
        Debug.Log("Game Over!");
        ShowGameOverUI();
    }


    private void ShowGameOverUI()
    {
        if (gameOverUIPanel != null)
        {
            // Activate your Game Over UI panel and restart button
            gameOverUIPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("gameOverUIPanel is not assigned in the inspector!");
        }
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
