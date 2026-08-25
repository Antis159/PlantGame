using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PotObject : MonoBehaviour
{
    public enum BuiltPotType
    {
        None_Hidden,
        None_Visible_Transparent,
        None_Visible_Build,
        Small,
        Big
    }
    [SerializeField] List<GlobalData.KeyValuePairV2<BuiltPotType, GameObject>> builtPot_ByType_List;
    public bool isPlanted;    
    // To track current pot only
    public BuiltPotType builtPotType;
    void Start()
    {
        WorldObjManager.AddPotObj(this);
        UpdatePotMesh_ByState();
    }
    public void UpdatePotMesh_ByState()
    {
        builtPot_ByType_List.ForEach( x => x.Value.SetActive(false));
        builtPot_ByType_List.First( x => x.Key == builtPotType).Value.SetActive(true);
    }
}
