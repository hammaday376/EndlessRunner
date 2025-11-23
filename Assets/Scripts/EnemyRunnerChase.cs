using UnityEngine;

public class EnemyRunnerChase : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player; // Reference to the player
    
    [Header("Lane Settings")]
    public float leftLaneX = -3f;    // X position of left lane
    public float centerLaneX = 0f;   // X position of center lane
    public float rightLaneX = 3f;    // X position of right lane
    public float laneChangeSpeed = 10f; // Speed of lane switching
    
    [Header("Follow Settings")]
    public float followDistance = 5f; // Distance to maintain behind player
    public float forwardSpeed = 8f;   // Forward movement speed
    public float maxForwardSpeed = 15f; // Maximum forward speed
    public float speedIncreaseRate = 0.5f; // How fast speed increases
    
    [Header("Smoothing")]
    public float positionSmoothTime = 0.3f;
    
    private float targetLaneX;
    private float currentSpeed;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Start at center lane
        targetLaneX = centerLaneX;
        currentSpeed = forwardSpeed;
        
        // If player not assigned, try to find it
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        
        // Update target lane based on player's position
        UpdateTargetLane();
        
        // Move forward
        MoveForward();
        
        // Change lanes smoothly
        ChangeLane();
        
        // Gradually increase speed over time
        IncreaseSpeed();
    }

    void UpdateTargetLane()
    {
        // Determine which lane the player is in
        float playerX = player.position.x;
        
        // Find closest lane to player
        if (Mathf.Abs(playerX - leftLaneX) < Mathf.Abs(playerX - centerLaneX) && 
            Mathf.Abs(playerX - leftLaneX) < Mathf.Abs(playerX - rightLaneX))
        {
            targetLaneX = leftLaneX;
        }
        else if (Mathf.Abs(playerX - rightLaneX) < Mathf.Abs(playerX - centerLaneX))
        {
            targetLaneX = rightLaneX;
        }
        else
        {
            targetLaneX = centerLaneX;
        }
    }

    void ChangeLane()
    {
        // Smoothly move to target lane
        Vector3 targetPosition = new Vector3(targetLaneX, transform.position.y, transform.position.z);
        Vector3 currentPos = transform.position;
        
        // Smooth lane change
        currentPos.x = Mathf.Lerp(currentPos.x, targetLaneX, laneChangeSpeed * Time.deltaTime);
        transform.position = currentPos;
    }

    void MoveForward()
    {
        // Calculate target Z position (maintain distance behind player)
        float targetZ = player.position.z - followDistance;
        
        // Always stay behind the player
        if (transform.position.z < targetZ)
        {
            // If too far behind, move faster to catch up
            transform.position += Vector3.forward * currentSpeed * Time.deltaTime;
        }
        else
        {
            // If at or ahead of target position, stay at the target distance
            Vector3 pos = transform.position;
            pos.z = targetZ;
            transform.position = pos;
        }
    }

    void IncreaseSpeed()
    {
        // Gradually increase speed over time (like Subway Surfers)
        if (currentSpeed < maxForwardSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
        }
    }

    // Optional: Reset speed (call this when game restarts)
    public void ResetSpeed()
    {
        currentSpeed = forwardSpeed;
    }

    // Optional: Visualize lanes in editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        // Draw lane positions
        Vector3 start = transform.position - Vector3.forward * 10f;
        Vector3 end = transform.position + Vector3.forward * 20f;
        
        // Left lane
        Gizmos.DrawLine(new Vector3(leftLaneX, start.y, start.z), 
                       new Vector3(leftLaneX, end.y, end.z));
        
        // Center lane
        Gizmos.DrawLine(new Vector3(centerLaneX, start.y, start.z), 
                       new Vector3(centerLaneX, end.y, end.z));
        
        // Right lane
        Gizmos.DrawLine(new Vector3(rightLaneX, start.y, start.z), 
                       new Vector3(rightLaneX, end.y, end.z));
    }
}