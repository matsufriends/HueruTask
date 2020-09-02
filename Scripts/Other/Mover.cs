using matsu.HueruTask;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    //----------変数----------

    [SerializeField] private Vector2 mDefault_Pos = Vector2.zero;

    private Vector2 offset = Vector3.zero;

    //----------メソッド----------

    public void Init() {
        transform.localPosition = mDefault_Pos;
    }

    public void PointerDown() {
        offset = transform.localPosition - Input.mousePosition * 1280f / Screen.height;
    }

    public void PointerDrag() {
        transform.localPosition = MasterTask.Clamp((Vector2)Input.mousePosition * 1280f / Screen.height + offset);
    }

}
