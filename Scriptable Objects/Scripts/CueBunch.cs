using UnityEngine;

[CreateAssetMenu(fileName = "Cue Bunch", menuName = "ScriptableObjects/CueBunch", order = 1)]
public class CueBunch : ScriptableObject
{
    [SerializeField] float spawnDelay;

    public SpawnCue[] cues;

    [ContextMenu("Set spawn delay for each cue")]
    void SetSpawnDelay(){
        foreach(var cue in cues){
            cue.delayBeforeSpawn = spawnDelay;
        }
        spawnDelay = 0;
    }
}
