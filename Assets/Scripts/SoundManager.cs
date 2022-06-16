using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource _bgmAudioSrc;
    AudioSource _buyAudioSrc;
    AudioSource _defeatAudioSrc;
    AudioSource _upgradeAudioSrc;
    AudioSource _victoryAudioSrc;
    IDictionary<string, AudioSource> _attackAudioDictionary;

    public ConfigManager _cfgMgr;

    // Start is called before the first frame update
    void Start()
    {
        _cfgMgr = FindObjectOfType<ConfigManager>();
        InitAudioSrc();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 오디오 초기화
    void InitAudioSrc()
    {
        if (transform.Find(_cfgMgr.audSrcBGM) is not null)
            _bgmAudioSrc = transform.Find(_cfgMgr.audSrcBGM).gameObject.GetComponent<AudioSource>();
        if (transform.Find(_cfgMgr.audSrcBuy) is not null)
            _buyAudioSrc = transform.Find(_cfgMgr.audSrcBuy).gameObject.GetComponent<AudioSource>();
        if (transform.Find(_cfgMgr.audSrcDefeat) is not null)
            _defeatAudioSrc = transform.Find(_cfgMgr.audSrcDefeat).gameObject.GetComponent<AudioSource>();
        if (transform.Find(_cfgMgr.audSrcUpgrade) is not null)
            _upgradeAudioSrc = transform.Find(_cfgMgr.audSrcUpgrade).gameObject.GetComponent<AudioSource>();
        if (transform.Find(_cfgMgr.audSrcVictory) is not null)
            _victoryAudioSrc = transform.Find(_cfgMgr.audSrcVictory).gameObject.GetComponent<AudioSource>();
        if (transform.Find(_cfgMgr.audSrcAttack) is not null)
        {
            _attackAudioDictionary = new Dictionary<string, AudioSource>();
            _attackAudioDictionary.Add(UNIT_CLASS.ARCHER.ToString(), transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(UNIT_CLASS.ARCHER.ToString()).gameObject.GetComponent<AudioSource>());
            _attackAudioDictionary.Add(UNIT_CLASS.SWORD.ToString(), transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(UNIT_CLASS.SWORD.ToString()).gameObject.GetComponent<AudioSource>());
            _attackAudioDictionary.Add(UNIT_CLASS.WIZARD.ToString(), transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(UNIT_CLASS.WIZARD.ToString()).gameObject.GetComponent<AudioSource>());
            _attackAudioDictionary.Add(UNIT_CLASS.GUARD.ToString(), transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(UNIT_CLASS.GUARD.ToString()).gameObject.GetComponent<AudioSource>());
        }
    }

    // 오디오 play
    public void Play(string gameObject)
    {
        if (transform.Find(gameObject) is not null)
            transform.Find(gameObject).GetComponent<AudioSource>().Play();
    }

    public void Play(UNIT_CLASS unitClass)
    {
        transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(unitClass.ToString()).gameObject.GetComponent<AudioSource>().Play();
    }

    // 오디오 stop
    public void Stop(string gameObject)
    {
        if (transform.Find(gameObject) is not null)
            transform.Find(gameObject).GetComponent<AudioSource>().Stop();
    }

    public void Stop(UNIT_CLASS unitClass)
    {
        transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(unitClass.ToString()).gameObject.GetComponent<AudioSource>().Stop();
    }

    // 오디오 pause
    public void Pause(string gameObject)
    {
        if (transform.Find(gameObject) is not null)
            transform.Find(gameObject).GetComponent<AudioSource>().Pause();
    }

    public void Pause(UNIT_CLASS unitClass)
    {
        transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(unitClass.ToString()).gameObject.GetComponent<AudioSource>().Pause();
    }

    // 오디오 unpause
    public void UnPause(string gameObject)
    {
        if (transform.Find(gameObject) is not null)
            transform.Find(gameObject).GetComponent<AudioSource>().UnPause();
    }

    public void UnPause(UNIT_CLASS unitClass)
    {
        transform.Find(_cfgMgr.audSrcAttack).gameObject.transform.Find(unitClass.ToString()).gameObject.GetComponent<AudioSource>().UnPause();
    }
}
