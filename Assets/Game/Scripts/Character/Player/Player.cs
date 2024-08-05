using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractCharacter
{
    [SerializeField]
    private Joystick joystick;
    private Vector3 startPos;
    private void Awake()
    {
        startPos = transform.position;
    }
    public override void OnInIt()
    {
        base.OnInIt();
        transform.position = startPos;  
    }

    private void FixedUpdate()
    {
        if (!isFall)
        {
            ChangeDirection();
            Move();
        }
       
    }
    
    public void ChangeDirection()
    {
        characterDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        if (characterDirection.sqrMagnitude >= 0.1)
        {
            transform.rotation = Quaternion.LookRotation(characterDirection);
        }
    }

    public override void Move()
    {
        rb.velocity = new Vector3(characterDirection.x * CHARACTER_SPEED * Time.deltaTime, rb.velocity.y, characterDirection.z * CHARACTER_SPEED * Time.deltaTime);
        animator.SetFloat("velocity", rb.velocity.sqrMagnitude);
    }

    public override void Fall()
    {
        base.Fall();
        StartCoroutine(WakeUp());
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Bot"))
        {
            Bot bot = collision.gameObject.GetComponent<Bot>();
            if(bot.GetStackCount() > this.GetStackCount()) {
                this.Fall();
                bot.ChangeState(new PickBrickState());
            }
            else if(bot.GetStackCount() < this.GetStackCount())
            {
                bot.Fall();
            }
            else
            {
                bot.Fall();
                this.Fall();
            }
        }
    }


}
