using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class MasterAnimation : Singleton<MasterAnimation> {

        //----------変数----------

        [SerializeField] private Text mText_Alert = null;
        [SerializeField] private CanvasGroup mAlertCanvasGroup = null;
        [SerializeField] private Transform mTestPlay_Parent = null;
        [SerializeField] private TestPlay mTestPlay = null;

        //----------定数----------

        private const float BUTTON_DURATION = 0.5f;
        private const float BUTTON_STRENGTH = 0.2f;
        private const float TASK_SPAWN_TIME = 0.5f;
        private const float TASK_DELETE_TIME = 0.5f;
        private const float TEXT_DAY_TIME = 1;
        private const float TEXT_ALERT_TIME = 1;
        private const float CANVAS_FADE_TIME = 1;
        private const float TESTPLAY_TIME = 4;
        private const float RANKING_TIME = 1;

        //----------メソッド----------

        public void ButtonAnimation(Transform t) {
            t.DOKill();
            t.localScale = Vector3.one;
            t.DOShakeScale(BUTTON_DURATION, BUTTON_STRENGTH);
        }

        public void DayAnimation(Transform t) {
            t.DOKill();
            t.localScale = Vector3.one * 2;
            t.DOScale(Vector3.one, TEXT_DAY_TIME);
        }

        public void TaskSpawn(Transform t) {
            t.DOKill();
            t.DOScale(Vector3.one, TASK_SPAWN_TIME);
        }

        public void DeleteAnimation(Transform t) {
            t.DOKill();
            t.DOScale(Vector3.zero, TASK_DELETE_TIME).OnComplete(() => {
                Destroy(t.gameObject);
            });
        }

        public void RankingAnimation(Transform t,Vector3 pos) {
            t.DOKill();
            t.DOLocalMove(pos, RANKING_TIME);
        }

        public Tween TaskWorkAnimation(Transform t, float task_Time) {
            t.DOKill();
            return t.DOScale(Vector3.zero, task_Time).SetEase(Ease.InQuad);
        }

        public Tween DeveloperWorkColorAnimation(Developer dev, float task_Time) {
            dev.mImage.DOKill();
            dev.mImage.color = MasterTask.Instance.mSeleceted_Color;
            return dev.mImage.DOColor(MasterTask.Instance.mNormal_Color, task_Time).SetEase(Ease.InExpo);
        }

        public void Alert(Vector3 pos, string text) {
            Text t = Instantiate(mText_Alert, mAlertCanvasGroup.transform);
            t.transform.localPosition = pos + new Vector3(0, 100, 0);
            t.text = text;
            DOTween.Sequence()
                .Append(t.transform.DOMoveY(50, TEXT_ALERT_TIME).SetRelative())
                .Join(t.DOFade(0, TEXT_ALERT_TIME))
                .OnComplete(() => {
                    Destroy(t.gameObject);
                });
        }

        public void ShowTestPlay() {
            TestPlay test = Instantiate(mTestPlay, mTestPlay_Parent);
            test.Init();
            test.transform.localPosition = new Vector3(700, 250, 0);
            test.transform.DOLocalMoveX(-700, TESTPLAY_TIME).SetEase(Ease.Linear)
                .OnComplete(() => {
                    Destroy(test.gameObject);
                });
        }

        public Tween FadeIn(CanvasGroup cg) {
            cg.DOKill();
            cg.blocksRaycasts = true;
            return cg.DOFade(1, CANVAS_FADE_TIME);
        }

        public Tween FadeOut(CanvasGroup cg) {
            cg.DOKill();
            cg.blocksRaycasts = false;
            return cg.DOFade(0, CANVAS_FADE_TIME);
        }
    }
}