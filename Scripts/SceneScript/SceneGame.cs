using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class SceneGame : SceneScript {

        //----------変数----------

        [SerializeField] private CanvasGroup mCanvasGroup = null;
        [SerializeField] private Image mImage_Unity1Week = null;
        [SerializeField] private Slider mSlider_Day = null;
        [SerializeField] private Text mText_Day = null;

        //----------メソッド----------

        public override void LoadScene() {
            MasterCamera.Instance.ChangeCamera(MasterGame.Instance.IsMulti ? MasterCamera.Camera_ID.Multi : MasterCamera.Camera_ID.Single);
            MasterAudio.Instance.PlayBGM(MasterAudio.BGM_ID.InGame);
            MasterAnimation.Instance.FadeIn(mCanvasGroup);

            mSlider_Day.value = 0;
            SetDayText();
            MasterTask.Instance.Reset();
            MasterGame.Instance.ScoreReset();

            MasterGame.Instance.DeveloperB.Active = MasterGame.Instance.IsMulti;
            MasterGame.Instance.DeveloperC.Active = MasterGame.Instance.IsMulti;

            float nextTaskSpawnTime = NextTaskSpawnTime();
            bool isUpdate = false;

            DOTween.Sequence()
                .AppendInterval(1)
                .AppendCallback(() => MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Whistle))
                .Append(mImage_Unity1Week.DOFade(1, 0.8f))
                .Append(mImage_Unity1Week.DOFade(0, 0.8f))
                .AppendInterval(0.5f)
                .AppendCallback(() => {
                    MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.GongStart);
                    isUpdate = true;
                })
                .Append(mSlider_Day.DOValue(1, MasterParameter.Instance.DayTime * 7).SetEase(Ease.Linear))
                .OnUpdate(() => {
                    if (isUpdate) {
                        SetDayText();
                        if (Time.time > nextTaskSpawnTime) {
                            MasterTask.Instance.SpawnTask();
                            nextTaskSpawnTime = NextTaskSpawnTime();
                        }
                        MasterGame.Instance.DeveloperA.ConcentrationDown();
                        MasterGame.Instance.DeveloperB.ConcentrationDown();
                        MasterGame.Instance.DeveloperC.ConcentrationDown();
                    }
                })
                .OnComplete(() => {
                    MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.GongEnd);
                    DOTween.Sequence()
                       .Append(MasterAnimation.Instance.FadeOut(mCanvasGroup))
                       .AppendCallback(() => {
                           MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Result);
                       });
                });
        }

        private void SetDayText() {
            string dayText;
            float value = mSlider_Day.value;
            if (value < 1 / 7f) dayText = "１日目";
            else if (value < 2 / 7f) dayText = "２日目";
            else if (value < 3 / 7f) dayText = "３日目";
            else if (value < 4 / 7f) dayText = "４日目";
            else if (value < 5 / 7f) dayText = "５日目";
            else if (value < 6 / 7f) dayText = "６日目";
            else dayText = "７日目";

            if (dayText != mText_Day.text) {
                mText_Day.text = dayText;
                MasterAnimation.Instance.DayAnimation(mText_Day.transform);
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.DayChange);
            }
        }

        private float NextTaskSpawnTime() {
            return Time.time + MasterParameter.Instance.TaskSpawnInterval + Random.Range(0, MasterParameter.Instance.TaskSpawnYuragi);
        }



    }
}
