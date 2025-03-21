using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Note : MonoBehaviour
{
    public Vector3 startPos;  // 노트의 시작 위치
    public Vector3 targetPos; // 중앙(타겟) 위치
    public float moveTime; // 이동 시간 (노트가 중앙까지 도달하는 시간)
    private bool moveOn = false;
    public float speed;
    public bool isInstantiated = false;
    
    

    private void OnEnable()
    {
        RhythmManager.OnBeat += UpdateNoteSpeed;
    }

    private void OnDisable()
    {
        RhythmManager.OnBeat -= UpdateNoteSpeed;
    }

    private void UpdateNoteSpeed(float newInterval)
    {
        moveTime = newInterval;
    }

    void Start()
    {
        startPos = gameObject.transform.position;
        targetPos = gameObject.transform.position;
        targetPos.x = 0;
    }

    void Update()
    {
        if(moveOn){
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if(transform.position.x>5.7){
            moveOn = false;
            transform.position = startPos;
        }
    }

    public void setSpeedAndMoveOn(){
        speed = (576 /(60f/RhythmManager.Instance.bpm*1f))/192f;
        moveOn = true;
        isInstantiated = true;
        
    }

    public bool checkMoveOn(){
        return moveOn;
    }
}
