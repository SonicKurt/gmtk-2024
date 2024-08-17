using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public float jumpForce;
    public float jumpTime;
    public float gravityModifier;
    public float fallGravityModifier;
    public float mass;
    private float scaleDuration;

    private Rigidbody rb;
    private bool isJumping;
    private float jumpKeyPressed;
    private bool isGrounded;
    private bool isMorphing;
    private Vector3 position;

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;
        Vector3 velocity = direction * speed;

        Vector3 currentVelocity = rb.velocity;

        rb.velocity = new Vector3(velocity.x, currentVelocity.y, velocity.z);

        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            isGrounded = false;
            jumpKeyPressed = 0;
        }

        if (isJumping)
        {
            jumpKeyPressed += Time.deltaTime;
            rb.velocity = rb.velocity + Vector3.up * Physics.gravity.y * (gravityModifier - 1) * Time.deltaTime;

            if (jumpKeyPressed > jumpTime || Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.up * Physics.gravity.y * (gravityModifier - 1) * Time.deltaTime, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(Vector3.up * Physics.gravity.y * (fallGravityModifier - 1) * Time.deltaTime, ForceMode.Acceleration);
                isJumping = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerController triggerController = other.GetComponent<TriggerController>();

        if (triggerController != null)
        {
            StartCoroutine(Morph(triggerController.playerObject));
        }
    }

    private IEnumerator Morph(GameObject targetObject)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = targetObject.transform.localScale;

        float elapsedTime = 0;

        while (elapsedTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scaleDuration);

            Debug.Log(transform.localScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        this.gameObject.SetActive(false);

        targetObject.transform.position = position;
        targetObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = mass;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        scaleDuration = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

        Move();
        Jump();
    }
}
