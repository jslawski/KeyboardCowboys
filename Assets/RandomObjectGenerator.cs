using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectGenerator : MonoBehaviour {

	public GameObject objectToGenerate;
	public int numberOfObjectsToGenerate;
	//VIEWPORT SPACE
	public float minFieldX;
	public float maxFieldX;
	public float minFieldY;
	public float maxFieldY;

	public float minXDistanceFromOtherObject;
	public float minYDistanceFromOtherObject;

	private int maxNumberRetries = 20;

	public List<GameObject> instantiatedObjects;
	private List<Vector3> alreadyInstantiatedPositions;

	public GameObject parentObject;

	private void Start()
	{
		this.instantiatedObjects = new List<GameObject>();
		this.alreadyInstantiatedPositions = new List<Vector3>();
		//this.GenerateRandomObjects();
	}

	public void GenerateRandomObjects()
	{
		for (int num = 0; num < this.numberOfObjectsToGenerate; num++)
		{
			float xValue = Random.Range(this.minFieldX, this.maxFieldX);
			float yValue = Random.Range(this.minFieldY, this.maxFieldY);

			for (int i = 0; i < this.maxNumberRetries; i++)
			{
				bool collision = false;

				for (int j = 0; j < this.alreadyInstantiatedPositions.Count; j++)
				{
					if (Mathf.Abs(this.alreadyInstantiatedPositions[j].x - xValue) <= this.minXDistanceFromOtherObject &&
						Mathf.Abs(this.alreadyInstantiatedPositions[j].y - yValue) <= this.minYDistanceFromOtherObject)
					{
						Debug.LogError("COLLISION");
						collision = true;
						break;
					}
				}

				if (collision == true)
				{
					xValue = Random.Range(this.minFieldX, this.maxFieldX);
					yValue = Random.Range(this.minFieldY, this.maxFieldY);
				}
				else
				{
					Vector3 screenSpaceGenerationPosition = GameManager.instance.mainCamera.ViewportToScreenPoint(new Vector3(xValue, yValue, 0.0f));
					GameObject objectInstance = Instantiate(this.objectToGenerate, screenSpaceGenerationPosition, new Quaternion(), parentObject.transform) as GameObject;
					this.instantiatedObjects.Add(objectInstance);
					this.alreadyInstantiatedPositions.Add(new Vector3(xValue, yValue, 0f));
					break;
				}
			}
		}
	}

	public void ClearObjects()
	{
		foreach (GameObject thisObject in this.instantiatedObjects)
		{
			Destroy(thisObject);
		}

		this.instantiatedObjects = new List<GameObject>();
		this.alreadyInstantiatedPositions = new List<Vector3>();
	}
}
