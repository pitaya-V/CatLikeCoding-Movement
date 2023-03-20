using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;//速度

	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f;//加速度
    
    Vector3 velocity;//速率
	Rigidbody body;
	void Awake(){
		body = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
        // playerInput.Normalize();//归一化

        //用来约束输入向量和归一化差不多，但这样可以使用向量和最大值1作为参数，要么是向量要么是最大值
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        //乘以一个速度，在上面对向量约束后默认为1的情况下，maxspeed用来给出在1m/s的基础上的速度。
		Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        //加速度
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        
		velocity.x =
			Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
		velocity.z =
			Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

		body.velocity = velocity;
    
    }
}
