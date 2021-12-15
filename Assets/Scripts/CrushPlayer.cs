using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushPlayer : MonoBehaviour
{
    Rigidbody2D crusherRb;

    [SerializeField] Vector2 startPosition;
    public float distanceToGround;
    public LayerMask playerLayer;

    public bool lookingForPlayer = true;
    public bool returningtoStart = false;

    // Start is called before the first frame update
    void Start()
    {
        crusherRb = GetComponent<Rigidbody2D>();
        crusherRb.Sleep();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookingForPlayer)
            LookForPlayer();
        else if(returningtoStart)
        {
            while (transform.position.y < startPosition.y)
            {
                //crusherRb.velocity = Vector2.up * 3.0f;
                transform.Translate(Vector2.up * 1.0f);
            }
            lookingForPlayer = true;
            returningtoStart = false;
            crusherRb.Sleep();
        }
    }

    void LookForPlayer()
    {
        //Debug.DrawRay(startPosition, Vector2.down * distanceToGround);
        // Allows the crusher to wait for the player to walk underneath
        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, distanceToGround, playerLayer);
        if(hit.collider != null)
        {
            //Debug.Log("Thwomp detected player");
            Fall();
        }
    }

    void Fall()
    {
        //Wakes up the Rigidbody2D so it falls
        crusherRb.WakeUp();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(ReturnToStartingPosition());
        }
    }

    IEnumerator ReturnToStartingPosition()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Waited two seconds");
        returningtoStart = true;
        //For whatever reason, this code causes Unity to crash,
        //will need to come up with a different solution in the future

        /*
        while (transform.position.y < startPosition.y)
        {
            //crusherRb.velocity = Vector2.up * 3.0f;
            transform.Translate(Vector2.up * 0.001f);
        }
        //crusherRb.velocity = Vector2.zero;
        crusherRb.Sleep();
        */
    }
}
