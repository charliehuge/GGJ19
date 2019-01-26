using System.Collections.Generic;
using UnityEngine;

public class Sampler : MonoBehaviour
{

    public bool test = false;

    [SerializeField] private Clock _clock;
    [SerializeField] private float _noteProbability = 0.25f;
    [SerializeField] private List<AudioClip> _clips;
    [SerializeField, Range(0f, 2f)] private double _attackTime;
    [SerializeField, Range(0f, 2f)] private double _releaseTime;
    [SerializeField, Range(1, 8)] private int _numVoices = 2;
    [SerializeField] private SamplerVoice _samplerVoicePrefab;

    private SamplerVoice[] _samplerVoices;
    private int _nextVoiceIndex;

    private void Awake()
    {
        _samplerVoices = new SamplerVoice[_numVoices];

        for (int i = 0; i < _numVoices; ++i)
        {
            SamplerVoice samplerVoice = Instantiate(_samplerVoicePrefab);
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;
            _samplerVoices[i] = samplerVoice;
        }
    }

    private void OnEnable()
    {
        if (_clock != null)
        {
            _clock.Ticked += OnTick;
        }
    }

    private void OnDisable()
    {
        if (_clock != null)
        {
            _clock.Ticked -= OnTick;
        }
    }

    private void OnTick(double tickTime)
    {
        float rand = Random.value;
        if (rand < _noteProbability)
        {
            Play(tickTime, 60, 1);
        }
    }

    private void Update()
    {
        if (test)
        {
            Play(0, 60, 0.2);
            test = false;
        }
    }

    private void Play(double tickTime, int midiNoteNumber, double duration)
    {
        int rand = Random.Range(0, _clips.Count);

        _samplerVoices[_nextVoiceIndex].Play(_clips[rand], 1.0f, tickTime, _attackTime, duration, _releaseTime);

        _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
    }
}
