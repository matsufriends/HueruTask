using NCMB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class SceneResult : SceneScript {

        //----------変数----------

        [SerializeField] private CanvasGroup mCanvasGroup = null;
        [SerializeField] private TestPlay mTestPlay = null;
        [SerializeField] private InputField mInputField_Name = null;
        [SerializeField] private Button mButton_Publish = null;
        [SerializeField] private Button mButton_Retry = null;
        [SerializeField] private Button mButton_Private = null;
        [SerializeField] private Color mColor_Normal = Color.white;
        [SerializeField] private Color mColor_Selected = Color.gray;

        //----------メソッド----------

        private void Start() {
            mButton_Publish.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Publish.transform);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
                if (MasterGame.Instance.IsMulti) {
                    NCMBObject obj = new NCMBObject("RankingMulti");
                    obj.Add("GameName", mInputField_Name.text);
                    obj.Add("DeveloperName", string.Format("{0},{1},{2}", MasterGame.Instance.DeveloperA.Name, MasterGame.Instance.DeveloperB.Name, MasterGame.Instance.DeveloperC.Name));
                    obj.Add("Score", MasterGame.Instance.Score);
                    obj.Add("Total", MasterGame.Instance.Total);
                    obj.SaveAsync((NCMBException e) => {
                        MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Ranking);
                    });
                } else {
                    NCMBObject obj = new NCMBObject("RankingSingle");
                    obj.Add("GameName", mInputField_Name.text);
                    obj.Add("DeveloperName", MasterGame.Instance.DeveloperA.Name);
                    obj.Add("Score", MasterGame.Instance.Score);
                    obj.Add("Total", MasterGame.Instance.Total);
                    obj.SaveAsync((NCMBException e) => {
                        MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Ranking);
                    });
                }

            });

            mButton_Retry.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Retry.transform);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Game);
            });

            mButton_Private.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Private.transform);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Title);
            });

            mInputField_Name.onValueChanged.AddListener(SetButton);
        }

        public override void LoadScene() {
            MasterAnimation.Instance.FadeIn(mCanvasGroup);
            mInputField_Name.text = "";
            mInputField_Name.Select();
            SetButton("");
            mTestPlay.Init();
        }

        private void SetButton(string value) {
            if (value.Length > 0) {
                mButton_Publish.enabled = true;
                mButton_Publish.image.color = mColor_Normal;
            } else {
                mButton_Publish.enabled = false;
                mButton_Publish.image.color = mColor_Selected;
            }
        }

    }
}