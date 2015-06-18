using UnityEngine;
using System.Collections;

public class UtilsVelocityEase : MonoBehaviour 
{
	public delegate void SetValueDelegate(Vector2 value, Object userObject);
	public SetValueDelegate setValueDelegate = null;
	public Object userObject = null;
	public float accelerate = 5000f;
	public float maxSpeed = 100000.0f;
	public Vector2 target;
	public Vector2 current;
	public Vector2 velocity;
	public Rect bounds;
	public bool useBounds;
	public bool useRealTime = true;
	private float lastTime = 0;
	public enum Modes
	{
		Scroll,
		LocalPosition,
		Position,
		Custom
	}

	public Modes mode = Modes.Scroll;

	// Use this for initialization
	void Awake () {
		enabled = false;
	}
		
	public void Begin(Vector2 target)
	{
		Begin(target, this.velocity);
	}

	public void Begin(Vector2 target, Vector2 velocity)
	{
		enabled = true;
		switch (mode)
		{
/*			case Modes.Scroll:
				LayoutBase lBase = GetComponent<LayoutBase>();
				if (lBase)
				{
					current = lBase.Offset;
				}
				break;*/
			case Modes.LocalPosition:
				current = transform.localPosition;
				break;
			case Modes.Position:
				current = transform.position;
				break;
		}
		this.velocity = velocity;
		this.target = target;
		lastTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update () 
	{
		float time = Time.deltaTime;
		if (useRealTime)
		{
			float now = Time.realtimeSinceStartup;
			time = now - lastTime;
			lastTime = now;
		}
		EaseTo(time, accelerate, target.x, ref this.current.x, ref this.velocity.x);
		EaseTo(time, accelerate, target.y, ref this.current.y, ref this.velocity.y);
		velocity.x = Mathf.Min(velocity.x, maxSpeed);
		velocity.x = Mathf.Max(velocity.x, -maxSpeed);
		velocity.y = Mathf.Min(velocity.y, maxSpeed);
		velocity.y = Mathf.Max(velocity.y, -maxSpeed);
		if (useBounds)
		{
			this.current.x = Mathf.Max(this.current.x, bounds.xMin);
			this.current.y = Mathf.Max(this.current.y, bounds.yMin);
			this.current.x = Mathf.Min(this.current.x, bounds.xMax);
			this.current.y = Mathf.Min(this.current.y, bounds.yMax);
		}
		switch (mode)
		{
/*			case Modes.Scroll:
				LayoutBase lBase = GetComponent<LayoutBase>();
				if (lBase)
				{
					lBase.Offset = new Vector3(current.x, current.y, lBase.Offset.z);
				}
				break;*/
			case Modes.LocalPosition:
				transform.localPosition = new Vector3(current.x, current.y, transform.localPosition.z);
				break;
			case Modes.Position:
				transform.position = new Vector3(current.x, current.y, transform.position.z);
				break;
		}
		if (setValueDelegate != null)
		{
			setValueDelegate.Invoke(current, userObject);
		}
		if((velocity.x == 0.0f)&&(velocity.y == 0.0f))
		{
			enabled = false;
		}
	}

	/// <summary>
	/// Ease to will use constant acceleration and deceleration to get from current to target.
	/// </summary>
	/// <param name="Time"></param>
	/// <param name="Accel"></param>
	/// <param name="Target"></param>
	/// <param name="Current"></param>
	/// <param name="Velocity"></param>
	void EaseTo(float time, float Accel, float Target, ref float Current, ref float Velocity)
	{
		if (Velocity == 0.0f)
		{
			if ((Current == Target) || (Accel == 0.0f)) return;
		}
		float  Accelerate = Accel;
		float Delta = Target - Current;
		if (Delta < 0)
		{
			Accelerate = - Accelerate;
		}

		// Calculate how long it is at Accel before we need to start decelerations
		float TimeToChange = -(Velocity/Accelerate) + Mathf.Sqrt((2.0f*Accelerate*Delta + Velocity*Velocity)/(2.0f *Accelerate*Accelerate));
		if (TimeToChange > 0)
		{
			if (time < TimeToChange)
			{
				TimeToChange = time;
			}
			// Accelerate first
			Current  += (Velocity+Accelerate*TimeToChange/2.0f)*TimeToChange;
			Velocity += Accelerate*TimeToChange;
			time-=TimeToChange;
		}

		if (time > 0.0f)
		{
			// Decelerate
			float TimeLeft = Velocity/Accelerate;
			if (TimeLeft <= time)
			{
				// We have reached our destination
				Current = Target;
				Velocity = 0.0f;
			}
			else
			{
				Current  += (Velocity-Accelerate*time/2.0f)*time;
				Velocity += -Accelerate*time;
			}
		}
	}

}
