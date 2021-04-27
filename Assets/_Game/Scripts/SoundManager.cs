using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSourceBGM;

    [SerializeField] private AudioClip _dungeonBgmClip;
    [SerializeField] private AudioClip _shopBgmClip;
    [SerializeField] private AudioClip _bossBgmClip;
    [SerializeField] private AudioClip _victoryBgmClip;

    [SerializeField] private AudioClip[] _hitAudioClips;
    [SerializeField] private AudioClip[] _jumpAudioClips;
    [SerializeField] private AudioClip[] _pickUpAudioClips;
    [SerializeField] private AudioClip[] _stabAudioClips;
    [SerializeField] private AudioClip[] _explosionAudioClips;
    [SerializeField] private AudioClip[] _spawnAudioClips;
    [SerializeField] private AudioClip[] _rockFallAudioClips;
    [SerializeField] private AudioClip[] _attackAudioClips;
    [SerializeField] private AudioClip[] _openChestAudioClips;
    [SerializeField] private AudioClip[] _buyAudioClips;
    [SerializeField] private AudioClip[] _helmetOffClips;
    [SerializeField] private AudioClip[] _stairsClips;

    [SerializeField] private float _pickupSoundCooldown = .1f;
    private float _lastPickupSound;

    private void Awake() {
        Instance = this;
    }

    public void PlayHit() {
        var randomClip = _hitAudioClips[Random.Range(0, _hitAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }

    public void PlayJump() {
        var randomClip = _jumpAudioClips[Random.Range(0, _jumpAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }

    public void PlayPickUp() {
        if (_lastPickupSound + _pickupSoundCooldown >= Time.time) {
            return;
        }

        var randomClip = _pickUpAudioClips[Random.Range(0, _pickUpAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);

        _lastPickupSound = Time.time;
    }

    public void PlayStab() {
        var randomClip = _stabAudioClips[Random.Range(0, _stabAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }

    public void PlayExplosion() {
        var randomClip = _explosionAudioClips[Random.Range(0, _explosionAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }
    
    public void PlaySpawn() {
        var randomClip = _spawnAudioClips[Random.Range(0, _spawnAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }
    
    public void PlayRockFall() {
        var randomClip = _rockFallAudioClips[Random.Range(0, _rockFallAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }

    public void PlayAttack() {
        var randomClip = _attackAudioClips[Random.Range(0, _attackAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }

    public void PlayOpenChest() {
        var randomClip = _openChestAudioClips[Random.Range(0, _openChestAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }
    
    public void PlayBuy() {
        var randomClip = _buyAudioClips[Random.Range(0, _buyAudioClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }
    
    public void PlayHelmetOff() {
        var randomClip = _helmetOffClips[Random.Range(0, _helmetOffClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }
    
    public void PlayStairs() {
        var randomClip = _stairsClips[Random.Range(0, _stairsClips.Length)];
        _audioSource.PlayOneShot(randomClip);
    }

    public void PlayDungeonBGM() {
        _audioSourceBGM.clip = _dungeonBgmClip;
        PlayBGM();
    }

    public void PlayShopBGM() {
        _audioSourceBGM.clip = _shopBgmClip;
        PlayBGM();
    }
    
    public void PlayBossBGM() {
        _audioSourceBGM.clip = _bossBgmClip;
        PlayBGM();
    }
    
    public void PlayVictoryBGM() {
        _audioSourceBGM.clip = _victoryBgmClip;
        PlayBGM();
    }

    public void PlayBGM() {
        _audioSourceBGM.loop = true;
        _audioSourceBGM.Play();
    }

    public void FadeOutBGM(float duration = .45f) {
        _audioSourceBGM.DOFade(0, duration);
    }

    public void FadeInBGM(float duration = .45f) {
        _audioSourceBGM.DOFade(1, duration);
    }
}
