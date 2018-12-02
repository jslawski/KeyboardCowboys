using UnityEngine;
using System.Collections;

public class BounceText : MonoBehaviour
{
	public float minAmplitude = 1f;
	public float maxAmplitude = 5f;
	public float minTimeBeforeDirectionChange = 1f;
	public float maxTimeBeforeDirectionChange = 1.5f;

	public float finalAmplitude1;
	public float finalTime1;
	public float finalAmplitude2;
	public float finalTime2;

	private Coroutine bounceCoroutine;

	protected virtual void Awake()
	{
		finalAmplitude1 = Random.Range(this.minAmplitude, this.maxAmplitude);
		finalTime1 = Random.Range(this.minTimeBeforeDirectionChange, this.maxTimeBeforeDirectionChange);

		finalAmplitude2 = Random.Range(this.minAmplitude, this.maxAmplitude);
		finalTime2 = Random.Range(this.minTimeBeforeDirectionChange, this.maxTimeBeforeDirectionChange);
	}

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

		float timeElapsed = 0;

		Vector3 startPos = transform.position;

		while (true)
		{
			timeElapsed += Time.deltaTime;

			transform.position = startPos + Vector3.up * finalAmplitude1 * Mathf.Sin(2 * Mathf.PI * timeElapsed / finalTime1) + 
				Vector3.right * finalAmplitude2 * Mathf.Sin(2 * Mathf.PI * timeElapsed / finalTime2);

			yield return 0;
		}
	}
}