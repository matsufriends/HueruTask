using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace matsu.HueruTask {
    public class MasterGame : Singleton<MasterGame> {

        //----------変数----------

        [SerializeField] private List<SceneScript> mScene_List = null;
        [SerializeField] private Scene_ID mFirst_SceneID = Scene_ID.Title;
        [SerializeField] private Mode_ID mMode_ID = Mode_ID.Single;

        [SerializeField] private Developer mDeveloperA = null;
        [SerializeField] private Developer mDeveloperB = null;
        [SerializeField] private Developer mDeveloperC = null;

        public enum Scene_ID {
            Title, Name, Produce, Game, Result, Ranking,HowTo
        }

        public enum Mode_ID {
            Single, Multi
        }

        //----------プロパティ----------

        public bool IsMulti { get { return mMode_ID == Mode_ID.Multi; } }
        public Developer DeveloperA { get { return mDeveloperA; } }
        public Developer DeveloperB { get { return mDeveloperB; } }
        public Developer DeveloperC { get { return mDeveloperC; } }
        public float Fun { get; set; } = 0;
        public float Graphic { get; set; } = 0;
        public float Sound { get; set; } = 0;
        public float Control { get; set; } = 0;
        public float World { get; set; } = 0;
        public float Odd { get; set; } = 0;
        public string Score {
            get {
                return string.Format("{0},{1},{2},{3},{4},{5}",
                    Mathf.Clamp(Fun, 0, 5), Mathf.Clamp(Graphic, 0, 5), Mathf.Clamp(Sound, 0, 5),
                    Mathf.Clamp(Control, 0, 5), Mathf.Clamp(World, 0, 5), Mathf.Clamp(Odd, 0, 5));
            }
        }
        public string Total {
            get {
                float total = (Mathf.Clamp(Fun, 0, 5) + Mathf.Clamp(Graphic, 0, 5) + Mathf.Clamp(Sound, 0, 5)
                    + Mathf.Clamp(Control, 0, 5) + Mathf.Clamp(World, 0, 5) + Mathf.Clamp(Odd, 0, 5)) / 6f;
                return total.ToString();
            }
        }

        //----------メソッド----------

        private void Start() {
            LoadScene(mFirst_SceneID);
        }

        public void SetMode(Mode_ID mode_ID) {
            mMode_ID = mode_ID;
        }

        public void LoadScene(Scene_ID scene_ID) {
            mScene_List[(int)scene_ID].LoadScene();
        }

        public T GetSceneScript<T>(Scene_ID sceneID) where T : SceneScript {
            return (T)mScene_List[(int)sceneID];
        }

        public Developer GetDeveloper(int num) {
            switch (num) {
                case 1:
                    return mDeveloperA;
                case 2:
                    return mDeveloperB;
                case 3:
                default:
                    return mDeveloperC;
            }
        }

        public void ScoreReset() {
            Fun = 0;
            Graphic = 0;
            Sound = 0;
            Control = 0;
            World = 0;
            Odd = 0;
            DeveloperA.ConcentrationReset();
            DeveloperB.ConcentrationReset();
            DeveloperC.ConcentrationReset();
        }

    }
}