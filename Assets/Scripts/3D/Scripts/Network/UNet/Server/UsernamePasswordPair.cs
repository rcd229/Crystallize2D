using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class UsernamePasswordPair
{
    public string Username { get; set; }
    public string Password { get; set; }

	public UsernamePasswordPair() {
        Username = "";
        Password = "";
	}

    public UsernamePasswordPair(string username, string password) {
        Username = username;
        //Password = password;
        byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
        Password = BitConverter.ToString(data);
    }
}
