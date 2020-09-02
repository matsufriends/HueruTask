using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class SceneProduce : SceneScript {

        //----------変数----------

        [SerializeField] private CanvasGroup mCanvasGroup = null;
        [SerializeField] private Canvas mBackCanvas = null;
        [SerializeField] private Text mProgressText = null;
        [SerializeField] private Slider mProgressSlider = null;
        [SerializeField] private Text mText_DeveloperA = null;
        [SerializeField] private Text mText_DeveloperB = null;
        [SerializeField] private Text mText_DeveloperC = null;
        [SerializeField] private Color mColor_Transparent = Color.white;

        //----------定数----------

        public const string PROGRESS = "Progress";
        public const string SIT_DOWN = "SitDown";
        private const float LOADING_TEXT_INTERVAL = 4;
        private const float LOAD_TIME_SINGLE = 4;
        private const float LOAD_TIME_MULTI = 6;
        private const float SPAWN_TIME = 1.5f;
        private const float SPAWN_INTERVAL = 1;
        private const float UNLOAD_TIME = 1.5f;

        //----------プロパティ----------

        private Vector3 AnimStartPosA { get { return new Vector3(0, 3, 5); } }
        private Vector3 AnimStartPosB { get { return new Vector3(-1, 3, 5); } }
        private Vector3 AnimStartPosC { get { return new Vector3(1, 3, 5); } }

        private Vector3 AnimEndPosA { get { return new Vector3(0, -0.5f, 5); } }
        private Vector3 AnimEndPosB { get { return new Vector3(-1, -0.5f, 5); } }
        private Vector3 AnimEndPosC { get { return new Vector3(1, -0.5f, 5); } }

        private Vector3 GameStartPosB { get { return new Vector3(15.9f, 0.04f, -1.37f); } }
        private Vector3 GameStartPosA { get { return new Vector3(14.5f, 0.04f, -3.7f); } }
        private Vector3 GameStartPosC { get { return new Vector3(18.35f, 0.04f, -3.45f); } }

        private Quaternion GameStartQuaternionB { get { return Quaternion.identity; } }
        private Quaternion GameStartQuaternionA { get { return QuaternionY180; } }
        private Quaternion GameStartQuaternionC { get { return Quaternion.Euler(0, -198, 0); } }

        private Quaternion QuaternionY180 { get { return Quaternion.Euler(0, 180, 0); } }

        //----------メソッド----------

        public override void LoadScene() {
            mBackCanvas.planeDistance = 1;
            mProgressSlider.value = 0;

            mText_DeveloperA.text = MasterGame.Instance.DeveloperA.Name;
            mText_DeveloperB.text = MasterGame.Instance.DeveloperB.Name;
            mText_DeveloperC.text = MasterGame.Instance.DeveloperC.Name;

            mText_DeveloperA.color = mColor_Transparent;
            mText_DeveloperB.color = mColor_Transparent;
            mText_DeveloperC.color = mColor_Transparent;

            GameObject developerA = Instantiate(MasterModel.Instance.GetModel(MasterGame.Instance.DeveloperA.Name), AnimStartPosA, QuaternionY180);
            GameObject developerB = MasterGame.Instance.IsMulti ? Instantiate(MasterModel.Instance.GetModel(MasterGame.Instance.DeveloperB.Name), AnimStartPosB, QuaternionY180) : null;
            GameObject developerC = MasterGame.Instance.IsMulti ? Instantiate(MasterModel.Instance.GetModel(MasterGame.Instance.DeveloperC.Name), AnimStartPosC, QuaternionY180) : null;

            Tween nowLoading_TextAnimation_Loop =
                DOTween.To(() => 0, (x) => SetText(x), 1, LOADING_TEXT_INTERVAL).SetLoops(-1).SetEase(Ease.Linear);

            Sequence seq = DOTween.Sequence();

            seq
                .Append(MasterAnimation.Instance.FadeIn(mCanvasGroup))
                .AppendCallback(() => {
                    MasterCamera.Instance.ChangeCamera(MasterCamera.Camera_ID.Produce);
                    mBackCanvas.planeDistance = 10;
                    MasterGame.Instance.DeveloperA.DestroyCharacter();
                    MasterGame.Instance.DeveloperB.DestroyCharacter();
                    MasterGame.Instance.DeveloperC.DestroyCharacter();
                })
                .Join(mProgressSlider.DOValue(1, MasterGame.Instance.IsMulti ? LOAD_TIME_MULTI : LOAD_TIME_SINGLE))
                .Join(developerA.transform.DOMove(AnimEndPosA, SPAWN_TIME))
                .Join(mText_DeveloperA.DOFade(1, SPAWN_TIME));

            if (MasterGame.Instance.IsMulti) {
                seq
                    .Join(developerB.transform.DOMove(AnimEndPosB, SPAWN_TIME).SetDelay(SPAWN_INTERVAL))
                    .Join(mText_DeveloperB.DOFade(1, SPAWN_TIME));
                seq
                    .Join(developerC.transform.DOMove(AnimEndPosC, SPAWN_TIME).SetDelay(SPAWN_INTERVAL))
                    .Join(mText_DeveloperC.DOFade(1, SPAWN_TIME));
            }

            seq.OnComplete(() => {
                DOTween.Sequence()
                    .Append(MasterCamera.Instance.Transform.DORotate(new Vector3(45, 0, 0), UNLOAD_TIME))
                    .Join(mText_DeveloperA.DOFade(0, UNLOAD_TIME))
                    .Join(mText_DeveloperB.DOFade(0, UNLOAD_TIME))
                    .Join(mText_DeveloperC.DOFade(0, UNLOAD_TIME))
                    .OnComplete(() => {
                        nowLoading_TextAnimation_Loop.Kill();
                        mBackCanvas.planeDistance = 1;

                        developerA.transform.SetPositionAndRotation(GameStartPosA, GameStartQuaternionA);
                        developerA.GetComponent<Animator>().SetBool(SIT_DOWN, true);
                        developerB?.transform.SetPositionAndRotation(GameStartPosB, GameStartQuaternionB);
                        developerB?.GetComponent<Animator>().SetBool(SIT_DOWN, true);
                        developerC?.transform.SetPositionAndRotation(GameStartPosC, GameStartQuaternionC);
                        developerC?.GetComponent<Animator>().SetBool(SIT_DOWN, true);

                        MasterGame.Instance.DeveloperA.CharacterModel = developerA;
                        MasterGame.Instance.DeveloperB.CharacterModel = developerB;
                        MasterGame.Instance.DeveloperC.CharacterModel = developerC;

                        MasterAnimation.Instance.FadeOut(mCanvasGroup);
                        MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Game);
                    });
            });
        }

        private void SetText(float progress) {
            if (progress < 0.25f) {
                mProgressText.text = "Now Loading";
            } else if (progress < 0.5f) {
                mProgressText.text = "Now Loading.";
            } else if (progress < 0.75f) {
                mProgressText.text = "Now Loading..";
            } else {
                mProgressText.text = "Now Loading...";
            }
        }
    }
}