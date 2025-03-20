using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public Vector3 startPos;  // 노트의 시작 위치
    public Vector3 targetPos; // 중앙(타겟) 위치
    public float moveTime; // 이동 시간 (노트가 중앙까지 도달하는 시간)

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
}
