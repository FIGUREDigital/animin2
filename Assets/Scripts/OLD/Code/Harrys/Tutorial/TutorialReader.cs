using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using System.Xml.Serialization;
using System.Xml;
using System.IO;






[Serializable()]
[System.Xml.Serialization.XmlRoot("ArrayOfTutorials")]
public class ArrayOfTutorials{
	[XmlArray("Tutorials")]
	[XmlArrayItem("Tutorial", typeof(Tutorial))]
	public Tutorial[] Tutorials{ get; set; }
}

[Serializable()]
public class Condition{ 

    public enum Type {None,Initial,Timer,Button,Collider,AdHoc};
    public Type type
    {
        get
        {
            if (Initial != null)
                return Type.Initial;
            if (Timer != null)
                return Type.Timer;
            if (ButtonCond != null)
                return Type.Button;
            if (AdHocCond != null)
                return Type.AdHoc;
            return Type.None;
        }
    }



    [XmlElement("Initial", typeof(InitialCond))]
    public InitialCond Initial{ get; set; }
    [XmlElement("Timer", typeof(TimerCond))]
    public TimerCond Timer{ get; set; }
    [XmlElement("Button", typeof(ButtonCond))]
    public ButtonCond ButtonCond{ get; set; }
    [XmlElement("AdHoc", typeof(AdHocCond))]
    public AdHocCond AdHocCond{ get; set; }


    //[XmlElement("Button", typeof(TutorialCond))]
    //public TutorialCond TutorialCondition{ get; set; }
}

[Serializable()]
public class TutorialCond{}
[Serializable()]
public class InitialCond:TutorialCond{}
[Serializable()]
public class TimerCond:TutorialCond{
    [System.Xml.Serialization.XmlAttribute("trigger")]
    public string trigger { get; set; }
    public int trigi
    {
        get
        {
            return UInt16.Parse(trigger);
        }
    }
    [System.Xml.Serialization.XmlAttribute("seconds")]
    public string seconds { get; set; }
    public float secf
    {
        get
        {
            return float.Parse(seconds);
        }
    }
}
[Serializable()]
public class ButtonCond:TutorialCond{
    [System.Xml.Serialization.XmlAttribute("name")]
    public string name {get;set;}
}
[Serializable()]
public class AdHocCond:TutorialCond{
    [System.Xml.Serialization.XmlAttribute("call")]
    public string call {get;set;}
}

[Serializable()]
public class Tutorial{

    [XmlElement("StartCondition", typeof(Condition))]
    public Condition Condition{ get; set; }

	[XmlArray("Lessons")]
	[XmlArrayItem("Lesson", typeof(Lesson))]
	public Lesson[] Lessons{ get; set; }
	[System.Xml.Serialization.XmlAttribute("id")]
    public string id { get; set; }
    [System.Xml.Serialization.XmlAttribute("name")]
    public string Name { get; set; }

	public int id_num
    {
        get
        {
            return UInt16.Parse(id);
        }
    }
}




[Serializable()]
public class Lesson{
	[XmlArray("TutEntries")]
	[XmlArrayItem("TutEntry", typeof(TutEntry))]
	public TutEntry[] TutEntries{ get; set; }

    [XmlElement("EndCondition", typeof(Condition))]
    public Condition EndCondition{ get; set; }
}
[Serializable()]
public class TutEntry{
	[System.Xml.Serialization.XmlAttribute("text")]
	public string text { get; set; }
}






public class TutorialReader{

	private ArrayOfTutorials m_Tutorials;
	public Tutorial[] Tutorials{ get { return m_Tutorials.Tutorials; } }
	private bool[] m_Finished;
	//public bool[] TutorialFinished{ get { return m_Finished; } }

	
	private const string FILENAME = "Assets/Resources/Tutorials.xml";

	public void Deserialize(bool print = false)
	{
		string filename = "Tutorials";
		TextAsset textAsset = (TextAsset)Resources.Load(filename, typeof(TextAsset));

		if(textAsset == null)
		{
			Debug.LogError("Could not load text asset " + filename);
		}

		string ToString = textAsset.ToString ();
		
		Debug.Log ("-DESERIALIZING---------------------------------------------------------------");
		ArrayOfTutorials data = null;
		
		XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfTutorials));
		
		//StreamReader reader = new StreamReader(ToString);
		//data = (ArrayOfTutorials)serializer.Deserialize(reader);
		//reader.Close();

		StringReader stringReader = new StringReader(ToString);
		XmlTextReader xmlReader = new XmlTextReader(stringReader);
		data = (ArrayOfTutorials)serializer.Deserialize(xmlReader);

		xmlReader.Close();
		stringReader.Close();
		
		//return data;
		
		m_Tutorials = data;
		//m_Finished = new bool[m_Tutorials.Tutorials.Length];

		if (print) test ();
	}











	public void test(){
		ArrayOfTutorials t = m_Tutorials;
		Debug.Log ("Counted : ["+t.Tutorials.Length+"] tutorials;");
		for (int i = 0; i < t.Tutorials.Length; i++){
			
			Debug.Log (" . Counted : ["+t.Tutorials[i].Lessons.Length+"] lessons;");
			for (int j = 0; j < t.Tutorials[i].Lessons.Length; j++){
				
				Debug.Log (" . . Counted : ["+t.Tutorials[i].Lessons[j].TutEntries.Length+"] entries;");
				for (int k = 0; k < t.Tutorials[i].Lessons[j].TutEntries.Length; k++){
					
					Debug.Log (" . . TutName : ["+t.Tutorials[i].id+"]; TutEntry : ["+t.Tutorials[i].Lessons[j].TutEntries[k].text+"];");
				}
			}
		}
	}






	#region Singleton
	private static TutorialReader s_Instance;
	
	public static TutorialReader Instance
	{
		get
		{
			if ( s_Instance == null )
			{
				s_Instance = new TutorialReader();
			}
			return s_Instance;
		}
	}
	#endregion
}














