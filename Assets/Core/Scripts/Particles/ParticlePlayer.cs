using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : KyawaLib.SingletonMonoBehaviour<ParticlePlayer>
{
    [SerializeField]
    GameObject m_hanabiraParticle = null;

    public void PlayHanabiraParticle(Vector3 pos)
    {
        Instantiate(m_hanabiraParticle, pos, Quaternion.identity);
    }
}
