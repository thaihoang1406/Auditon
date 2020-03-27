using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip[] m_clips;

    SoundManager m_sm;

    private void Start() { m_sm = SoundManager.instance; }

    public void Play()
    {
        var clip = m_clips[Random.Range(0, m_clips.Length)];
        m_sm.PlaySingle(clip);
    }
}
