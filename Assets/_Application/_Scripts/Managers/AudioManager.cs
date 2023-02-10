using System.Collections;
using _Application.Scripts.Infrastructure.Services;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    public class AudioManager : MonoBehaviourService
    {
        [SerializeField] private AudioClip[] _backgroundClips;
        [SerializeField] private AudioClip _winClip;
        [SerializeField] private AudioClip _loseClip;
        [SerializeField] private AudioSource _audioBackSource;
        [SerializeField] private AudioSource _audioEffectsSource;
        
        private Coroutine _playAudioCor;

        public void StartPlayBack()
        {
            _audioBackSource.Play();
        }

        public override void Init()
        {
            base.Init();
            
            _audioBackSource.clip = _backgroundClips[0];
        }

        public void PlayEndgame(bool isWin)
        {
            _audioBackSource.Pause();
            _audioEffectsSource.clip = isWin ? _winClip : _loseClip;
            _playAudioCor = StartCoroutine(PlayAudio());
        }

        public void PlayBackgroundAgain()
        {
            StopCoroutine(_playAudioCor);
            
            _audioEffectsSource.Stop();
            _audioBackSource.Play();
        }

        private IEnumerator PlayAudio()
        {
            _audioEffectsSource.Play();
            while (_audioEffectsSource.isPlaying)
                yield return null;

            _audioEffectsSource.Stop();
            _audioBackSource.Play();
        }
    }
}