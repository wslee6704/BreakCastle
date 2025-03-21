using System.Collections.Generic;
using UnityEngine;

public class BlockData
{
    public float height;
    public GameObject obj;
    int indexX;
    public int indexY;
    
    public BlockData(Vector2 vec, GameObject obj){
        this.height = vec.y;
        this.obj = obj;
        this.SetBlock(vec);
    }
    public BlockData(int x, int y, GameObject obj){
        this.indexX = x;
        this.indexY = y;
        this.obj = obj;
        this.SetBlock(IndexToVec(x,y));
    }

    void SetBlock(Vector2 vec){
        this.obj.transform.position = vec;
    }

    Vector2 IndexToVec(int x, int y){
        return new Vector2(-2.34375f + x*0.9375f, 0 -y*0.9375f);
    }
}

public class BlockManage
{
    public List<BlockData> ListBlockData = new List<BlockData>();

    public void InitBlockSet(int index, GameObject obj){
        for(int y = 0;y<8; y++){
            GameObject realObj = ObjectPoolManager.instance.GetObject(obj,Vector3.zero);
            ListBlockData.Add(new BlockData(index, y, realObj));
        }
    }

    void MakeNewBlock(Vector2 vec, GameObject obj){
        ListBlockData.Add(new BlockData(vec,obj));
    }

    public void BreakHighestOne(){
        ObjectPoolManager.instance.ReleaseObject(BlockManager.Instance.block,ListBlockData[0].obj);
        ListBlockData.RemoveAt(0);
    }
}

public class BlockManager : MonoBehaviour
{

    public static BlockManager Instance;
    //블럭을 관리하는 list
    BlockManage[] blockArray = new BlockManage[6];
    public GameObject block;

    [SerializeField]private float xPos = -2.34375f;
    [SerializeField]private float xDistance = 0.9375f;
    
    public int newY = 0;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
    }

    void Start()
    {
        //block =  Resources.Load<GameObject>("Block");
        for(int i = 0; i<blockArray.Length;i++){//초반 세팅
            blockArray[i] = new BlockManage();
            blockArray[i].InitBlockSet(i,ChooseBlockType());
        }
    }

    
    GameObject ChooseBlockType(){
        return block;
    }

    public int getHighestIndexY(int index){
        return blockArray[index].ListBlockData[0].indexY;
    }

    public void BreakHighestBlock(int index){
        blockArray[index].BreakHighestOne();
    }
}




