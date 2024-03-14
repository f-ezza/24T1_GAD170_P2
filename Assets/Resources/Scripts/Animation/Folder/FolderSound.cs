using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;


    /*
    []---PlaySlideSound---[]
    []Checks if the clip ("PaperSlide") is valid
    []If it is, assign the clip to the source and then play it
    []---Conclusion Of PlaySlideSound Method---[]
     */
    public void PlaySlideSound()
    {
        if (clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }
}
