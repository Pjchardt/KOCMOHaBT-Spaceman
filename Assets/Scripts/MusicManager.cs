using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    AudioClip currentClip;

    bool oneActive;

    List<MusicObject> musicObjects = new List<MusicObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void NewMusic(AudioClip clip)
    {
        if (currentClip != null && currentClip == clip)
            return;

        for (int i = 0; i < musicObjects.Count; i++)
        {
            musicObjects[i].StartFadeOut();
        }
           

        GameObject obj = new GameObject();
       
        MusicObject temp = obj.AddComponent<MusicObject>();
        temp.StartFadeIn(clip);
        musicObjects.Add(temp);
    }

    public void RemoveMusic(MusicObject o)
    {
        musicObjects.Remove(o);
    }
}
