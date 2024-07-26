using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BrickSO", menuName = "Brick")]
public class BrickSO : ScriptableObject
{
    public Material material;

    public int layerMask;

}
