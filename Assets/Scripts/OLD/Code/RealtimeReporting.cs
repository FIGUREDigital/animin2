using UnityEngine;
using System.Collections;

public class RealtimeReporting : MonoBehaviour {

		#region Singleton

		private static RealtimeReporting s_Instance;

		public static RealtimeReporting Instance
		{
				get
				{
						if( s_Instance == null )
						{
								s_Instance = new RealtimeReporting();
						}
						return s_Instance;
				}
		}

		#endregion

		private const string SERVER_SEND_URL = "http://terahard.org/Teratest/DatabaseAndScripts/RealtimeDataSend.php";

		public IEnumerator WWWSendData(string dataType, int amountToAdd  )
		{

				WWWForm webForm = new WWWForm();

				webForm.AddField( "DataType", dataType );

				webForm.AddField( "AmountToAdd", amountToAdd );

				WWW w = new WWW( SERVER_SEND_URL, webForm );

				yield return w;

				if( w.error != null )
				{
					Debug.Log( w.error );
				}
				else
				{
					Debug.Log( "Finished uploading reporting data" );
				}

		}
}
