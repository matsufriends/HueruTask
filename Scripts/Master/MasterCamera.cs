using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCamera : Singleton<MasterCamera> {

    //----------変数----------

    [SerializeField] private Transform mCamera = null;
    private Camera_ID mCamera_ID = Camera_ID.Rotation;

    public Transform Transform {
        get { return mCamera; }
    }

    public enum Camera_ID {
        Rotation, Produce, Single, Multi
    }

    //----------メソッド----------

    public void ChangeCamera(Camera_ID id) {
        switch (id) {
            case Camera_ID.Rotation:
                mCamera.position = new Vector3(19, 2, -1.7f);
                Camera.main.fieldOfView = 60;
                break;
            case Camera_ID.Produce:
                mCamera.position = Vector3.zero;
                mCamera.eulerAngles = Vector3.zero;
                Camera.main.fieldOfView = 60;
                break;
            case Camera_ID.Single:
                mCamera.position = new Vector3(14.7f, 2.5f, -1.5f);
                Camera.main.fieldOfView = 60;
                break;
            case Camera_ID.Multi:
                mCamera.position = new Vector3(19.5f, 2.2f, -1.3f);
                Camera.main.fieldOfView = 90;
                break;
        }
        mCamera_ID = id;
        Update();
    }

    public void Update() {
        switch (mCamera_ID) {
            case Camera_ID.Rotation:
                float t = (Mathf.Sin(Time.time * 0.2f) + 1) / 2;
                mCamera.eulerAngles = new Vector3(20, Mathf.Lerp(200, 280, t), 0);
                break;
            case Camera_ID.Single:
                float t1 = (Mathf.Sin(Time.time * 0.2f) + 1) / 2;
                mCamera.eulerAngles = new Vector3(20, Mathf.Lerp(165, 195, t1), 0);
                break;
            case Camera_ID.Multi:
                float t2 = (Mathf.Sin(Time.time * 0.2f) + 1) / 2;
                mCamera.eulerAngles = new Vector3(30, Mathf.Lerp(210, 270, t2), 0);
                break;
        }
    }

}
