using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ConfigurationManager {

	#region DontTouch!
	private Dictionary<string,int> intPreferences = new Dictionary<string, int>(); 
	private static ConfigurationManager instance;
	private ConfigurationManager(){}
	public static ConfigurationManager Instance{
		get{
			return (instance !=null) ? instance : instance = new ConfigurationManager();
		}
	}

	private void SetPreferences(string key, int value){
		if(intPreferences.ContainsKey(key)){
			intPreferences[key] = value;
		}else{
			intPreferences.Add(key,value);
		}
		PlayerPrefs.SetInt(key,value);
	}

	private int GetPreference(string key){
		if(intPreferences.ContainsKey(key)){
			return intPreferences[key];
		}
		int value = PlayerPrefs.GetInt(key,0);
		intPreferences.Add(key,value);
		return value;
	
	}

	#endregion

	public bool GetInstructionsSeen(){
		return 1 == GetPreference("instructions");
	}

	public void SetInstructionToSeen(){
		SetPreferences("instructions",1);
	}
}
