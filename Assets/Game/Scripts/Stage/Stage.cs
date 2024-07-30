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

    void Start()
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
                Vector3 newPos = new Vector3(i * 0.8f, 0.05f, j * 0.4f);
                Brick newBrick = SimplePool.Spawn<Brick>(brickPrefab, newPos, Quaternion.identity, parrentTf);
                newBrick.SetData(inGameColor);
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


}
