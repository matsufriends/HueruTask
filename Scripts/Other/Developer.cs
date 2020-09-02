using matsu.HueruTask;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Developer : MonoBehaviour {

    //----------変数----------

    [SerializeField] public Image mImage = null;
    [SerializeField] private Text mText_Name = null;
    [SerializeField] private Text mText_Concentration = null;
    [SerializeField] private Text mText_Fun = null;
    [SerializeField] private Text mText_Graphic = null;
    [SerializeField] private Text mText_Sound = null;
    [SerializeField] private Text mText_Control = null;
    [SerializeField] private Text mText_World = null;
    [SerializeField] private Text mText_Odd = null;

    //----------プロパティ----------

    public string Name { get { return mText_Name.text; } }
    public GameObject CharacterModel { get; set; } = null;
    public bool Active { set { gameObject.SetActive(value); } }
    public int Fun { get; private set; } = 50;
    public int Graphic { get; private set; } = 50;
    public int Sound { get; private set; } = 50;
    public int Control { get; private set; } = 50;
    public int World { get; private set; } = 50;
    public int Odd { get; private set; } = 50;
    public float Concenctarion { get; private set; } = 100;

    //----------メソッド----------

    public void Init(string name) {
        mText_Name.text = name;

        if (name.Length == 0) {
            mText_Fun.text = "楽しさ：N";
            mText_Graphic.text = "絵作り：N";
            mText_Sound.text = "サウンド：N";
            mText_Control.text = "操作性：N";
            mText_World.text = "雰囲気：N";
            mText_Odd.text = "斬新さ：N";
        } else {
            Random.InitState(name.GetHashCode());

            Fun = Random.Range(1, 101);
            Graphic = Random.Range(1, 101);
            Sound = Random.Range(1, 101);
            Control = Random.Range(1, 101);
            World = Random.Range(1, 101);
            Odd = Random.Range(1, 101);
            Concenctarion = 100;

            mText_Fun.text = string.Format("楽しさ：{0}", Status(Fun));
            mText_Graphic.text = string.Format("絵作り：{0}", Status(Graphic));
            mText_Sound.text = string.Format("サウンド：{0}", Status(Sound));
            mText_Control.text = string.Format("操作性：{0}", Status(Control));
            mText_World.text = string.Format("雰囲気：{0}", Status(World));
            mText_Odd.text = string.Format("斬新さ：{0}", Status(Odd));
            mText_Concentration.text = string.Format("集中力：{0}%", Concenctarion);
        }
    }

    private string Status(int value) {
        if (value < 15) return "F";
        else if (value < 29) return "E";
        else if (value < 43) return "D";
        else if (value < 57) return "C";
        else if (value < 71) return "B";
        else if (value < 85) return "A";
        else return "S";
    }

    public void DestroyCharacter() {
        if (CharacterModel != null) {
            Destroy(CharacterModel);
            CharacterModel = null;
        }
    }

    public void ConcentrationReset() {
        Concenctarion = 100;
        mText_Concentration.text= string.Format("集中力：{0}%", (int)Concenctarion);
    }

    public void ConcentrationDown() {
        Concenctarion = Mathf.Clamp(Concenctarion - Time.deltaTime * MasterTask.Instance.TaskCount * MasterParameter.Instance.ConcentrationDownSpeed, 0, 100);
        mText_Concentration.text = string.Format("集中力：{0}%", (int)Concenctarion);
    }

    public void ConcentrationUp() {
        Concenctarion = Mathf.Clamp(Concenctarion + MasterParameter.Instance.ConcentrationAdd, 0, 100);
        mText_Concentration.text = string.Format("集中力：{0}%", (int)Concenctarion);
    }

}
