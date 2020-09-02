using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class MyTask : MonoBehaviour {

        //----------変数----------

        [SerializeField] private Button mButton = null;
        [SerializeField] private Text mText = null;

        private Vector2 offset = Vector2.zero;
        public Task_ID mTask_ID;

        public enum Task_ID {
            Fun, Graphic, Sound, Control, World, Odd, HP, Test, Bug
        }

        //----------プロパティ----------

        public Button.ButtonClickedEvent onClick { get { return mButton.onClick; } }

        public bool ButtonEnabled {
            set {
                mButton.enabled = value;
                mButton.image.raycastTarget = false;
                mButton.interactable = value;
            }
        }
        public bool Did { get; set; } = false;

        //----------メソッド----------

        public void Init() {
            Task_ID task_ID = (Task_ID)System.Enum.ToObject(typeof(Task_ID), Random.Range(0, 9));

            switch (task_ID) {
                case Task_ID.Fun:
                    mText.text = "楽しさ\n追求！";
                    break;
                case Task_ID.Graphic:
                    mText.text = "絵作り\n追求！";
                    break;
                case Task_ID.Sound:
                    mText.text = "サウンド\n追求！";
                    break;
                case Task_ID.Control:
                    mText.text = "操作性\n追求！";
                    break;
                case Task_ID.World:
                    mText.text = "雰囲気\n追求！";
                    break;
                case Task_ID.Odd:
                    mText.text = "斬新さ\n追求！";
                    break;
                case Task_ID.HP:
                    mText.text = "休養！";
                    break;
                case Task_ID.Test:
                    mText.text = "テスト\nプレイ！";
                    break;
                case Task_ID.Bug:
                    mText.text = "バグ\n修正！";
                    break;
            }
            mTask_ID = task_ID;
        }

        public void PointerDown() {
            if (Input.GetKey("w")) {
                MasterTask.Instance.TaskReleased(this, -1);
            } else if (Input.GetKey("a") && !MasterTask.Instance.DeveloperA_Working) {
                MasterTask.Instance.TaskReleased(this, 1);
            } else if (Input.GetKey("s") && MasterGame.Instance.IsMulti && !MasterTask.Instance.DeveloperB_Working) {
                MasterTask.Instance.TaskReleased(this, 2);
            } else if (Input.GetKey("d") && MasterGame.Instance.IsMulti && !MasterTask.Instance.DeveloperC_Working) {
                MasterTask.Instance.TaskReleased(this, 3);
            } else {
                offset = transform.localPosition - Input.mousePosition * 1280f / Screen.height;
                PointerDrag();
            }
        }

        public void PointerDrag() {
            if (!Did) {
                transform.localPosition = MasterTask.Clamp((Vector2)Input.mousePosition * 1280f / Screen.height + offset);
                transform.SetAsLastSibling();
                MasterTask.Instance.TaskMovingCheck(transform.localPosition);
            }
        }

        public void AddParameter(Developer dev) {
            float point = MasterParameter.Instance.TaskPoint * MyMath.Remap(dev.Concenctarion, 0, 100, MasterParameter.Instance.ConcentrationToPointRatioMin, 1);
            Vector3 pos = dev.transform.localPosition;
            switch (mTask_ID) {
                case Task_ID.Fun:
                    point *= ParameterToRatio(dev.Fun);
                    MasterGame.Instance.Fun += point;
                    MasterAnimation.Instance.Alert(pos, string.Format("楽しさ+{0:0.0}UP", point));
                    break;
                case Task_ID.Graphic:
                    point *= ParameterToRatio(dev.Graphic);
                    MasterGame.Instance.Graphic += point;
                    MasterAnimation.Instance.Alert(pos, string.Format("絵作り+{0:0.0}UP", point));
                    break;
                case Task_ID.Sound:
                    point *= ParameterToRatio(dev.Sound);
                    MasterGame.Instance.Sound += point;
                    MasterAnimation.Instance.Alert(pos, string.Format("サウンド+{0:0.0}UP", point));
                    break;
                case Task_ID.Control:
                    point *= ParameterToRatio(dev.Control);
                    MasterGame.Instance.Control += point;
                    MasterAnimation.Instance.Alert(pos, string.Format("操作性+{0:0.0}UP", point));
                    break;
                case Task_ID.World:
                    point *= ParameterToRatio(dev.World);
                    MasterGame.Instance.World += point;
                    MasterAnimation.Instance.Alert(pos, string.Format("雰囲気+{0:0.0}UP", point));
                    break;
                case Task_ID.Odd:
                    point *= ParameterToRatio(dev.Odd);
                    MasterGame.Instance.Odd += point;
                    MasterAnimation.Instance.Alert(pos, string.Format("斬新さ+{0:0.0}UP", point));
                    break;
                case Task_ID.HP:
                    dev.ConcentrationUp();
                    MasterAnimation.Instance.Alert(pos, string.Format("集中力UP", point));
                    break;
                case Task_ID.Test:
                    MasterAnimation.Instance.ShowTestPlay();
                    break;
                case Task_ID.Bug:
                    MasterAnimation.Instance.Alert(pos, "全体的にUP");
                    point /= 6f;
                    MasterGame.Instance.Fun += point;
                    MasterGame.Instance.Graphic += point;
                    MasterGame.Instance.Sound += point;
                    MasterGame.Instance.Control += point;
                    MasterGame.Instance.World += point;
                    MasterGame.Instance.Odd += point;
                    break;
            }
        }

        private float ParameterToRatio(int para) {
            return MyMath.Remap(para, 0, 100, MasterParameter.Instance.ParameterToPointRatioMin, 1);
        }

        public float WorkTime(Developer dev) {
            float parameter;
            switch (mTask_ID) {
                case Task_ID.Fun:
                    parameter = dev.Fun;
                    break;
                case Task_ID.Graphic:
                    parameter = dev.Graphic;
                    break;
                case Task_ID.Sound:
                    parameter = dev.Sound;
                    break;
                case Task_ID.Control:
                    parameter = dev.Control;
                    break;
                case Task_ID.World:
                    parameter = dev.World;
                    break;
                case Task_ID.Odd:
                    parameter = dev.Odd;
                    break;
                case Task_ID.HP:
                case Task_ID.Test:
                case Task_ID.Bug:
                default:
                    parameter = (dev.Fun + dev.Graphic + dev.Sound + dev.Control + dev.World + dev.Odd) / 6f;
                    break;
            }

            parameter = MyMath.Remap(parameter, 0, 100, MasterParameter.Instance.ParameterToPointRatioMin, 1);

            parameter *= MyMath.Remap(dev.Concenctarion, 0, 100, MasterParameter.Instance.ConcentrationToPointRatioMin, 1);

            return MyMath.Remap(parameter, 0, 1, MasterParameter.Instance.WorkTimeMax, MasterParameter.Instance.WorkTimeMin);
        }

    }
}