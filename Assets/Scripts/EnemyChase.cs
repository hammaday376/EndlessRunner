using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Chase Settings")]
    [SerializeField] private float enemySpeed = 7f;
    [SerializeField] private float catchDistance = 1.5f;
    [SerializeField] private float followDistance = 4f; // distance behind player
    [SerializeField] private float catchUpMultiplier = 1.5f;

    [Header("Height Control")]
    [SerializeField] private float fixedYPosition = 0f;

    private float currentSpeed;
    private bool isCatchingUp = false;

    private void Start()
    {
        currentSpeed = enemySpeed;

        // Lock enemy Y position (important for runners)
        Vector3 pos = transform.position;
        pos.y = fixedYPosition;
        transform.position = pos;
    }

    private void Update()
    {
        if (!player) return;

        FollowPlayer();
        CheckCatch();
    }

    private void FollowPlayer()
    {
        // Desired position behind the player
        Vector3 targetPosition = player.position - player.forward * followDistance;
        targetPosition.y = fixedYPosition;

        // Move precisely toward target
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            currentSpeed * Time.deltaTime
        );

        // Rotate enemy to face forward (same direction as player)
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            player.rotation,
            10f * Time.deltaTime
        );
    }

    private void CheckCatch()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= catchDistance)
        {
            Debug.Log("PLAYER CAUGHT!");
            // Trigger Game Over / Animation / UI here
        }
    }

    // Call when player hits obstacle or slows down
    public void BoostEnemy()
    {
        isCatchingUp = true;
        currentSpeed = enemySpeed * catchUpMultiplier;
    }

    // Call when player resumes normal speed
    public void NormalizeEnemy()
    {
        isCatchingUp = false;
        currentSpeed = enemySpeed;
    }
}
