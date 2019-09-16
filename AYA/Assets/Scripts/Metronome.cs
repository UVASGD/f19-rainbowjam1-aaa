using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour {

    public bool Active = true;

    [Header("Metronome")]
    [Tooltip("Prefab SFX_Obj for full measure tick")]
    public GameObject metronomeMajor;
    [Tooltip("Prefab SFX_Obj for beat tick")]
    public GameObject metronomeMinor;

    // Start is called before the first frame update
    void Start() {
        SongController.instance.MajorNote += MajorTick;
        SongController.instance.MinorNote += MinorTick;
    }

    // Update is called once per frame
    void Update() {

    }

    public void MajorTick() {
        if (!Active) return;
        SFX_Spawner.instance.SpawnFX(metronomeMajor, transform.position,
                   parent: transform);
    }

    public void MinorTick() {
        if (!Active) return;
        SFX_Spawner.instance.SpawnFX(metronomeMinor, transform.position,
                    parent: transform);
    }
}
