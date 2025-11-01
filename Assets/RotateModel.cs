using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    [Header("회전 설정")]
    [Tooltip("Y축 기준 회전 속도 (도/초)")]
    public float rotationSpeed = 30f; // 초당 30도 회전
    
    [Tooltip("한 바퀴 도는 시간 (초). 0이면 rotationSpeed 사용")]
    public float rotationTime = 0f; // 0이면 rotationSpeed 사용, 설정하면 해당 시간에 맞춰 회전
    
    [Tooltip("회전할 축 (X, Y, Z)")]
    public Vector3 rotationAxis = Vector3.up; // Y축 기준 (0, 1, 0)
    
    [Tooltip("회전 시작 여부")]
    public bool startRotation = true;
    
    [Header("회전 중심점 설정")]
    [Tooltip("사용자 지정 회전 중심점 사용 여부")]
    public bool useCustomPivot = false;
    
    [Tooltip("회전 중심점 위치 (월드 좌표). useCustomPivot이 true일 때만 사용")]
    public Vector3 pivotPoint = Vector3.zero;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        // 초기 위치와 회전 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        
        // 시작 시 회전이 활성화되어 있는지 확인
        if (!startRotation)
        {
            Debug.LogWarning("RotateModel: startRotation이 false입니다. 회전을 시작하려면 true로 설정하세요.");
        }
    }

    private void Update()
    {
        if (startRotation && enabled)
        {
            float speed;
            
            // rotationTime이 설정되어 있으면 해당 시간에 맞춰 계산
            if (rotationTime > 0)
            {
                speed = 360f / rotationTime; // 360도를 rotationTime 초에 맞춰 회전
            }
            else
            {
                speed = rotationSpeed;
            }
            
            // 회전 속도가 0이 아닌지 확인
            if (speed != 0 && rotationAxis != Vector3.zero)
            {
                if (useCustomPivot)
                {
                    // 사용자 지정 중심점 기준 회전
                    RotateAroundPivot(pivotPoint, rotationAxis.normalized, speed * Time.deltaTime);
                }
                else
                {
                    // 기본: Transform 위치 기준 회전 (현재 방식)
                    transform.Rotate(rotationAxis.normalized * speed * Time.deltaTime, Space.World);
                }
            }
        }
    }
    
    // 특정 중심점을 기준으로 회전하는 함수
    private void RotateAroundPivot(Vector3 pivot, Vector3 axis, float angle)
    {
        // Unity의 RotateAround를 사용하여 중심점 기준 회전
        transform.RotateAround(pivot, axis, angle);
    }
    
    // 스크립트에서 회전 시작/정지
    public void StartRotation()
    {
        startRotation = true;
    }
    
    public void StopRotation()
    {
        startRotation = false;
    }
    
    // 회전 속도 변경
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
        rotationTime = 0; // rotationTime 비활성화
    }
    
    // 회전 시간 설정 (예: 5초에 한 바퀴)
    public void SetRotationTime(float time)
    {
        rotationTime = time;
    }
}

