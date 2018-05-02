using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour 
{
	[Header("Control")]
	public float speed = 5;
	public float jumpSpeed = 5;

	[Header("GroundCheck")]
	public Transform groundCheckPosition;
	public Vector2 groundCheckSize;

    Vector3 initalPoint;
	SpriteRenderer sr;
	Rigidbody2D rb;
	BetterJump bt;

	public bool IsGround
	{
		get
		{
			var cc = Physics2D.BoxCast(groundCheckPosition.position, 
                                        groundCheckSize,
                                        0, 
                                        Vector2.zero); 

	    	if (cc.collider == null)
				return false;
			if (cc.collider.gameObject == gameObject)
				return false;		
			
			return true;
		}
	}
		
	void Start () 
	{
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		bt = GetComponent<BetterJump> ();

		rb.freezeRotation = true;
        initalPoint = transform.position;
	}
	void OnDrawGizmosSelected()
	{
		if (!groundCheckPosition)
			return;
		Gizmos.DrawWireCube ((Vector3) groundCheckPosition.position, (Vector3)groundCheckSize);
	}

	bool ground;
	void Update () 
	{
        if (!Maker.playing)
            return;

        ground = IsGround;

		ManageFlip ();
		ManageJump ();
    }


	bool jumpRequest = false;
	void FixedUpdate()
	{
        if (!Maker.playing)
            return;

        if (jumpRequest) 
		{
			jumpRequest = false;
			rb.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Impulse);		
		}
	
		rb.velocity = new Vector3 (InputManager.HorizontalAxis * speed, rb.velocity.y);	

		bt.ApplyBetterJump ();			
	}

	void ManageFlip()
	{
		if(InputManager.HorizontalAxis != 0)
			sr.flipX = InputManager.HorizontalAxis < 0;
	}

	void ManageJump()
	{		
		if (InputManager.JumpButtonPressed) {
			if (ground) {
                jumpRequest = true;
			}			
		}
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            transform.position = initalPoint;
        }
    }
}
