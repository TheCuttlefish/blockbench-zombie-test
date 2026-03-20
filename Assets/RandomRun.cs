using UnityEngine;

public class RandomRun : MonoBehaviour
{
    public float speed = 2f;
    public float maxRotationAngle = 30f;
    public float maxDistanceFromCenter = 20f;
    public Vector3 centerPoint = Vector3.zero;

    private float timer;
    private float currentInterval;


    public string[] animationStates; // put your state names here (e.g. "Run", "Run2", "Run3")

    void Start()
    {
        Animator anim = GetComponent<Animator>();

        if (anim != null && animationStates.Length > 0)
        {
            // Pick random animation
            string randomState = animationStates[Random.Range(0, animationStates.Length)];

            // Start at random point in animation
            float randomStartTime = Random.value;

            anim.Play(randomState, 0, randomStartTime);

            // Slight speed variation
            anim.speed = Random.Range(0.8f, 1.2f);
        }
    
    
        SetNewInterval();
    }

    void Update()
    {
        Vector3 flatPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatCenter = new Vector3(centerPoint.x, 0f, centerPoint.z);

        float distanceFromCenter = Vector3.Distance(flatPosition, flatCenter);

        if (distanceFromCenter > maxDistanceFromCenter)
        {
            Vector3 directionToCenter = (flatCenter - flatPosition).normalized;

            if (directionToCenter != Vector3.zero)
            {
                // Because movement uses -Vector3.forward,
                // we must face the OPPOSITE direction so movement goes toward center
                transform.rotation = Quaternion.LookRotation(-directionToCenter);
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= currentInterval)
            {
                float randomAngle = Random.Range(-maxRotationAngle, maxRotationAngle);
                transform.Rotate(0f, randomAngle, 0f);

                timer = 0f;
                SetNewInterval();
            }
        }

        // Your model moves correctly with reversed forward
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

    void SetNewInterval()
    {
        currentInterval = Random.Range(0f, 2f);
    }
}