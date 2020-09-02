using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class SceneName : SceneScript {

        //----------変数----------

        [SerializeField] private CanvasGroup mCanvasGroup = null;
        [SerializeField] private InputField mInputField1 = null;
        [SerializeField] private InputField mInputField2 = null;
        [SerializeField] private InputField mInputField3 = null;
        [SerializeField] private Button mButton_Back = null;
        [SerializeField] private Button mButton_OK = null;
        [SerializeField] private Developer mDeveloper1 = null;
        [SerializeField] private Developer mDeveloper2 = null;
        [SerializeField] private Developer mDeveloper3 = null;

        private bool Enabled = false;

        //----------定数----------

        private const float OK_BUTTON_SHOW_TIME = 0.5f;
        private const float OK_BUTTON_HIDE_TIME = 0.5f;

        //----------メソッド----------

        private void Start() {
            mInputField1.onValueChanged.AddListener((value) => {
                OKButtonCheck();
                mDeveloper1.Init(value);
            });

            mInputField2.onValueChanged.AddListener((value) => {
                OKButtonCheck();
                mDeveloper2.Init(value);
            });

            mInputField3.onValueChanged.AddListener((value) => {
                OKButtonCheck();
                mDeveloper3.Init(value);
            });

            mButton_Back.onClick.AddListener(() => {
                MasterAnimation.Instance.FadeOut(mCanvasGroup);
                MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
                MasterAnimation.Instance.ButtonAnimation(mButton_Back.transform);
                MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Title);
            });

            mButton_OK.onClick.AddListener(() => {
                OKButtonPressed();
            });
        }

        private void Update() {
            if (Enabled) {
                if (Input.GetKeyDown("tab")) {
                    if (MasterGame.Instance.IsMulti) {
                        bool shift = Input.GetKey("left shift");
                        if (mInputField1.isFocused) {
                            if (shift) mInputField3.Select();
                            else mInputField2.Select();
                        } else if (mInputField2.isFocused) {
                            if (shift) mInputField1.Select();
                            else mInputField3.Select();
                        } else if (mInputField3.isFocused) {
                            if (shift) mInputField2.Select();
                            else mInputField1.Select();
                        } else {
                            mInputField1.Select();
                        }
                    } else {
                        mInputField1.Select();
                    }
                }
                if (Input.GetKeyDown("return") && mButton_OK.enabled) {
                    OKButtonPressed();
                }
            }
        }

        public override void LoadScene() {
            MasterAnimation.Instance.FadeIn(mCanvasGroup);
            mDeveloper1.Init("");
            mDeveloper2.Init("");
            mDeveloper3.Init("");
            mInputField1.text = "";
            mInputField2.text = "";
            mInputField3.text = "";

            mInputField2.gameObject.SetActive(MasterGame.Instance.IsMulti);
            mInputField3.gameObject.SetActive(MasterGame.Instance.IsMulti);
            mDeveloper2.gameObject.SetActive(MasterGame.Instance.IsMulti);
            mDeveloper3.gameObject.SetActive(MasterGame.Instance.IsMulti);

            mInputField1.Select();

            mButton_OK.transform.localScale = Vector3.zero;
            mButton_OK.enabled = false;
            Enabled = true;
        }

        private void OKButtonCheck() {
            bool next;
            if (MasterGame.Instance.IsMulti) {
                next = (mInputField1.text.Length > 0 &&
                mInputField2.text.Length > 0 &&
                mInputField3.text.Length > 0 &&
                mInputField1.text != mInputField2.text &&
                mInputField1.text != mInputField3.text &&
                mInputField2.text != mInputField3.text);
            } else {
                next = mInputField1.text.Length > 0;
            }

            if (mButton_OK.enabled != next) {
                mButton_OK.DOKill();
                if (next) {
                    mButton_OK.transform.DOScale(1, OK_BUTTON_SHOW_TIME).SetEase(Ease.OutBounce);
                } else {
                    mButton_OK.transform.DOScale(0,OK_BUTTON_HIDE_TIME);
                }
                mButton_OK.enabled = next;
            }
        }

        private void OKButtonPressed() {
            Enabled = false;
            mButton_OK.enabled = false;
            MasterAnimation.Instance.FadeOut(mCanvasGroup);
            MasterAudio.Instance.PlaySE(MasterAudio.SE_ID.Button1);
            MasterAnimation.Instance.ButtonAnimation(mButton_OK.transform);
            MasterGame.Instance.DeveloperA.Init(mInputField1.text);
            MasterGame.Instance.DeveloperB.Init(mInputField2.text);
            MasterGame.Instance.DeveloperC.Init(mInputField3.text);
            MasterGame.Instance.LoadScene(MasterGame.Scene_ID.Produce);
        }

    }
}