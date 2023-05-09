using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D player;
    public float turnSpeed = 45;
    public float moveSpeed = 5;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 direction = playerPosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), turnSpeed * Time.deltaTime));
        rb.MovePosition(rb.position + ((Vector2)transform.right * moveSpeed * Time.deltaTime));
    }
}
