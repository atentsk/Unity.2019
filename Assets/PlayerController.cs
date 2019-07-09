using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Animator anim;
    Rigidbody rb;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GetComponent<NavMeshAgent>())
            ControllAgent();
        else
            ControllPlayer();
    }

    // NavMesh 를 이동하는 길찾기
    void ControllAgent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            RaycastHit hit;

            Ray ray_ = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray_, out hit, 100))
            {
                agent.destination = hit.point;
                // 목표위치가 갱신되면 NavMeshAgent 에서 이동 처리
            }
        }
    }

    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetInteger("Walk", 1);
        }
        else {
            anim.SetInteger("Walk", 0);
        }

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
                rb.AddForce(0, jumpForce, 0);
                canJump = Time.time + timeBeforeNextJump;
                anim.SetTrigger("jump");
        }
    }
}