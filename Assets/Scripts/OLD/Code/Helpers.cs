using UnityEngine;
using System.Collections.Generic;

public static class Helpers 
{
	public static string ExtraFilename(string path)
	{
		for(int i=path.Length-1;i>=0;--i)
		{
			if(path[i] == '/' || path[i] == '\\')
			{
				return path.Substring(i + 1);
			}
		}

		return path;
	}
}
