using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public static void Spawn(Item item, Vector2 position)
	{
		Instantiate(item, new Vector3(position.x, 0, position.y), Quaternion.identity);
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}
