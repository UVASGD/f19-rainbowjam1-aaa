using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredPulse : PulseObj {

    public PulseObj parent;
    public float delay;

    //private Coroutine delayPulse;
    
    protected override void Start() {
        // Link to direct parent
        if(this.parent) this.parent.stagger += StaggeredHit;

        // Copy the pulse characteristics of root PulseObj
        PulseObj parent = this.parent;
        while (parent is StaggeredPulse)
            parent = ((StaggeredPulse)parent).parent;
        if (parent) {
            intensityMul = parent.intensityMul;
            frequency = parent.frequency;
            timing = parent.timing;
            easing = parent.easing;
            type = parent.type;
            majorScaleAmplitude = parent.majorScaleAmplitude;
            minorScaleAmplitude = parent.minorScaleAmplitude;

        }
    }

    public void Update() {
        if (parent) {
            intensityMul = parent.intensityMul;
            frequency = parent.frequency;
            timing = parent.timing;
            easing = parent.easing;
            type = parent.type;
            majorScaleAmplitude = parent.majorScaleAmplitude;
            minorScaleAmplitude = parent.minorScaleAmplitude;
        }
    }

    public void StaggeredHit() {
        StartCoroutine(DelayedPulse());
    }
    

    public IEnumerator DelayedPulse() {
        yield return new WaitForSeconds(delay);
        this.NoteHit();
    }

}
