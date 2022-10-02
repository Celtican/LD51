using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioRandomizer : MonoBehaviour
{
    public float randomPitchRange;
    public float randomVolumeRange;
    public AudioClip[] audioClips;

    private AudioSource audioSource;
    private float pitch;
    private float volume;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void RandomPlay() {
        if (audioClips.Length == 0) throw new Exception("AudioRandomizer requires at least one audio clip");
		audioSource.pitch = pitch + Random.Range(-randomPitchRange, randomPitchRange);
		audioSource.volume = volume + Random.Range(-randomVolumeRange, randomVolumeRange);
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
	}
}
