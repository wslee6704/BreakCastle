using System;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static event Action<float> OnBeat; // 비트가 발생할 때 실행될 이벤트
    public static event Action<string> OnPlayerBeat; // 플레이어 입력 이벤트 (입력 타입 전달)

    [SerializeField] private float bpm = 60f; // BPM 설정 (예: 120 BPM)
    [SerializeField] private float inputOffset = 0.15f; // 허용되는 오차 (±0.15초)
    [SerializeField] private float bestInputOffset = 0.07f; // 허용되는 오차 (±0.07초)
    [SerializeField] private float duplInputOffset = 0.03f; // 중복입력허용되는 오차 (±0.07초)

    public float beatInterval; // 한 비트당 시간 (초)
    private float nextBeatTime; // 다음 비트 발생 시간
    private float judgeTime;//실제 판정허가 시간
    private bool inputRegistered = false; // 입력이 이미 처리되었는지 확인
    private List<KeyCode> inputBuffer = new List<KeyCode>(); // 동시 입력을 저장할 버퍼
    private void Awake()
    {
        beatInterval = 60f / bpm; // BPM을 초 단위로 변환
        nextBeatTime = Time.time + beatInterval;
        judgeTime = Time.time + beatInterval + inputOffset;//다음 비트 + 인풋 오차 시간
    }

    private void Update()
    {
        float currentTime = Time.time;

        // 🎵 비트 발생 체크 - 단순 메트로놈과 노트만 다룸.
        if (currentTime >= nextBeatTime)
        {
            //Debug.Log("노트 오차"+(currentTime - nextBeatTime));
            OnBeat?.Invoke(beatInterval); // 비트 이벤트 호출
            nextBeatTime += beatInterval;
            
        }
        //게임 엔티티 이동체크(노트 시간 + inputOffset 값인 judge라는 변수사용)
        if(currentTime >= judgeTime)
        {
            if(Mathf.Abs(judgeTime - nextBeatTime)>inputOffset) 
            {
                judgeTime = nextBeatTime + inputOffset;
            }
            else{
                judgeTime = nextBeatTime + inputOffset + beatInterval;
            } 
            ProcessInputBuffer(currentTime);
            inputRegistered = false; // 새 판정범위에서 입력 초기화
        }
        // 🎮 플레이어 입력 체크
        if (Input.anyKeyDown&&!inputRegistered)
        {
            CheckFirstInput(currentTime);
            
        }else if(Input.anyKeyDown) {
            StoreInput(currentTime);
        }
    }

    private void CheckFirstInput(float currentTime){
        //플레이어 판정 수치 확인하는곳 perfect good bad
        float timeDifference = Mathf.Abs(currentTime - (judgeTime - inputOffset));//목표 노트 시간
        if(timeDifference <= bestInputOffset){
            Debug.Log("🎯지렸다리 지렸다! 입력 성공 "
            + "판정가능 시간 : " + (judgeTime - inputOffset) + "\n실제시간 : " + currentTime);
        }else if(timeDifference <= inputOffset){
            Debug.Log("🎯그냥 입력 성공 "
            + "판정가능 시간 : " + (judgeTime - inputOffset) + "\n실제시간 : " + currentTime);
        }else{
            Debug.Log("❌ Missed Timing! 입력 실패 "
            + "판정가능 시간 : " + (judgeTime - inputOffset) + "\n실제시간 : " + currentTime);
        }

        //플레이어가 입력이 됐으니 그후 바로 판정나오게
        if(timeDifference <= inputOffset){
            inputRegistered = true;
            if(currentTime + duplInputOffset < judgeTime){
                judgeTime = currentTime + duplInputOffset;
            }
            StoreInput(currentTime);
        }
        
    }

     private void StoreInput(float currentTime)
    {
        if (Input.GetKeyDown(KeyCode.A) && !inputBuffer.Contains(KeyCode.A)) inputBuffer.Add(KeyCode.A);
        if (Input.GetKeyDown(KeyCode.D) && !inputBuffer.Contains(KeyCode.D)) inputBuffer.Add(KeyCode.D);
        if (Input.GetKeyDown(KeyCode.K) && !inputBuffer.Contains(KeyCode.K)) inputBuffer.Add(KeyCode.K);
    }

    private void ProcessInputBuffer(float currentTime)
    {
        if (inputBuffer.Count == 0)
        {
            Debug.Log("⏳ 입력 없음 현재 시간 " + currentTime +"\n판정 시간 :" + judgeTime);
            return;
        }

        string action = DetermineAction();
        if (!string.IsNullOrEmpty(action))
        {
            Debug.Log(action + Time.time);
            OnPlayerBeat?.Invoke(action); // 이벤트 실행 (한 번만 호출)
        }

        inputBuffer.Clear(); // 입력 버퍼 초기화
    }

    private string DetermineAction()
    {
        // 🎮 동시 입력 판정 (A+K, D+K)
        if (inputBuffer.Contains(KeyCode.K))
        {
            if (inputBuffer.Contains(KeyCode.A)) return "special_AK";
            if (inputBuffer.Contains(KeyCode.D)) return "special_DK";
        }

        // 일반 행동 판정
        if (inputBuffer.Contains(KeyCode.A)) return "move_A";
        if (inputBuffer.Contains(KeyCode.D)) return "move_D";
        if (inputBuffer.Contains(KeyCode.K)) return "attack";

        return null;
    }
}

