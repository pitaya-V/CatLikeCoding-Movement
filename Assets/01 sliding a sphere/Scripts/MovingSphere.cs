using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;//速度

	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f;//加速度
    [SerializeField]
	Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);//范围

    [SerializeField, Range(0f, 1f)]
	float bounciness = 0.5f;//小球达到约定范围后的弹性大小
    
    Vector3 velocity;//速率

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

        //恒定的位移取消帧率对它的影响
        Vector3 displacement = velocity * Time.deltaTime;
        
        
#region 允许操控的面积和弹性的计算

        Vector3 newPosition = transform.localPosition + displacement;
		if (newPosition.x < allowedArea.xMin) {
			newPosition.x = allowedArea.xMin;
			velocity.x = -velocity.x * bounciness;
		}
		else if (newPosition.x > allowedArea.xMax) {
			newPosition.x = allowedArea.xMax;
			velocity.x = -velocity.x * bounciness;
		}
		if (newPosition.z < allowedArea.yMin) {
			newPosition.z = allowedArea.yMin;
			velocity.z = -velocity.z * bounciness;
		}
		else if (newPosition.z > allowedArea.yMax) {
			newPosition.z = allowedArea.yMax;
			velocity.z = -velocity.z * bounciness;
		}
#endregion
        //赋值速度
		transform.localPosition = newPosition;
    }
}
