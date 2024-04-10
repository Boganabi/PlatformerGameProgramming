using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{

    public float speed = 4.5f;
    private float jumpForce = 12.0f;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;

    // bool updatedScale = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;

        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = hit != null; // false
        //if(hit != null)
        //{
        //    grounded = true;
        //}

        // if we are on the moving platform, attach to it
        // if we are not on the platform, detach
        MovingPlatform platform = null;
        if (hit != null)
        {
            platform = hit.GetComponent<MovingPlatform>();
        }
        if(platform != null)
        {
            //transform.parent = platform.transform;
            //Debug.Log(transform.localScale.x);
            //Debug.Log(platform.transform.localScale.x);
            //transform.localScale = new Vector3(
            //transform.localScale.x / platform.transform.localScale.x,
            //transform.localScale.y / platform.transform.localScale.y,
            //transform.localScale.z / platform.transform.localScale.z);
            //transform.localScale = Vector3.one;
            // Debug.Log(transform.localScale);
            // updatedScale = true;
            // transform.position = platform.transform.position;
            // could add rotation here too if needed
            transform.SetParent(platform.transform, true);
        }
        else
        {
            transform.parent = null;
            // transform.localScale = Vector3.one;
            // updatedScale = false;
        }

        // make sure scale stays the same
        Vector3 playerScale = Vector3.one;
        if(platform != null)
        {
            // playerScale = platform.transform.localScale;
        }
        if(!Mathf.Approximately(deltaX, 0))
        {
            // transform.localScale = new Vector3(Mathf.Sign(deltaX) / playerScale.x, 1 / playerScale.y, 1);
        }

        anim.SetFloat("Speed", Mathf.Abs(deltaX));
        if(!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }

        body.gravityScale = (grounded && Mathf.Approximately(deltaX, 0)) ? 0 : 1;
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
