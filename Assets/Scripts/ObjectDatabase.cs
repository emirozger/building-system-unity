﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ObjectDatabase", fileName = "ObjectDatabase", order = 0)]
public class ObjectDatabase : ScriptableObject
{
    public List<ObjectData> objectsData;
    
}


[System.Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}