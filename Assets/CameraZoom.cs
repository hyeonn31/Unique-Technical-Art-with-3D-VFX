using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("줌 설정")]
    [Tooltip("줌인/줌아웃 주기 (초)")]
    public float zoomDuration = 5f; // 한 사이클에 걸리는 시간
    
    [Tooltip("줌 방식 선택")]
    public ZoomMode zoomMode = ZoomMode.FieldOfView;
    
    [Tooltip("FOV 줌일 때: 최소 FOV 값")]
    public float minFOV = 30f;
    
    [Tooltip("FOV 줌일 때: 최대 FOV 값")]
    public float maxFOV = 60f;
    
    [Tooltip("Position 줌일 때: 최대 줌 거리 (카메라를 앞으로 이동)")]
    public float maxZoomDistance = 5f;
    
    [Tooltip("Position 줌일 때: 최소 줌 거리")]
    public float minZoomDistance = 0f;
    
    [Tooltip("애니메이션 커브 (선택사항, null이면 선형)")]
    public AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Tooltip("자동 시작")]
    public bool autoStart = true;

    private Camera cam;
    private Vector3 originalPosition;
    private float originalFOV;
    private bool isZooming = false;

    public enum ZoomMode
    {
        FieldOfView,    // FOV 변경으로 줌
        Position        // 카메라 위치 변경으로 줌
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam == null)
        {
            Debug.LogError("CameraZoom: 카메라를 찾을 수 없습니다!");
            enabled = false;
            return;
        }

        originalPosition = cam.transform.localPosition;
        originalFOV = cam.fieldOfView;

        if (autoStart)
        {
            StartZoom();
        }
    }

    public void StartZoom()
    {
        if (!isZooming)
        {
            isZooming = true;
            StartCoroutine(ZoomCycle());
        }
    }

    public void StopZoom()
    {
        isZooming = false;
        StopAllCoroutines();
    }

    private IEnumerator ZoomCycle()
    {
        while (isZooming)
        {
            // 줌인 (카메라가 가까워지거나 FOV가 작아짐)
            yield return StartCoroutine(ZoomIn());
            
            // 줌아웃 (카메라가 멀어지거나 FOV가 커짐)
            yield return StartCoroutine(ZoomOut());
        }
    }

    private IEnumerator ZoomIn()
    {
        float elapsed = 0f;
        
        while (elapsed < zoomDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (zoomDuration / 2f);
            
            // 커브 적용
            if (zoomCurve != null && zoomCurve.length > 0)
            {
                t = zoomCurve.Evaluate(t);
            }

            if (zoomMode == ZoomMode.FieldOfView)
            {
                cam.fieldOfView = Mathf.Lerp(maxFOV, minFOV, t);
            }
            else // Position
            {
                float distance = Mathf.Lerp(minZoomDistance, maxZoomDistance, t);
                cam.transform.localPosition = originalPosition - cam.transform.forward * distance;
            }

            yield return null;
        }
    }

    private IEnumerator ZoomOut()
    {
        float elapsed = 0f;
        
        while (elapsed < zoomDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (zoomDuration / 2f);
            
            // 커브 적용
            if (zoomCurve != null && zoomCurve.length > 0)
            {
                t = zoomCurve.Evaluate(t);
            }

            if (zoomMode == ZoomMode.FieldOfView)
            {
                cam.fieldOfView = Mathf.Lerp(minFOV, maxFOV, t);
            }
            else // Position
            {
                float distance = Mathf.Lerp(maxZoomDistance, minZoomDistance, t);
                cam.transform.localPosition = originalPosition - cam.transform.forward * distance;
            }

            yield return null;
        }
    }

    // 즉시 원래 상태로 리셋
    public void ResetCamera()
    {
        cam.fieldOfView = originalFOV;
        cam.transform.localPosition = originalPosition;
    }
}

