using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum SFXType {Jump, Hit, Die} //임시 예시입니다 필요하신 sfx추가하시면 됩니다!
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip defalutBGMClip;

    //중복되는 사운드
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip dieClip;


    private void Start()
    {
        bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        PlayBGMSource(defalutBGMClip); //배경음 자동실행
    }
    public void PlayBGMSource(AudioClip audioClip)  //배경음악 교체시
    {
        if(audioClip==null) return;

        bgmSource.clip=audioClip;
        bgmSource.loop = true;

        bgmSource.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    //사운드만 갈경우
    public void PlaySFX(AudioClip audioClip) //효과음 교체시
    {
        if(audioClip==null) return;

        sfxSource.PlayOneShot(audioClip);
    }

    //중복되는 사운드 사용할 경우
    public void PlaySFX(SFXType type)
    {
        switch(type)
        {
            case SFXType.Jump: sfxSource.PlayOneShot(jumpClip); break;
            case SFXType.Hit:sfxSource.PlayOneShot(hitClip); break;
            case SFXType.Die:sfxSource.PlayOneShot(dieClip); break;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
