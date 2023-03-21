using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;//速度

	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f;//加速度

	[SerializeField, Range(0f, 10f)]
	float jumpHeight = 2f;//跳跃高度
    
	Vector3 velocity, desiredVelocity;
	Rigidbody body;
	bool desiredJump;//是否跳跃
	bool onGround;//是否在地面上
	void Awake(){
		body = GetComponent<Rigidbody>();
	}

    // 检查输入并设置所需速度的部分保留在 Update 中
    void Update()
    {
        Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		desiredJump |= Input.GetButtonDown("Jump");//保证在fixedUpdate调用前这个值一直是true，直到我们显示的不再跳跃
        //用来约束输入向量和归一化差不多，但这样可以使用向量和最大值1作为参数，要么是向量要么是最大值
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);

		desiredVelocity = new Vector3(playerInput.x,0f,playerInput.y)*maxSpeed;
    
    }
	//速度的调整移动到 FixedUpdate 方法,使用了物理引擎所以在物理帧中改变速度
	private void FixedUpdate() {
		velocity = body.velocity;
		float maxSpeedChange = maxAcceleration * Time.deltaTime;
		velocity.x = Mathf.MoveTowards(velocity.x,desiredVelocity.x,maxSpeedChange);
		velocity.z = Mathf.MoveTowards(velocity.z,desiredVelocity.z,maxSpeedChange);
		if (desiredJump) {
			desiredJump = false;
			Jump();
		}
		body.velocity = velocity;
		onGround = false;
	}
	//跳跃方法
    private void Jump()
    {
		if (onGround)//只有在地面上的时候才可以跳跃
		{
			velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);//速度v = √-2gh 重力是负号所以是-2*
		}
       	
    }

	void OnCollisionEnter () {
		onGround = true;
	}

	void OnCollisionStay  () {
		onGround = true;
	}
}
