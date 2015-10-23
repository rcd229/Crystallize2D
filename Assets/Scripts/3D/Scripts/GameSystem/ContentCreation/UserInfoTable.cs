using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util.Serialization;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Data;
using System;
using System.Security.Cryptography;
using System.Text;

public class UserInfoTable {
	
	static UserInfoTable _instance;
	public static UserInfoTable Instance{
		get{
			if(_instance == null){
				Load ();
			}
			return _instance;
		}
	}

	UserInfoTable (){
		try
		{
			// setup the connection element
			con = new MySqlConnection(constr);
			// lets see if we can open the connection
			con.Open();
			Debug.Log("Connection State: " + con.State);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
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
	
//	const string FileName = "CrystallizeUserTable";
//	const string FileExtension = ".txt";
//	const string EditorFilePath = "/crystallize/Resources/";
//	
//	static string GetPath(){
//		return Application.dataPath + EditorFilePath + FileName + FileExtension;
//	}
	
//	public List<UserInfo> Table{get; private set;}
	
	//unnecessary if relational table is used.
//	public static void Save(){
//		Serializer.SaveToXml<UserInfoTable>(GetPath(), _instance);
//	}
	public static void Load(){
//		_instance = Serializer.LoadFromXml<UserInfoTable>(GetPath(), false);
//		if (_instance == null) {
//			Debug.Log("empty xml");
//			_instance = new UserInfoTable();
//			_instance.Table = new List<UserInfo>();
//		}
		_instance = new UserInfoTable();
	}

	public UserInfo Get(string name, string password){
		string query = string.Empty;
		UserInfo info = new UserInfo();
		try{
			query = "SELECT * FROM `user_info` WHERE `id` = ?ID AND `password` = ?PASS";
			if (!con.State.Equals(ConnectionState.Open))
				con.Open();
			using (con){
				using (cmd = new MySqlCommand(query, con)){
					MySqlParameter param = cmd.Parameters.Add("?ID", MySqlDbType.VarChar);
					param.Value = name;
					MySqlParameter param1 = cmd.Parameters.Add("?PASS", MySqlDbType.VarChar);
					param1.Value = HashPassword(password);
					rdr = cmd.ExecuteReader();
					if(rdr.HasRows){
						rdr.Read();
						bool canCreate = bool.Parse(rdr["create"].ToString());
						info = new UserInfo(rdr["id"].ToString(), rdr["password"].ToString(), canCreate);
					}
				}
			}
		}
		catch(Exception e){
			Debug.Log (e.ToString());
			return null;
		}
		return info;
	}

	/**
	 * return true and create user if the user is unique
	 * Otherwise return false
	 */
	public bool Create(string name, string password, bool isAdmin = false){
		string query = string.Empty;
		try
		{
			query = "SELECT `id` FROM `user_info` WHERE `id` = ?ID";
			if (!con.State.Equals(ConnectionState.Open))
				con.Open();
			using (con)
			{
				using (cmd = new MySqlCommand(query, con))
				{
					MySqlParameter param = cmd.Parameters.Add("?ID", MySqlDbType.VarChar);
					param.Value = name;
					rdr = cmd.ExecuteReader();
					if(rdr.HasRows){
						rdr.Dispose();
						return false;
					}
					else{
						rdr.Dispose();
					}
				}
				string insertQuery = "INSERT INTO `user_info` (`id`, `edit`, `read`, `create`, `password`) VALUES (?id, ?edit, ?read, ?create, ?password)";
				using (cmd = new MySqlCommand(insertQuery, con)){
					UserInfo info = new UserInfo(name, password, isAdmin);
					MySqlParameter param = cmd.Parameters.Add("?id", MySqlDbType.VarChar);
					param.Value = info.UserName;
					MySqlParameter param1 = cmd.Parameters.Add("?edit", MySqlDbType.VarChar);
					param1.Value = info.CanEdit ? "1" : "0";
					MySqlParameter param2 = cmd.Parameters.Add("?read", MySqlDbType.VarChar);
					param2.Value = info.CanRead ? "1" : "0";
					MySqlParameter param3 = cmd.Parameters.Add("?create", MySqlDbType.VarChar);
					param3.Value = info.CanCreate ? "1" : "0";
					MySqlParameter param4 = cmd.Parameters.Add("?password", MySqlDbType.VarChar);
					param4.Value = HashPassword(password);
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
//		if(Table.Exists(s => s.UserName == name)){
//			return false;
//		}
//		else{
//			Table.Add(new UserInfo(name, password));
//			return true;
//		}
	}

	string HashPassword(string password){
		MD5 cipher = MD5.Create();
		byte[] b = Encoding.ASCII.GetBytes(password);
		byte[] c = cipher.ComputeHash(b);
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < c.Length; i++)
		{
			sb.Append(c[i].ToString("X2"));
		}
		Debug.Log(sb.ToString() + " " + password);
		return sb.ToString();
	}
}
