using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class CustomConversationPostCompiler : EditorPostCompiler {
	#region implemented abstract members of EditorPostCompiler
	public override void AfterCompile ()
	{
		var quests = from p in Assembly.GetAssembly(typeof(EditorPostCompiler)).GetTypes()
			where p.HasAttribute<UserConversationAttribute>()
				select p;
//		typeof(CustomConversation).GetStaticFieldAndPropertyValues
		//TODO use this ^

		List<CustomConversation> conversations = new List<CustomConversation>();
		foreach (var quest in quests){
			var conv = quest.GetFieldAndPropertyValues<CustomConversation>(Activator.CreateInstance(quest));
			conversations.AddRange(conv);
		}



		foreach(var c in conversations){
			var user = ConversationDatabaseController.SuperUser;
			var log = ConversationDatabaseController.Instance.read(user, c.cid);
			if(log == null){
				ConversationDatabaseController.Instance.create(user, c.cid, c.title, c.description);
			}
			if(!log.Data.description.Equals(c.description) || !log.Data.title.Equals(c.title)){
				ConversationDatabaseController.Instance.modify(user, c.cid, c.title, c.description, log.Data.dialogue);
			}
		}

		Debug.Log(string.Format("{0} custom conversation found.", conversations.Count));
		
		

	}
	#endregion
		
}
		