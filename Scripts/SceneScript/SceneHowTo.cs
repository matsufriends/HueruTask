using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class SceneHowTo : SceneScript {

        //----------変数----------

        [SerializeField] private CanvasGroup mCanvasGroup = null;
        [SerializeField] private Button mButton_Back = null;
        [SerializeField] private RectTransform mContents = null;

        //----------メソッド----------

        private void Start() {
            mButton_Back.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Back.transform);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Title);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
            });
        }

        public override void LoadScene() {
            MasterAnimation.Instance.FadeIn(mCanvasGroup);
            mContents.anchoredPosition = Vector3.zero;
        }

    }
}
