using System.Collections;
using System.Collections.Generic;

using UnityEngine;



public delegate void MusicEvent();
public class SongController : MonoBehaviour {

    public static SongController instance;
    private bool init = false;

    public AudioSource song;
    public bool metronomeEnabled = false;

    public MusicEvent MajorNote;
    public MusicEvent MinorNote;
    public MusicEvent AnyNote;

    public MusicEvent OnPauseSong;
    public MusicEvent OnPlaySong;
    public MusicEvent OnRelocateTime;

    /// <summary> Coroutine that delays playing the song from start </summary>
    private Coroutine playInitiater;
    private float delayTime;
    //private float DelayTime => (float)(DateTime.Now - delayStartTime).TotalSeconds;
    private float PickupTime => MajorTime * pickupCount;

    [Header("Timing")]
    [Range(1, 400)] public float BPM = 120;
    [Tooltip("The number of frames to offset a rhythmic pulse")]
    public int pulseFrameOffset = 2;
    public int beatsPerBar = 4;
    public int measureUnit = 4;
    [Tooltip("The number of Measures to wait before playing")]
    public int pickupCount = 2;

    /// <summary> Interval time for a single measure </summary>
    public float MajorTime => beatsPerBar * A * P / playSpeed;
    /// <summary> Interval time for a single beat in measure </summary>
    public float MinorTime => A * P / playSpeed;
    float P => 1f / BPM;
    readonly float A = 60;

    public bool IsPlaying { get; private set; }
    public float PlayTime { get; private set; }
    [Range(.25f, 2)] public float playSpeed = 1;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        //trackingHolder = new GameObject("Tracking Points");
        song = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (IsPlaying) {
            PlayTime += Time.deltaTime;
            PulseCheck(PlayTime);

            if (!song.isPlaying) {
                PlayTime = 0;
                Pause();
            }
        } else if (playInitiater != null) {
            delayTime += Time.deltaTime;
            PulseCheck(delayTime);
        }
    }

    /// <summary>
    /// Checks if a rythmic pulse can be applied at the given time in seconds
    /// </summary>
    private void PulseCheck(float t0) {
        if ((t0 + pulseFrameOffset * Time.deltaTime) % MajorTime <= Time.deltaTime) {
            MajorNote?.Invoke();
            AnyNote?.Invoke();
        } else if ((t0 + pulseFrameOffset * Time.deltaTime) % MinorTime <= Time.deltaTime) {
            MinorNote?.Invoke();
            AnyNote?.Invoke();
        }
    }


    #region Play Controls
    /// <summary>
    /// Start playback information. Resets all previous data
    /// </summary>
    public void Begin() {

        ////init = true;
        song.time = PlayTime = 0;
        playInitiater = StartCoroutine(PlayDelay());
    }

    /// <summary>
    /// pauses playback
    /// </summary>
    public void Pause() {
        if (playInitiater != null) {
            // Paused During pickup
            StopCoroutine(playInitiater);
            playInitiater = null;
        } else {
            // Paused during song
            IsPlaying = false;
            song.Pause();
            OnPauseSong?.Invoke();
            print("Paused!");

            // quantize play time to the nearest measure
            PlayTime -= (PlayTime % MajorTime);
            song.time = PlayTime;

            //SaveSongData();
        }
    }

    /// <summary>
    /// continues playback at a quantized interval behind the current trackTime
    /// </summary>
    public void Continue() {
        if (!init) {
            Begin();
            return;
        }

        playInitiater = StartCoroutine(PlayDelay());
    }

    /// <summary>
    /// Start playback after a delay
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayDelay() {
        delayTime = 0;
        //StartCoroutine(BrickSpawner.instance.Reset());
        yield return new WaitForSeconds(PickupTime);

        IsPlaying = true;
        OnPlaySong?.Invoke();
        song.Play();
        playInitiater = null;
        print("Playing!");
    }

    #endregion
}
