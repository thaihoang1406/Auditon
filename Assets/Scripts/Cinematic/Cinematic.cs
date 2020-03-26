using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class Cinematic : MonoBehaviour
{
    public string[] m_dancers;
    public string m_director;
    public CinemachineVirtualCamera[] m_cameras;
    // public PlayableDirector m_director;

    State m_state;

    private void Update()
    {
        switch(m_state)
        {
        case State.INITIALIZED:
            var first = GameObject.Find(m_dancers[0]).transform;
            m_cameras[0].m_Follow = first;
            m_cameras[m_cameras.Length - 1].m_Follow = first;
            for(var i = 1; i < m_dancers.Length; i++)   m_cameras[i].m_Follow = GameObject.Find(m_dancers[i]).transform;
            GameObject.Find(m_director).GetComponent<PlayableDirector>().Play();
            m_state = State.STARTED;
            break;
        }
    }

    enum State
    {
        INITIALIZED,
        STARTED
    }
}
