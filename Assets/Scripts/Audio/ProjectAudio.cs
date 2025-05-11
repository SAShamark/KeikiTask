using System;
using System.Collections.Generic;
using System.Linq;
using Audio.Data;
using Services;
using Services.Storage;
using UI.Popups;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class ProjectAudio : MonoBehaviour, IAudioManager, ILoadingInitialization
    {
        [SerializeField]
        private AudioConfig _audioConfig;

        [SerializeField]
        private AudioContainer _audioContainer;

        private Dictionary<AudioMixerGroups, float> _volumeSettings = new();
        private IStorageService _storageService;

        private readonly Dictionary<AudioGroupType, AudioContainer> _audioGroupContainer = new();

        public event Action<float> OnMusicVolumeChanged;
        public event Action<float> OnSoundVolumeChanged;

        [Inject]
        private void Construct(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public int Priority => 1;

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            _volumeSettings =
                _storageService.LoadData(StorageConstants.AUDIO_SETTINGS, new Dictionary<AudioMixerGroups, float>());

            foreach (AudioGroup audioGroup in _audioConfig.AudioGroups)
            {
                AudioContainer audioContainer = Instantiate(_audioContainer, transform);

                audioContainer.Initialize(audioGroup.GroupType, audioGroup.AudioMixerGroup,
                    audioGroup.IsOneAudioSource);
                _audioGroupContainer.Add(audioGroup.GroupType, audioContainer);
            }

            bool isTurnOnMaster = Math.Abs(GetVolume(AudioMixerGroups.Master) - 1) < ValueConstants.EPSILON;
            MasterSwitcher(isTurnOnMaster);
            ChangeMusicVolume(GetVolume(AudioMixerGroups.Music));
            ChangeSoundVolume(GetVolume(AudioMixerGroups.Sounds));
        }

        public void Play(AudioGroupType audioGroupType, string name, Vector3 position = new())
        {
            AudioGroup audioGroup = _audioConfig.FindGroup(audioGroupType);
            AudioInfo audioInfo = audioGroup.FindAudioInfo(name);

            AudioSourceModel audioSourceModel = _audioGroupContainer[audioGroupType].GetAudioSourceModel(name);

            audioSourceModel.Source.clip = audioInfo.Clip;
            audioSourceModel.Source.volume = audioInfo.Volume;
            audioSourceModel.Source.loop = audioInfo.Loop;
            audioSourceModel.Source.mute = false;
            audioSourceModel.Source.Play();

            if (position != Vector3.zero)
            {
                audioSourceModel.transform.position = position;
            }
        }

        public void MuteSwitcher(AudioGroupType audioGroupType, string name, bool isMute)
        {
            AudioSourceModel audioSourceModel = _audioGroupContainer[audioGroupType].GetAudioSourceModel(name);

            if (audioSourceModel.Source.clip == null)
            {
                AudioGroup audioGroup = _audioConfig.FindGroup(audioGroupType);
                AudioInfo audioInfo = audioGroup.FindAudioInfo(name);
                audioSourceModel.Source.clip = audioInfo.Clip;
                audioSourceModel.Source.volume = audioInfo.Volume;
                audioSourceModel.Source.loop = audioInfo.Loop;
                audioSourceModel.Source.mute = false;
                audioSourceModel.Source.Play();
            }

            audioSourceModel.Source.mute = isMute;
        }

        public void MuteAllEffectSounds()
        {
            var groupsToMute = new[] { AudioGroupType.EffectSounds };

            foreach (AudioGroupType group in groupsToMute)
            {
                foreach (AudioSourceModel audioSource in _audioGroupContainer[group].AudioSources
                             .Where(audioSource => audioSource.Source != null))
                {
                    audioSource.Source.mute = true;
                }
            }
        }

        public float GetVolume(AudioMixerGroups audioMixerGroup) =>
            _volumeSettings.GetValueOrDefault(audioMixerGroup, 1f);

        public void MasterSwitcher(bool state)
        {
            float volume = state ? 1 : 0;
            ChangeVolume(AudioMixerGroups.Master, volume);
        }

        public void MusicSwitcher(bool state)
        {
            float volume = state ? 1 : 0;
            ChangeVolume(AudioMixerGroups.Music, volume);
            OnMusicVolumeChanged?.Invoke(volume);
        }

        public void SoundSwitcher(bool state)
        {
            float volume = state ? 1 : 0;
            ChangeVolume(AudioMixerGroups.Sounds, volume);
            ChangeVolume(AudioMixerGroups.UI, volume);
            OnSoundVolumeChanged?.Invoke(volume);
        }

        public void ChangeMusicVolume(float volume)
        {
            ChangeVolume(AudioMixerGroups.Music, volume);
            OnMusicVolumeChanged?.Invoke(volume);
        }

        public void ChangeSoundVolume(float volume)
        {
            ChangeVolume(AudioMixerGroups.Sounds, volume);
            ChangeVolume(AudioMixerGroups.UI, volume);
            OnSoundVolumeChanged?.Invoke(volume);
        }

        private void ChangeVolume(AudioMixerGroups type, float volume)
        {
            _audioConfig.AudioMixer.SetFloat(type.ToString(), SqrtToDecibel(volume));
            _volumeSettings[type] = volume;
        }

        private float SqrtToDecibel(float volume)
        {
            return Mathf.Lerp(AudioConstants.MIN_VOLUME, AudioConstants.MAX_VOLUME, Mathf.Sqrt(volume));
        }

        public void SaveAudioSettings()
        {
            _storageService.SaveData(StorageConstants.AUDIO_SETTINGS, _volumeSettings);
        }
    }
}