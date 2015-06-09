using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FEVersion : MonoBehaviour {

    public UIText all;
    public UIText summary;
    public UIText scmCommitId;
    public UIText buildNumber;
    public UIText buildStartTime;

	// Use this for initialization
	void Start ()
    {
        TextAsset manifest = (TextAsset)Resources.Load("UnityCloudBuildManifest.json");
        bool truncateCommitId = true;
        if (manifest == null)
        {
            manifest = (TextAsset)Resources.Load("Version");
            truncateCommitId = false;
        }
        if (manifest != null)
        {
            Dictionary<string, object> manifestDict = PhiMiniJSON.Deserialize(manifest.text) as Dictionary<string, object>;

            if (all != null)
            {
                string allText = "";
                foreach (KeyValuePair<string, object> kvp in manifestDict)
                {
                    if(kvp.Value != null)
                    {
                        allText += kvp.Key + ": " + kvp.Value.ToString() + "\n";
                    }
                }
                all.Text = allText;
            }

            string summaryText = "";
            object result;

            if (manifestDict.TryGetValue("buildNumber", out result))
            {
                string text = result.ToString();
                if (buildNumber)
                {
                    buildNumber.Text = text;
                }
                summaryText += " " + text;
            }

            if (manifestDict.TryGetValue("scmCommitId", out result))
            {
                string text = result.ToString();
                if (truncateCommitId)
                {
                    text = text.Substring(0, 7);
                }
                if (scmCommitId)
                {
                    scmCommitId.Text = text;
                }
                summaryText += " " + text;
            }
            if (manifestDict.TryGetValue("buildStartTime", out result))
            {
                string text = result.ToString();
                text = text.Replace("T", " ");
                text = text.Replace(".000Z", "");
                text = text.Replace("Z", "");
                if (buildStartTime)
                {
                    buildStartTime.Text = text;
                }
                summaryText += " " + text;
            }

            if (summary != null)
            {
                summary.Text = summaryText;
            }
        }
	}
	
}
