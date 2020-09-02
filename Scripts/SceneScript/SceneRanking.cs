using NCMB;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class SceneRanking : SceneScript {

        //----------変数----------

        [SerializeField] private CanvasGroup mCanvasGroup = null;
        [SerializeField] private Button mButton_Back1 = null;
        [SerializeField] private Button mButton_Back2 = null;
        [SerializeField] private Button mButton_Single = null;
        [SerializeField] private Button mButton_Multi = null;
        [SerializeField] private TestPlay mRankingNode_Prefab = null;
        [SerializeField] private Transform mContent_Single = null;
        [SerializeField] private Transform mContent_Multi = null;
        [SerializeField] private Transform mParent = null;

        //----------メソッド----------

        private void Start() {

            mButton_Back1.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
                MasterAnimation.Instance.ButtonAnimation(mButton_Back1.transform);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Title);
            });

            mButton_Back2.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
                MasterAnimation.Instance.ButtonAnimation(mButton_Back2.transform);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Title);
            });

            mButton_Single.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.RankingAnimation(mParent, Vector3.zero);
                MasterAnimation.Instance.ButtonAnimation(mButton_Single.transform);
            });

            mButton_Multi.onClick.AddListener(() => {
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.RankingAnimation(mParent, -Vector3.right * 720);
                MasterAnimation.Instance.ButtonAnimation(mButton_Multi.transform);
            });

        }

        public override void LoadScene() {
            MasterAnimation.Instance.FadeIn(mCanvasGroup);

            for (int i = mContent_Single.childCount - 1; i >= 0; i--) {
                Destroy(mContent_Single.GetChild(i).gameObject);
            }

            for (int i = mContent_Multi.childCount - 1; i >= 0; i--) {
                Destroy(mContent_Multi.GetChild(i).gameObject);
            }

            NCMBQuery<NCMBObject> singleRanking = new NCMBQuery<NCMBObject>("RankingSingle");
            singleRanking.OrderByDescending("Total");

            singleRanking.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                if (e == null) {
                    for (int i = 0; i < objList.Count; i++) {
                        NCMBObject obj = objList[i];
                        string[] score = ((string)obj["Score"]).Split(',');
                        Instantiate(mRankingNode_Prefab, mContent_Single).Set(
                            i + 1, (string)obj["GameName"], (string)obj["DeveloperName"],
                            float.Parse(score[0]), float.Parse(score[1]), float.Parse(score[2]),
                            float.Parse(score[3]), float.Parse(score[4]), float.Parse(score[5]));
                    }
                }
            });

            NCMBQuery<NCMBObject> multiRanking = new NCMBQuery<NCMBObject>("RankingMulti");
            multiRanking.OrderByDescending("Total");

            multiRanking.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                if (e == null) {
                    for (int i = 0; i < objList.Count; i++) {
                        NCMBObject obj = objList[i];
                        string[] score = ((string)obj["Score"]).Split(',');
                        Instantiate(mRankingNode_Prefab, mContent_Multi).Set(
                            i + 1, (string)obj["GameName"], (string)obj["DeveloperName"],
                            float.Parse(score[0]), float.Parse(score[1]), float.Parse(score[2]),
                            float.Parse(score[3]), float.Parse(score[4]), float.Parse(score[5]));
                    }
                }
            });

        }

    }
}
