using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Data;
using System;
using System.Globalization;

public class ConversationDatabaseController {
	static ConversationDatabaseController _instance;

	public static ConversationDatabaseController Instance{
		get{
			if(_instance == null){
				_instance = new ConversationDatabaseController();
			}
			return _instance;
		}
	}

	string constr = "Server=127.0.0.1;Database=crystallize;User ID=root;Password=123456;Pooling=true";

	// connection object
	MySqlConnection con = null;
	// command object
	MySqlCommand cmd = null;
	// reader object
	MySqlDataReader rdr = null;
	// error object
	MySqlError er = null;

	ConversationDatabaseController(){
		try
		{
			// setup the connection element
			con = new MySqlConnection(constr);
			// lets see if we can open the connection
			con.Open();
			Debug.Log("Connection State: " + con.State);
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.ToString());
		}
	}

	public static UserInfo SuperUser{
		get{
			return new UserInfo("virtual-admin", "crystallize is a good game and I like it", true);
		}
	}
	
	private bool VerifyUserRead(UserInfo info){
		//TODO more logic later
		return info.CanEdit;
	}
	
	private bool VerifyUserWrite(UserInfo info){
		//TODO more logic later
		return info.CanRead;
	}
	
	public bool modify(UserInfo user, ConversationID id, string title = "", string description = "", SequentialConveration dialogue = null){
		if(!VerifyUserWrite(user)){
			return false;
		}
		
		if(id == null){
			return false;
		}

		string query = string.Empty;
		// Error trapping in the simplest form
		try
		{
			query = "INSERT INTO `conversation_table` " +
				"(`cid`, `title`, `description`, `dialogue`, `uid`, `time`, `status`) " +
				"VALUES (?cid, ?title, ?description, ?dialogue, ?uid, ?time, ?status)";
			if (!con.State.Equals(ConnectionState.Open))
				con.Open();
			using (con)
			{
				using (cmd = new MySqlCommand(query, con))
				{
					MySqlParameter oParam = cmd.Parameters.Add("?cid", MySqlDbType.VarChar);
					oParam.Value = id.ToString();
					MySqlParameter oParam1 = cmd.Parameters.Add("?title", MySqlDbType.VarChar);
					oParam1.Value = title;
					MySqlParameter oParam2 = cmd.Parameters.Add("?description", MySqlDbType.Text);
					oParam2.Value = description;
					MySqlParameter oParam3 = cmd.Parameters.Add("?dialogue", MySqlDbType.LongText);
					oParam3.Value = Util.Serialization.Serializer.SaveToXmlString<SequentialConveration>(dialogue);
					MySqlParameter oParam4 = cmd.Parameters.Add("?uid", MySqlDbType.VarChar);
					oParam4.Value = user.UserName;
					MySqlParameter oParam5 = cmd.Parameters.Add("?time", MySqlDbType.DateTime);
					oParam5.Value = DateTime.Now;
					MySqlParameter oParam6 = cmd.Parameters.Add("?status", MySqlDbType.VarChar);
					oParam6.Value = Enum.GetName(typeof(LogStatus),LogStatus.Edit);


					cmd.ExecuteNonQuery();
				}

			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
			return false;
		}

		return true;
	}
	
	public bool create(UserInfo user, string title, string description, SequentialConveration dialogue = null){
		return create (user, new ConversationID(),title, description, dialogue);
	}
	
	public bool create(UserInfo user, ConversationID id, string title, string description, SequentialConveration dialogue = null){
		if(!user.CanCreate){
			return false;
		}
		string query = string.Empty;
		// Error trapping in the simplest form
		try
		{
			query = "INSERT INTO `conversation_table` " +
				"(`cid`, `title`, `description`, `dialogue`, `uid`, `time`, `status`) " +
					"VALUES (?cid, ?title, ?description, ?dialogue, ?uid, ?time, ?status)";
			if (!con.State.Equals(ConnectionState.Open))
				con.Open();
			using (con)
			{
				using (cmd = new MySqlCommand(query, con))
				{
					MySqlParameter oParam = cmd.Parameters.Add("?cid", MySqlDbType.VarChar);
					oParam.Value = id.ToString();
					MySqlParameter oParam1 = cmd.Parameters.Add("?title", MySqlDbType.VarChar);
					oParam1.Value = title;
					MySqlParameter oParam2 = cmd.Parameters.Add("?description", MySqlDbType.Text);
					oParam2.Value = description;
					MySqlParameter oParam3 = cmd.Parameters.Add("?dialogue", MySqlDbType.LongText);
					oParam3.Value = Util.Serialization.Serializer.SaveToXmlString<SequentialConveration>(dialogue);
					MySqlParameter oParam4 = cmd.Parameters.Add("?uid", MySqlDbType.VarChar);
					oParam4.Value = user.UserName;
					MySqlParameter oParam5 = cmd.Parameters.Add("?time", MySqlDbType.DateTime);
					oParam5.Value = DateTime.Now;
					MySqlParameter oParam6 = cmd.Parameters.Add("?status", MySqlDbType.VarChar);
					oParam6.Value = Enum.GetName(typeof(LogStatus),LogStatus.Creation);
					
					
					cmd.ExecuteNonQuery();
				}
				
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
			return false;
		}
		return true;
	}
	
	//read the most recent content on this id
	public ConversationCreationLog read(UserInfo user, ConversationID id){
		if(!VerifyUserRead(user)){
			return null;
		}
		
		string query = string.Empty;
		List<ConversationCreationLog> logs = new List<ConversationCreationLog>();
		// Error trapping in the simplest form
		try
		{
			query = "SELECT A.cid, A.dialogue, A.uid, A.title, A.description, A.status, A.time FROM " +
				"(SELECT * FROM conversation_table WHERE cid = ?ID) AS A " +
				"LEFT JOIN (SELECT * FROM conversation_table WHERE cid = ?ID) AS B " +
				"ON A.time < B.time " +
				"WHERE B.time IS NULL";
			if (!con.State.Equals(ConnectionState.Open))
				con.Open();
			using (con)
			{
				using (cmd = new MySqlCommand(query, con))
				{
					MySqlParameter param = cmd.Parameters.Add("?ID", MySqlDbType.VarChar);
					param.Value = id.ToString();
					rdr = cmd.ExecuteReader();
					if(rdr.HasRows){
						while (rdr.Read()){
							ConversationCreationLog itm = new ConversationCreationLog();
							itm.Data = new ConversationModel();
							itm.status = (LogStatus)Enum.Parse(typeof(LogStatus), rdr["status"].ToString());
							itm.Data.cid = id;
							itm.Data.title = rdr["title"].ToString();
							itm.Data.description = rdr["description"].ToString();
							itm.Data.dialogue = Util.Serialization.Serializer.
								LoadFromXmlString<SequentialConveration>(rdr["dialogue"].ToString());
							itm.TimeStamp = rdr["time"].ToString();
							itm.User = rdr["uid"].ToString();
							
							logs.Add(itm);
						}
						rdr.Dispose();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
			return null;
		}
		
		return logs.FirstOrDefault();
	}
	
	public ConversationModel readContent (UserInfo user, ConversationID id){
		ConversationCreationLog log = read (user, id);
		if(log != null){
			return log.Data;
		}else{
			return null;
		}
	}
	
	//get all logs of modification on this id
	public List<ConversationCreationLog> readAll(UserInfo user, ConversationID id){
		if(!VerifyUserRead(user)){
			return null;
		}

		string query = string.Empty;
		List<ConversationCreationLog> logs = new List<ConversationCreationLog>();
		// Error trapping in the simplest form
		try
		{
			query = "SELECT * FROM conversation_table WHERE cid = ?ID";
			if (!con.State.Equals(ConnectionState.Open))
				con.Open();
			using (con)
			{
				using (cmd = new MySqlCommand(query, con))
				{
					MySqlParameter param = cmd.Parameters.Add("?ID", MySqlDbType.VarChar);
					param.Value = id.ToString();
					rdr = cmd.ExecuteReader();
					if(rdr.HasRows){
						while (rdr.Read()){
							ConversationCreationLog itm = new ConversationCreationLog();
							itm.Data = new ConversationModel();
							itm.status = (LogStatus)Enum.Parse(typeof(LogStatus), rdr["status"].ToString());
							itm.Data.cid = id;
							itm.Data.title = rdr["title"].ToString();
							itm.Data.description = rdr["description"].ToString();
							itm.Data.dialogue = Util.Serialization.Serializer.
								LoadFromXmlString<SequentialConveration>(rdr["dialogue"].ToString());
							itm.TimeStamp = rdr["time"].ToString();
							itm.User = rdr["uid"].ToString();
							
							logs.Add(itm);
						}
						rdr.Dispose();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
			return logs;
		}

		return logs;
	}
	
	public List<TaskSelectionInfo> readAllIDs(){
		string query = string.Empty;
		List<TaskSelectionInfo> logs = new List<TaskSelectionInfo>();
		// Error trapping in the simplest form
		try
		{
			query = "SELECT DISTINCT A.cid, A.title FROM " +
				"conversation_table A " +
				"LEFT JOIN conversation_table B " +
				"ON A.cid = B.cid AND A.time < B.time " +
				"WHERE B.time IS NULL";
			if (!con.State.Equals(ConnectionState.Open))
				con.Open();
			using (con)
			{
				using (cmd = new MySqlCommand(query, con))
				{
					rdr = cmd.ExecuteReader();
					if(rdr.HasRows){
						while (rdr.Read()){
							TaskSelectionInfo itm = new TaskSelectionInfo(new ConversationID(rdr["cid"].ToString()), rdr["title"].ToString());
							logs.Add(itm);
						}
						rdr.Dispose();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
			return logs;
		}
		
		return logs;
	}
}
