using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterModel : Singleton<MasterModel> {

    //----------変数----------

    [SerializeField] private List<GameObject> mModel_List = null;

    //----------メソッド----------

    public GameObject GetModel(string name) {
        int count = mModel_List.Count;
        int hash = Mathf.Abs(name.GetHashCode());
        return mModel_List[hash % count];
    }

}
