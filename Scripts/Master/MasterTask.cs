using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class MasterTask : Singleton<MasterTask> {

        //----------変数----------

        [SerializeField] public Color mNormal_Color = Color.white;
        [SerializeField] public Color mSeleceted_Color = Color.gray;

        [SerializeField] private Transform mTask_Parent = null;
        [SerializeField] private MyTask mTask_Prefab = null;

        [SerializeField] private Image mImage_Delete = null;
        [SerializeField] private Image mImage_DeveloperA = null;
        [SerializeField] private Image mImage_DeveloperB = null;
        [SerializeField] private Image mImage_DeveloperC = null;

        private bool mDeleteNear = false;
        private bool mDeveloperANear = false;
        private bool mDeveloperBNear = false;
        private bool mDeveloperCNear = false;



        //----------定数----------

        private const float CONFIRM_DISTANCE = 120;
        private const float DEVELOPER_FRAME_OFFSET = 160;
        private const float MIN_X = -280;
        private const float MAX_X = 280;
        private const float MIN_Y = -560;
        private const float MAX_Y = 440;

        //----------プロパティ----------

        public int TaskCount { get; set; } = 0;
        private Vector3 Delete_Pos { get { return mImage_Delete.transform.localPosition; } }
        private Vector3 Developer1_Pos { get { return MasterGame.Instance.DeveloperA.transform.localPosition + Vector3.up * DEVELOPER_FRAME_OFFSET; } }
        private Vector3 Developer2_Pos { get { return MasterGame.Instance.DeveloperB.transform.localPosition + Vector3.up * DEVELOPER_FRAME_OFFSET; } }
        private Vector3 Developer3_Pos { get { return MasterGame.Instance.DeveloperC.transform.localPosition + Vector3.up * DEVELOPER_FRAME_OFFSET; } }
        private Vector3 RandomTaskPos { get { return new Vector3(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y), 0); } }
        public bool DeveloperA_Working { get; set; } = false;
        public bool DeveloperB_Working { get; set; } = false;
        public bool DeveloperC_Working { get; set; } = false;

        //----------メソッド----------

        private void Update() {
            if (Input.GetKeyDown("w")) SetColor(ref mDeleteNear, true, ref mImage_Delete);
            if (Input.GetKeyDown("a")) SetColor(ref mDeveloperANear, true, ref mImage_DeveloperA);
            if (Input.GetKeyDown("s")) SetColor(ref mDeveloperBNear, true, ref mImage_DeveloperB);
            if (Input.GetKeyDown("d")) SetColor(ref mDeveloperCNear, true, ref mImage_DeveloperC);

            if (Input.GetKeyUp("w")) SetColor(ref mDeleteNear, false, ref mImage_Delete);
            if (Input.GetKeyUp("a")) SetColor(ref mDeveloperANear, false, ref mImage_DeveloperA);
            if (Input.GetKeyUp("s")) SetColor(ref mDeveloperBNear, false, ref mImage_DeveloperB);
            if (Input.GetKeyUp("d")) SetColor(ref mDeveloperCNear, false, ref mImage_DeveloperC);
        }

        public void Reset() {
            for (int i = mTask_Parent.childCount - 1; i >= 0; i--) {
                Destroy(mTask_Parent.GetChild(i).gameObject);
            }
            TaskCount = 0;
        }

        public void SpawnTask() {
            MyTask newTask = Instantiate(mTask_Prefab, mTask_Parent);
            newTask.transform.localPosition = RandomTaskPos;
            newTask.Init();
            newTask.onClick.AddListener(() => TaskReleased(newTask));
            TaskCount++;
            MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.TaskSpawn);
        }

        public void TaskMovingCheck(Vector3 taskPos) {
            bool delete = false;
            bool dev1 = false;
            bool dev2 = false;
            bool dev3 = false;
            CheckNear(taskPos, ref delete, ref dev1, ref dev2, ref dev3);

            SetColor(ref mDeleteNear, delete, ref mImage_Delete);
            SetColor(ref mDeveloperANear, dev1, ref mImage_DeveloperA);
            SetColor(ref mDeveloperBNear, dev2, ref mImage_DeveloperB);
            SetColor(ref mDeveloperCNear, dev3, ref mImage_DeveloperC);
        }

        public void TaskReleased(MyTask task, int check = 0) {
            if (task.Did) return;
            if (check == 0) check = JudgeTaskNear(task);

            if (check != 0) {
                task.ButtonEnabled = false;
                TaskCount--;
                task.Did = true;

                switch (check) {
                    case -1:
                        task.transform.localPosition = Delete_Pos;
                        task.transform.SetParent(mImage_Delete.transform);
                        SetColor(ref mDeleteNear, false, ref mImage_Delete);
                        break;
                    case 1:
                        task.transform.localPosition = Developer1_Pos;
                        task.transform.SetParent(MasterGame.Instance.DeveloperA.transform);
                        DeveloperA_Working = true;
                        SetColor(ref mDeveloperANear, false, ref mImage_DeveloperA);
                        break;
                    case 2:
                        task.transform.localPosition = Developer2_Pos;
                        task.transform.SetParent(MasterGame.Instance.DeveloperB.transform);
                        DeveloperB_Working = true;
                        SetColor(ref mDeveloperBNear, false, ref mImage_DeveloperB);
                        break;
                    case 3:
                        task.transform.localPosition = Developer3_Pos;
                        task.transform.SetParent(MasterGame.Instance.DeveloperC.transform);
                        DeveloperC_Working = true;
                        SetColor(ref mDeveloperCNear, false, ref mImage_DeveloperC);
                        break;
                }

                if (check < 0) {
                    MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.TaskDelete);
                    MasterAnimation.Instance.DeleteAnimation(task.transform);
                } else {
                    MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.TaskConfrim);
                    Developer dev = MasterGame.Instance.GetDeveloper(check);
                    float workTime = task.WorkTime(dev);
                    DOTween.Sequence()
                        .Append(MasterAnimation.Instance.TaskWorkAnimation(task.transform, workTime))
                        .Join(MasterAnimation.Instance.DeveloperWorkColorAnimation(dev, workTime))
                        .OnComplete(() => {
                            task.AddParameter(dev);
                            Destroy(task.gameObject);
                            WorkFinish(check);
                        });
                }
            }
        }

        private void SetColor(ref bool baseBool, bool newBool, ref Image img) {
            if (baseBool != newBool) {
                baseBool = newBool;
                img.color = newBool ? mSeleceted_Color : mNormal_Color;
            }
        }

        private int JudgeTaskNear(MyTask task) {
            bool delete = false;
            bool dev1 = false;
            bool dev2 = false;
            bool dev3 = false;
            CheckNear(task.transform.localPosition, ref delete, ref dev1, ref dev2, ref dev3);

            if (delete) return -1;
            else if (dev1) return 1;
            else if (dev2) return 2;
            else if (dev3) return 3;
            else return 0;
        }

        private void CheckNear(Vector3 pos, ref bool delete, ref bool dev1, ref bool dev2, ref bool dev3) {
            delete = (Delete_Pos - pos).magnitude < CONFIRM_DISTANCE;

            dev1 = (Developer1_Pos - pos).magnitude < CONFIRM_DISTANCE
                && !DeveloperA_Working && !delete;

            dev2 = (Developer2_Pos - pos).magnitude < CONFIRM_DISTANCE
                 && !DeveloperB_Working && !dev1 && MasterGame.Instance.IsMulti;

            dev3 = (Developer3_Pos - pos).magnitude < CONFIRM_DISTANCE
                && !DeveloperC_Working && !dev2 && MasterGame.Instance.IsMulti;
        }

        public void WorkFinish(int num) {
            switch (num) {
                case 1:
                    DeveloperA_Working = false;
                    break;
                case 2:
                    DeveloperB_Working = false;
                    break;
                case 3:
                    DeveloperC_Working = false;
                    break;
            }
        }

        public static Vector3 Clamp(Vector3 pos) {
            return new Vector3(Mathf.Clamp(pos.x, MIN_X-100, MAX_X+100), Mathf.Clamp(pos.y, MIN_Y-100, MAX_Y+100), pos.z);
        }

    }
}