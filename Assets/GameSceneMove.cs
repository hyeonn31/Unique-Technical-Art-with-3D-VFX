using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneMove : MonoBehaviour
{
    [Header("씬 이름 설정")]
    public string sceneName0 = "Start"; // 버튼 0번용 씬 이름
    public string sceneName1 = "BlockStyleParticle"; // 버튼 1번용 씬 이름
    public string sceneName2 = "Swaying Style"; // 버튼 2번용 씬 이름
    public string sceneName3 = "StatueCaptureEffect"; // 버튼 3번용 씬 이름
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   // 버튼 0번 클릭 시 호출
    public void LoadScene0(){
        SceneManager.LoadScene(sceneName0);
    }

    // 버튼 1번 클릭 시 호출
    public void LoadScene1(){
        SceneManager.LoadScene(sceneName1);
    }

    // 버튼 2번 클릭 시 호출
    public void LoadScene2(){
        SceneManager.LoadScene(sceneName2);
    }

    // 버튼 3번 클릭 시 호출
    public void LoadScene3(){
        SceneManager.LoadScene(sceneName3);
    }
}
