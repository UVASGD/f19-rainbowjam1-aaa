using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PropertyType {
    Rotation,
    Scale,
}

public enum Timing {
    Major,
    Minor,
    MajorMinor,
    Quantized
}

public class PulseObj : MonoBehaviour {

    public float intensityMul = 1;
    public float frequency = 1;
    public Ease easing = Ease.InOutCubic;
    public PropertyType type;
    public Timing timing = Timing.MajorMinor;

    public MusicEvent stagger;
    

    [Header("Scaling Pulse (Local Space)")]
    public float majorScaleAmplitude = 1.25f;
    public float minorScaleAmplitude = 1.17f;
    //public AnimationCurve scaleFalloff;

    private float fallTime;
    private float scaleAmplitude;

    protected virtual void Start() {
        //Subscribe to events
        if(timing == Timing.MajorMinor || timing == Timing.Major)
            SongController.instance.MajorNote += MajorNote;
        if (timing == Timing.MajorMinor || timing == Timing.Minor)
            SongController.instance.MinorNote += MinorNote;
    }

    public void MajorNote() {
        scaleAmplitude = majorScaleAmplitude;
        NoteHit();
    }

    public void MinorNote() {
        scaleAmplitude = minorScaleAmplitude;
        NoteHit();
    }

    protected void NoteHit() {
        fallTime = 0;
        switch (type) {
            case PropertyType.Scale:
                transform.DOScale(intensityMul, frequency).SetEase(easing);
                stagger?.Invoke();
                break;

            case PropertyType.Rotation:
                Vector3 curRot = transform.rotation.eulerAngles;
                transform.DORotate(curRot + intensityMul * Vector3.forward, frequency).SetEase(easing);
                stagger?.Invoke();
                //transform.DORotate
                break;
        }
    }
}
