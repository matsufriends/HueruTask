using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAudio : Singleton<MasterAudio> {

    //----------変数----------

    [SerializeField] private AudioSource mSE_AudioSource = null;
    [SerializeField] private AudioSource mBGM_AudioSource = null;
    [SerializeField] private List<AudioClip> mSE_ClipList = null;
    [SerializeField] private List<AudioClip> mBGM_ClipList = null;

    public enum SE_ID {
        Button1,DayChange,TaskSpawn,TaskDelete,TaskConfrim,Whistle,GongStart,GongEnd
    }

    public enum BGM_ID {
        OutGame,InGame
    }

    //----------プロパティ----------

    public float Volume {
        set {
            mSE_AudioSource.volume = value;
            mBGM_AudioSource.volume = value;
        }
    }

    //----------メソッド----------

    public void PlaySE(SE_ID se_ID) {
        mSE_AudioSource.PlayOneShot(mSE_ClipList[Mathf.Clamp((int)se_ID, 0, mSE_ClipList.Count - 1)]);
    }

    public void PlayBGM(BGM_ID bgm_ID) {
        AudioClip clip = mBGM_ClipList[Mathf.Clamp((int)bgm_ID, 0, mBGM_ClipList.Count - 1)];
        if (mBGM_AudioSource.clip != clip) {
            mBGM_AudioSource.clip = clip;
            mBGM_AudioSource.Play();
        }
    }

}
