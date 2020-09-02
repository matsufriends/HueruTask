using UnityEngine;
using UnityEngine.UI;

namespace matsu.HueruTask {
    public class TestPlay : MonoBehaviour {

        //----------変数----------

        [SerializeField] private Text mText_Ranking = null;
        [SerializeField] private Text mText_GameName = null;
        [SerializeField] private Text mText_DeveloperName = null;
        [SerializeField] private Slider mSlider_Fun = null;
        [SerializeField] private Slider mSlider_Graphic = null;
        [SerializeField] private Slider mSlider_Sound = null;
        [SerializeField] private Slider mSlider_Control = null;
        [SerializeField] private Slider mSlider_World = null;
        [SerializeField] private Slider mSlider_Odd = null;

        //----------メソッド----------

        public void Init() {
            Set(MasterGame.Instance.Fun, MasterGame.Instance.Graphic, MasterGame.Instance.Sound, MasterGame.Instance.Control, MasterGame.Instance.World, MasterGame.Instance.Odd);
        }

        public void Set(int ranking, string gameName, string developerName, float fun, float graphic, float sound, float control, float world, float odd) {
            mText_Ranking.text = string.Format("{0}位", ranking);
            mText_GameName.text = gameName;
            mText_DeveloperName.text = developerName;
            Set(fun, graphic, sound, control, world, odd);
        }

        public void Set(float fun, float graphic, float sound, float control, float world, float odd) {
            mSlider_Fun.value = Score01(fun);
            mSlider_Graphic.value = Score01(graphic);
            mSlider_Sound.value = Score01(sound);
            mSlider_Control.value = Score01(control);
            mSlider_World.value = Score01(world);
            mSlider_Odd.value = Score01(odd);
        }

        private float Score01(float score) {
            return Mathf.Clamp(score / 5f, 0, 1);
        }
    }
}
