using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticles : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem system;
	[SerializeField]
	private List<Material> materials;

	public HitParticles Init(int characterIndex)
	{
		system.GetComponent<Renderer>().material = materials[characterIndex];
		return this;
	}

	public void Emit(int count) =>
		system.Emit(count);
}
