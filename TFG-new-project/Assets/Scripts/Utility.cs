using UnityEngine;

public class Utility
{
	public static GameObject FindChildByTag(GameObject parent, string childTag) {
		{
			GameObject child = null;

			foreach (Transform transform in parent.transform)
			{
				if (transform.CompareTag(childTag))
				{
					child = transform.gameObject;
					break;
				}
			}

			if (child == null) {
				Debug.LogError("Could not find a child with tag: " + childTag + " in parent: " + parent.name);
			}

			return child;
		}
	}
}
