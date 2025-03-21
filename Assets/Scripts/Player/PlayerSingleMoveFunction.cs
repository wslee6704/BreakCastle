using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
public class PlayerSingleMoveFunction : MonoBehaviour
{

    //플레이어의 기본적인 이동을 저장해놓음. 위, 수평, 아래
    private PlayerMove playerMove;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }
    
    
    //플레이어가 normalMove를 한다면, 위, 수평, 아래의 이동을 결정하고 실제 이동을 실행.
    public void estimateNormalMove(int pos){
        //플레이어의 현재 위치를 index값으로 확인
        (int x, int y) myPos = PlayerMove.VecToIndex(transform.position);
        int nextIndexX = myPos.x + pos;
        if(nextIndexX < 0) nextIndexX = 5;
        else if(nextIndexX > 5) nextIndexX = 0;


        playerMove.playerIndexX = nextIndexX;
        //움직여야할 블럭의 y인덱스 확인
        int nextIndexY = BlockManager.Instance.getHighestIndexY(nextIndexX);
        
        //목표한 위치로 설정
        playerMove.targetPos = PlayerMove.IndexToVec(nextIndexX, nextIndexY);
        
        //평행이동
        if(myPos.y == nextIndexY) actionNormalMoveAxiom(pos);
        //아래로 이동
        else if(myPos.y > nextIndexY) actionNormalMoveDown(playerMove.targetPos);
        //위로 이동
        else actionNormalMoveUp(playerMove.targetPos, nextIndexY-myPos.y);
    }


    //////////////////////////////아래로 이동 함수 정리////////////////////////////////
    void actionNormalMoveAxiom(int pos){
        playerMove.PlayerOnMove();
        transform.DOMove(playerMove.targetPos, 20f/RhythmManager.Instance.bpm).SetEase(Ease.OutCubic).OnComplete(() =>{
            playerMove.PlayerOffMove();
        });
    }

    //////////////////////////////위로 이동 함수 정리////////////////////////////////
    public Vector3 targetPos;  // 최종 목표 위치
    public int repeatCount;    // 올라가야 하는 칸 / maxStep
    public int maxStep = 3;    // 한 번에 이동할 수 있는 최대 칸
    public float duration = 0.5f; // 이동 속도
    public Vector3 besideOffset = new Vector3(1, 0, 0); // 측면 이동 거리

    private List<Vector3> positions = new List<Vector3>(); // 이동 경로
    private int currentIndex = 0;
    private int moveCount = 0;
    void actionNormalMoveUp(Vector2 vec, int distance){
        //위에 이동 후, 옆으로 이동
        //climbstatus가 2라고 하였을 때,
        //3칸 밑으로 있다면 upup upbeside
        //2칸 밑으로 있다면 upupbeside 이렇게 움직인다.
        //몫에서 마지막에 옆으로 이동하는걸 넣어야함.
        
    }

    void GeneratePositions()
    {
        positions.Clear();
        Vector3 startPos = transform.position;
        Vector3 currentPos = startPos;

        int totalSteps = repeatCount * maxStep; // 전체 올라가야 하는 칸 수
        int restCount = totalSteps % maxStep;   // 남은 칸 수

        // 주 이동 (maxStep씩 반복)
        for (int i = 0; i < repeatCount; i++)
        {
            for (int j = 0; j < maxStep; j++)
            {
                currentPos += Vector3.up; // 위로 이동
                positions.Add(currentPos);
            }
        }

        // 남은 칸이 있다면 먼저 이동
        for (int i = 0; i < restCount; i++)
        {
            currentPos += Vector3.up;
            positions.Add(currentPos);
        }

        // 마지막 측면 이동 (beside)
        currentPos += besideOffset;
        positions.Add(currentPos);
    }

    void MoveToNextPosition()
{
    if (moveCount >= positions.Count)
    {
        return; // 모든 이동 완료
    }

    Vector3 targetPosition = positions[moveCount];

    float moveTime = 20f / RhythmManager.Instance.bpm / (repeatCount + 1);

    transform.DOMove(targetPosition, moveTime).OnComplete(() => {
        moveCount++;
        MoveToNextPosition(); // 재귀 호출
    });
}


    void actionNormalMoveDown(Vector2 vec){

    }
}
