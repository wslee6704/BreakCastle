using UnityEngine;

public class RotationTest : MonoBehaviour
{
    AudioSource audioSource;
    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        // 뮤트: true일 경우 소리가 나지 않음
        audioSource.mute = false;

        // 루핑: true일 경우 반복 재생
        audioSource.loop = false;

        // 자동 재생: true일 경우 자동 재생
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        RhythmManager.OnBeat += RotateNote; // 박자 이벤트 구독
    }

    private void OnDisable()
    {
        RhythmManager.OnBeat -= RotateNote; // 박자 이벤트 해제
    }

    void RotateNote(float num){
        transform.Rotate(Vector3.back * 45);
        audioSource.Play();
    }
}
