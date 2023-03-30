using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyPart : MonoBehaviour
{
	public ParticleSystem p;
	public float speed;
	public ParticleSystem.Particle[] particles;

	private bool move;

	private bool isActived;

	public GameObject Mesh;


	void Start()
	{
		p = GetComponent<ParticleSystem>();
		TriggerManager.Activation += ActivationParticle;
	}


	void Update()
	{
		if (move)
		{
			particles = new ParticleSystem.Particle[p.particleCount];

			p.GetParticles(particles);

			for (int i = 0; i <= particles.GetUpperBound(0); i++)
			{
				float step = speed *Time.deltaTime;
				particles[i].position = Vector3.MoveTowards(particles[i].position, GameManager.Instance.player.transform.position - transform.position + new Vector3(0, 1.25f, 0), step);
			}

			p.SetParticles(particles, particles.Length);
		}
	}

	void ActivationParticle(GameObject triggerObj)
    {
		if (triggerObj.name == gameObject.name && !isActived)
		{
			Mesh.SetActive(false);
			GameManager.Instance.currentState = GameManager.PlayerState.Wait;
			isActived = true;
			p.Play();
			StartCoroutine(ParticleDelay());
		}
    }

	IEnumerator ParticleDelay()
    {
		yield return new WaitForSeconds(1);
		move = true;
		yield return new WaitForSeconds(1.5f);
		move = false;
		GameManager.Instance.currentState = GameManager.PlayerState.Idle;
		TriggerManager.Activation -= ActivationParticle;
		Destroy(gameObject);
	}

    ~TeddyPart()
    {
		TriggerManager.Activation -= ActivationParticle;
	}
}
