using UnityEngine;
using System.Collections;

public class UtilsRollingAverage
{		
	public class RAFloat
	{
		public int Count
		{
			get {return samples.Length;}
		}
		private int ignoreLast = 0;
		private float[] samples;
		private float total;
		private int index = 0;
		public RAFloat(int numSamples, int ignoreLast)
		{
			this.ignoreLast = ignoreLast;
			samples = new float[numSamples];
			index = 0;
		}
		
		public void Start(float v)
		{			
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] = v;
			}
			total = v * samples.Length;
		}
		
		public float Average
		{
			get {return total / (float)(samples.Length - ignoreLast);}
		}
		
		public void Add(float v)
		{
			// remove oldest sample;
			total -= samples[index];
			// replace with latest sample;
			samples[index] = v;
			int ignored =index - ignoreLast;
			if (ignored < 0)
			{
				ignored += samples.Length;
			}
			total += samples[ignored];
			index++;
			if (index >= samples.Length)
			{
				index = 0;
			}
		}		

		public override string ToString()
		{
			string t = Average.ToString();
			for(int i = 1; i <= samples.Length; i++)
			{
				int ii = index - i;
				while (ii < 0)
				{
					ii += samples.Length;
				}
				t = t+", "+samples[ii];
			}
			return t;
		}

	}
	
	public class RAVector3
	{
		private int ignoreLast = 0;
		private Vector3[] samples;
		private Vector3 total;
		private int index = 0;
		public RAVector3(int numSamples, int ignoreLast)
		{
			this.ignoreLast = ignoreLast;
			samples = new Vector3[numSamples];
			index = 0;
		}
		
		public void Start(Vector3 v)
		{			
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] = v;
			}
			total = v * samples.Length;
		}
		
		public Vector3 Average
		{
			get {return total / (float)(samples.Length - ignoreLast);}
		}
		
		public void Add(Vector3 v)
		{
			// remove oldest sample;
			total -= samples[index];
			// replace with latest sample;
			samples[index] = v;
			int ignored =index - ignoreLast;
			if (ignored < 0)
			{
				ignored += samples.Length;
			}
			total += samples[ignored];
			index++;
			if (index >= samples.Length)
			{
				index = 0;
			}
		}		
	}
}
