﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mengubah vector ke vector2
public static class ExtensionMethods {
	public static Vector2 toVector2(this Vector3 vec3)
	{
		return new Vector2(vec3.x, vec3.y);
	}

}
