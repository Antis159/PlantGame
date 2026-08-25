using System.Collections.Generic;
using UnityEngine;

public class WorldObjManager : MonoBehaviour
{ 
    [HideInInspector] public WorldObjManager instance;
    [SerializeField] GameObject interactableObjParent;
    [SerializeField] static List<PotObject> potObj_List = new List<PotObject>();
    void Awake()
    {
        instance = this;
    }
    public static void AddPotObj(PotObject potObj)
    {
        potObj_List.Add(potObj);
    }
}
