using UnityEngine;
using UnityEngine.Pool;

public class NoteManager : MonoBehaviour
{
    [Header("references")]
    public GameObject[] gameObjects;
    private void OnEnable()
    {
        RhythmManager.OnBeat += UpdateNoteSpeed;
    }

    private void OnDisable()
    {
        RhythmManager.OnBeat -= UpdateNoteSpeed;
    }

    private void UpdateNoteSpeed(float Bpm)
    {
        for(int i = 0; i< gameObjects.Length;i++){
            if(gameObjects[i].GetComponent<Note>().checkMoveOn() == false){
                if(gameObjects[i].GetComponent<Note>().isInstantiated){

                }else{
                    Instantiate(gameObjects[i], transform.position, Quaternion.identity);
                }
                gameObjects[i].GetComponent<Note>().setSpeedAndMoveOn();
                return;
            }
        }
    }
}
