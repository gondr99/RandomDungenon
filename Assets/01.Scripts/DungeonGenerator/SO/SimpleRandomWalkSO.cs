using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParam", menuName = "SO/SimpleRandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iteration = 10, walkLength = 10;
    public bool startRandomlyEachIteration = true;
}
