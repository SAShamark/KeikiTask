using System;
using Audio.Data;
using UnityEngine;

namespace Audio
{
    public interface IAudioManager
    {
        event Action<float> OnMusicVolumeChanged;
        event Action<float> OnSoundVolumeChanged;
        AudioSource Play(AudioGroupType audioGroupType, string name);
        void MuteSwitcher(AudioGroupType audioGroupType, string name, bool isMute);
        void MuteAllEffectSounds();
        
        float GetVolume(AudioMixerGroups audioMixerGroups);
        void MasterSwitcher(bool state);
        void MusicSwitcher(bool isMuted);
        void SoundSwitcher(bool isMuted);
        void ChangeMusicVolume(float volume);
        void ChangeSoundVolume(float volume);
        void SaveAudioSettings();
    }
}