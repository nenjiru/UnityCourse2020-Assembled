using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public Animator animator;
    public Rigidbody rigid;
    public Vector3 velocity;
    public float moveSpeed = 1.5f;
    public float jumpSpeed = 3f;
    private bool isGrounded = false;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void Jump()
    {
        if (isGrounded == true)
        {
            isGrounded = false;
            rigid.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    void Update()
    {
        float x = Mathf.Abs(joystick.Horizontal);
        float y = Mathf.Abs(joystick.Vertical);
        float move = Mathf.Max(x, y);
        animator.SetFloat("Blend", move);

        Vector3 input = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        Vector3 direction = transform.TransformDirection(input);
        velocity = direction * moveSpeed;
        transform.localPosition += velocity * Time.deltaTime;

        if (input != Vector3.zero)
        {
            animator.transform.localRotation = Quaternion.LookRotation(input);
        }
    }
}
