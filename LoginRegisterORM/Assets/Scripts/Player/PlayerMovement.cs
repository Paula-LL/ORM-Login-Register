using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public Rigidbody2D rb; 
    public Animator animator;

    public int facing = 1; 

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0 && transform.localScale.x < 0|| horizontal < 0 && transform.localScale.x > 0) {
            FlipSprite(); 
        }

        animator.SetFloat("horizontal", Mathf.Abs(horizontal));
        animator.SetFloat("vertical", Mathf.Abs(vertical));

        rb.velocity = new Vector2(horizontal, vertical) * moveSpeed;
    }

    public void FlipSprite() {
        facing *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); 
    }
}
