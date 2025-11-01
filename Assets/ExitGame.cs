using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [Header("종료 설정")]
    [Tooltip("ESC 키로 종료 가능 여부")]
    public bool allowESCToExit = true;
    
    [Tooltip("종료 확인 다이얼로그 표시 여부")]
    public bool showConfirmDialog = false;

    private static ExitGame instance;

    private void Awake()
    {
        // 싱글톤 패턴: 중복 방지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
    }

    private void Update()
    {
        if (allowESCToExit && Input.GetKeyDown(KeyCode.Escape))
        {
            if (showConfirmDialog)
            {
                // Editor에서는 다이얼로그 표시 (빌드에서는 직접 종료)
                #if UNITY_EDITOR
                if (UnityEditor.EditorUtility.DisplayDialog("종료", "게임을 종료하시겠습니까?", "예", "아니오"))
                {
                    QuitGame();
                }
                #else
                QuitGame();
                #endif
            }
            else
            {
                QuitGame();
            }
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Unity Editor에서는 재생 모드 중지
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 애플리케이션에서는 종료
        Application.Quit();
        #endif
    }

    // 버튼에서 호출할 수 있는 메서드
    public void ExitButton()
    {
        QuitGame();
    }
}

