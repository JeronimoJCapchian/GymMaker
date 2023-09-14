using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GymMaker/GeneralDataBase")]
public class ObjectDatabase : ScriptableObject
{
    public List<Machine> machines;
    public List<Floors> floors;
}

[Serializable]
public class Machine
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public Sprite Icon { get; private set; }
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public MachineType Type { get; private set; }
}

public enum MachineType
{
    Push,
    Pull,
    Legs,
    Weights,
    Cardio
}
[Serializable]
public class Floors
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    // [field: SerializeField]
    // public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public Sprite Icon { get; private set; }
    [field: SerializeField]
    public Material Material { get; private set; }
}