using UnityEngine;
using System.Collections;

public class BounceText : MonoBehaviour
{
	public float minAmplitude = 1f;
	public float maxAmplitude = 5f;
	public float minTimeBeforeDirectionChange = 1f;
	public float maxTimeBeforeDirectionChange = 1.5f;

	private Coroutine bounceCoroutine;

	// Use this for initialization
	void OnEnable()
	{
		this.bounceCoroutine = StartCoroutine(BounceTextCoroutine());
	}

	void OnDisable()
	{
		StopCoroutine(this.bounceCoroutine);
	}

	IEnumerator BounceTextCoroutine()
	{
		float amplitude1 = Random.Range(this.minAmplitude, this.maxAmplitude);
		float timeBeforeDirectionChange1 = Random.Range(this.minTimeBeforeDirectionChange, this.maxTimeBeforeDirectionChange);

		float amplitude2 = Random.Range(this.minAmplitude, this.maxAmplitude);
		float timeBeforeDirectionChange2 = Random.Range(this.minTimeBeforeDirectionChange, this.maxTimeBeforeDirectionChange);

		float timeElapsed = 0;

		Vector3 startPos = transform.position;

		while (true)
		{
			timeElapsed += Time.deltaTime;

			transform.position = startPos + Vector3.up * amplitude1 * Mathf.Sin(2 * Mathf.PI * timeElapsed / timeBeforeDirectionChange1) + 
				Vector3.right * amplitude2 * Mathf.Sin(2 * Mathf.PI * timeElapsed / timeBeforeDirectionChange2);

			yield return 0;
		}
	}
}