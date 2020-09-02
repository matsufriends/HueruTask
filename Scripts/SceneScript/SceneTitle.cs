using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class SceneTitle : SceneScript {

        //----------変数----------

        [SerializeField] private CanvasGroup mCanvasGroup = null;
        [SerializeField] private Button mButton_Help = null;
        [SerializeField] private Button mButton_Single = null;
        [SerializeField] private Button mButton_Multi = null;
        [SerializeField] private Button mButton_Ranking = null;
        [SerializeField] private Slider mSlider_Volume = null;

        //----------メソッド----------

        private void Start() {
            mButton_Help.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Help.transform);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.HowTo);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
            });

            mButton_Single.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Single.transform);
                MasterGame.Instance.SetMode(MasterGame.Mode_ID.Single);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Name);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
            });

            mButton_Multi.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Multi.transform);
                MasterGame.Instance.SetMode(MasterGame.Mode_ID.Multi);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Name);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
            });

            mButton_Ranking.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Ranking.transform);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Ranking);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
            });

            mSlider_Volume.onValueChanged.AddListener((value) => {
                MasterAudio.Instance.Volume = value;
            });
        }

        public override void LoadScene() {
            MasterAudio.Instance.PlayBGM(MasterAudio.BGM_ID.OutGame);
            MasterCamera.Instance.ChangeCamera(MasterCamera.Camera_ID.Rotation);
            MasterAnimation.Instance.FadeIn(mCanvasGroup);
        }

    }
}