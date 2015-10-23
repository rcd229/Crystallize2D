using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

/**
 * Controller class that provides api to query and modify the conversation table
 * Should be the only class working with conversationTable class
 * API:
 * Query: different queries, according to need (TODO more specific later)
 * Modification: modify by id. Do not support set to null
 */ 
public class ConversationDataController {

	public static UserInfo SuperUser{
		get{
			return new UserInfo("virtual-admin", "", true);
		}
	}

	private static bool VerifyUserRead(UserInfo info){
		//TODO more logic later
		return info.CanEdit;
	}

	private static bool VerifyUserWrite(UserInfo info){
		//TODO more logic later
		return info.CanRead;
	}

	public static bool modify(UserInfo user, ConversationID id, string title = "", string description = "", SequentialConveration dialogue = null){
		if(!VerifyUserWrite(user)){
			return false;
		}

		if(id == null){
			return false;
		}

		var row = ConversationTable.Instance.Table.Get(id);

		if(row == null || row.Log.Count == 0){
			return false;
		}

		var log = row.Log.OrderByDescending(s => Convert.ToDateTime(s.TimeStamp, new CultureInfo("en-US"))).FirstOrDefault().Copy();
		var model = log.Data;
		if(!title.IsEmptyOrNull()){
			model.title = title;
		}
		if(!description.IsEmptyOrNull()){
			model.description = description;
		}
		if(dialogue != null){
			model.dialogue = dialogue;
		}

		log.status = LogStatus.Edit;
		log.TimeStamp = DateTime.Now.ToString();
		log.User = user.UserName;
		row.Log.Add(log);
		ConversationTable.Instance.Table.Set(row);
		ConversationTable.Save();
		return true;
	}

	public static bool create(UserInfo user, string title, string description, SequentialConveration dialogue = null){
		return create (user, new ConversationID(),title, description, dialogue);
	}

	public static bool create(UserInfo user, ConversationID id, string title, string description, SequentialConveration dialogue = null){
		if(!user.CanCreate){
			return false;
		}
		var collect = ConversationTable.Instance.Table.Get(id);
		if(collect != null){
			return false;
		}
		collect = new ConversationLogCollection();
		collect.ID = id;

		var newLog = new ConversationCreationLog();
		newLog.Data = new ConversationModel(id, title, description, dialogue);
		newLog.TimeStamp = DateTime.Now.ToString();
		newLog.status = LogStatus.Creation;
		newLog.User = user.UserName;
		collect.Log.Add(newLog);
		ConversationTable.Instance.Table.Add(collect);
		ConversationTable.Save();
		return true;
	}


	public static ConversationCreationLog read(UserInfo user, ConversationID id){
		var lst = readAll(user, id);
		if(lst == null) return null;
		return lst.FirstOrDefault();
	}

	public static ConversationModel readContent (UserInfo user, ConversationID id){
		ConversationCreationLog log = read (user, id);
		if(log != null){
			return log.Data;
		}else{
			return null;
		}
	}

	//get all logs of modification on this id
	public static List<ConversationCreationLog> readAll(UserInfo user, ConversationID id){
		if(!VerifyUserRead(user)){
			return null;
		}
		var logs = ConversationTable.Instance.Table.Get(id);
		if(logs == null){
			return null;
		}
		var myLog = from l in logs.Log
				where l.Data.cid.Equals(id)
				orderby Convert.ToDateTime(l.TimeStamp, new CultureInfo("en-US")) descending
				select l;
		return myLog.ToList();
	}

	public static List<TaskSelectionInfo> readAllIDs(){
		var ids = from l in ConversationTable.Instance.Table.Items
			select 
				new TaskSelectionInfo(l.ID, 
				                      l.Log.OrderByDescending(
					s => Convert.ToDateTime(s.TimeStamp, new CultureInfo("en-US"))).FirstOrDefault().Data.title
				                      );
		return ids.ToList();
	}
}

public class TaskSelectionInfo{
	public ConversationID ID;
	public string title;
	public TaskSelectionInfo(ConversationID id, string title){
		ID = id;
		this.title = title;
	}
}

public class UserInfo{
	public bool CanEdit{get;private set;}

	public bool CanRead{get; private set;}

	public bool CanCreate{get; private set;}

	public string UserName{get; private set;}

	public string Password{get; private set;}
	//TODO karma
	public UserInfo(string userName, string password, bool isAdmin = false){

		this.UserName = userName;
		this.Password = password;
		if(isAdmin){
			CanCreate = true;
		}
		CanEdit = true;
		CanRead = true;
	}

	public UserInfo(){
		UserName = "";
		Password = "";
	}
}
