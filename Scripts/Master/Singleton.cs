using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    
    //----------変数----------

    public static T mInstance = null;

    //----------プロパティ----------

    public static T Instance {
        get {
            if (mInstance == null) {
                mInstance = (T)FindObjectOfType(typeof(T));
                if (mInstance == null) {
                    Debug.LogError("MasterAudioが存在しません");
                }
            }
            return mInstance;
        }
    }

}
