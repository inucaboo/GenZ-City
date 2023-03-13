using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] nhacNen, sfxSounds;
    public AudioSource _nhacNen, _sfxSounds;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //không hủy đối tượng khi chuyển scene.
        }
        else //Nếu giá trị của Instance khác null, thì hủy đối tượng đang gọi hàm Awake().
        {
            Destroy(gameObject);
        }
    } //Điều này đảm bảo rằng chỉ có một đối tượng của AudioManager được tạo ra trong toàn bộ game và không bị hủy khi chuyển scene.


    private void Start()
    {
        PlayMusic("NhacChill");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(nhacNen, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Khong tim thay nhac");
        }
        else
        {
            _nhacNen.clip = s.clip;
            _nhacNen.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Khong tim thay nhac");
        }
        else
        {
            _sfxSounds.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        _nhacNen.mute = !_nhacNen.mute;
    }
    public void ToggleSFX()
    {
        _sfxSounds.mute = !_sfxSounds.mute;
    }

    public void MusicVolume(float volume)
    {
        _nhacNen.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        _sfxSounds.volume = volume;
    }
}
