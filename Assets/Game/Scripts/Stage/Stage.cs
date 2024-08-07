using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] Brick brickPrefab;
    [SerializeField] Transform parrentTf;
    private int maxBrick;
    private Dictionary<InGameColor, int> colorQuantity = new Dictionary<InGameColor, int>();

    [SerializeField]
    private int numberOfRow ;

    [SerializeField]
    private int numberOfCol ;

    private List<Brick> bricks = new List<Brick>();

    void Start()
    {
        OnInIt();
    }
    public void OnInIt()
    {
        maxBrick = numberOfCol * numberOfRow;
        int numberOfBrick = maxBrick / 4;
        for (int i = 1; i < 5; i++)
        {
            colorQuantity.Add((InGameColor)i, numberOfBrick);
        }
        GenerateBrick();
    }
    public void GenerateBrick()
    {
        int i = 0;
        int j = 0;
        int count = 1;
        while (count <= maxBrick)
        {
            InGameColor inGameColor = (InGameColor)Random.Range(1, 5);
            if (colorQuantity[inGameColor] > 0)
            {
                Vector3 newPos = new Vector3(i * Constants.OFFSET_BRICK_SPAWN_X, Constants.OFFSET_BRICK_SPAWN_Y, j * Constants.OFFSET_BRICK_SPAWN_Z);
                Brick newBrick = SimplePool.Spawn<Brick>(brickPrefab, newPos, Quaternion.identity, parrentTf);
                newBrick.SetData(inGameColor);
                newBrick.OnDespawn();
                bricks.Add(newBrick);
                colorQuantity[inGameColor]--;
                count++;
                i++;
                if (i == numberOfCol)
                {
                    j++;
                    i = 0;
                    if (j == numberOfRow) j = 0;
                }
            }
        }
       
       
    }
    public List<Brick> GetBricks()
    {
        return bricks;
    }


}
