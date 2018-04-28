using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour 
{
	[Header("Control")]
	public float speed = 10;
	public float jumpSpeed = 3;

	[Header("GroundCheck")]
	public Transform groundCheckPosition;
	public Vector2 groundCheckSize;

	SpriteRenderer sr;
	Rigidbody2D rb;
	BetterJump bt;

	public bool IsGround
	{
		get
		{

			var cc = Physics2D.BoxCast(groundCheckPosition.position, groundCheckSize,0, Vector2.up); 

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
	}
	void OnDrawGizmosSelected()
	{
		if (!groundCheckPosition)
			return;
		Gizmos.DrawWireCube ((Vector3) groundCheckPosition.position, (Vector3)groundCheckSize);
	}

	float horizontalAxis;
	bool ground;
	void Update () 
	{
        if (!Maker.Play)
            return;


		horizontalAxis = Input.GetAxis ("Horizontal");
		ground = IsGround;

		ManageFlip ();
		ManageJump ();
    }


	bool jumpRequest = false;
	void FixedUpdate()
	{
		if (jumpRequest) 
		{
			jumpRequest = false;
			rb.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Impulse);		
		}
	
		rb.velocity = new Vector3 (horizontalAxis * speed, rb.velocity.y);	

		bt.ApplyBetterJump ();			
	}

	void ManageFlip()
	{
		if(horizontalAxis != 0)
			sr.flipX = horizontalAxis < 0;
	}

	void ManageJump()
	{		
		if (InputManager.JumpButtonPressed) {
			if (ground) {
                jumpRequest = true;
			}			
		}
	}
		
	IEnumerator DelayedEvents(System.Action ev, float time)
	{
		yield return new WaitForSeconds (time);
		ev ();
	}


}
