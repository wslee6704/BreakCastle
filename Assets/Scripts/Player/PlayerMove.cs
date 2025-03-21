using DG.Tweening;
using Unity.Collections;
using UnityEditor;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    public int playerIndexX = 0;
    private bool playerOnMove = false;



    public Vector2 targetPos;
    private PlayerSingleMoveFunction singleFunc;

    public int climbStatus = 2;//한번에 오를 수 있는 블럭 수

    void Start()
    {
        singleFunc = GetComponent<PlayerSingleMoveFunction>();
    }


    
    private void OnEnable()
    {
        RhythmManager.OnPlayerBeat += playerAction; // 박자 이벤트 구독
    }

    private void OnDisable()
    {
        RhythmManager.OnPlayerBeat -= playerAction; // 박자 이벤트 해제
    }

    void playerAction(string moveType){
        
        switch(moveType){
            
            case "move_A" :
                singleFunc.estimateNormalMove(-1);
                break;
            case "move_D" :
                singleFunc.estimateNormalMove(1);
                break;
            case "attack" :
                BlockManager.Instance.BreakHighestBlock(playerIndexX);
                break;
            default :
                break;
        }
        
    }
    public static (int, int) VecToIndex(Vector2 vec)
    {
        int x = Mathf.RoundToInt((vec.x + 2.34375f) / 0.9375f); // x 계산
        int y = Mathf.RoundToInt(-vec.y / 0.9375f);             // y 계산

        return (x, y);
    }
    public static Vector2 IndexToVec(int x, int y){
        return new Vector2(-2.34375f + x*0.9375f, 0 -y*0.9375f);
    }
    

    //플레이어가 갈 방향 찾기
    //플레이어 이동하는거 속도 정해주기
    //플레이어 이동함수 실행(update에서 해줘야한다)
    //내려갈때는 비트 멈춰야함..

//Getter&Setter
    public void PlayerOnMove(){
        this.playerOnMove = true;
    }

    public void PlayerOffMove(){
        this.playerOnMove = false;
    }

    public bool PlayerIsOnMove(){
        return this.playerOnMove;
    }
}
