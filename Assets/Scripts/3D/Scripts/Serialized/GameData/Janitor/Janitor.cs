using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class OpenJanitor : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.OpenJanitor, "Janitor", 0, 10);
            AddTask<OpenJanitorTask>();
            job.formalLevel = JobFormalLevel.Versatile;
            job.QuestID = new JanitorUnlockQuest().ID;
            AddLine(JanitorProcess.CleanArea);
            AddLine(JanitorProcess.NiceJob);
            AddLine(JanitorProcess.OkayJob);
            AddLine(JanitorProcess.WorkHarder);
            AddLine(JanitorProcess.BadJob);

            foreach (var p in JanitorProcess.GetPropsForDifficulty(job.Difficulty)) {
                AddLine(p.Label);
            }

            //job.AddRequirement("Tourist 1");
            job.AddRequirement(GetPhrase("place"));
            job.Money = 1000;
            job.HelpText = "You need to learn the required words first. (Try doing Tourist 1)";

            job.PromotionMistakes = 1;
            job.PromotionReq = 10000;
            job.PromotionTask = job.Tasks[0];

            GameDataInitializer.AddOpenJob(Name, job);
        }
    }

    public class Janitor01 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.Janitor1, "Janitor 1", 0, 10);
            AddTask<JanitorTask01>();
			job.formalLevel = JobFormalLevel.Versatile;
            AddLine(JanitorProcess.CleanArea);
            AddLine(JanitorProcess.NiceJob);
            AddLine(JanitorProcess.OkayJob);
            AddLine(JanitorProcess.WorkHarder);
            AddLine(JanitorProcess.BadJob);

            foreach (var p in JanitorProcess.GetPropsForDifficulty(job.Difficulty)) {
                AddLine(p.Label);
            }

            //job.AddRequirement("Tourist 1");
            job.AddRequirement(GetPhrase("place"));
            job.Money = 1000;
            job.HelpText = "You need to learn the required words first. (Try doing Tourist 1)";

            job.PromotionMistakes = 1;
            job.PromotionReq = 0;
            job.PromotionTask = job.Tasks[0];
        }
    }

    public class Janitor02 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.Janitor2, "Janitor 2", 1, 10);
            AddTask<JanitorTask02>();
			job.formalLevel = JobFormalLevel.Versatile;
            AddLine(JanitorProcess.CleanArea);
            AddLine(JanitorProcess.NiceJob);
            AddLine(JanitorProcess.OkayJob);
            AddLine(JanitorProcess.WorkHarder);
            AddLine(JanitorProcess.BadJob);

            foreach (var p in JanitorProcess.GetPropsForDifficulty(job.Difficulty)) {
                AddLine(p.Label);
            }

            job.Money = 1500;
            job.AddRequirement(JobID.Janitor1);
            job.AddRequirement(GetPhrase("plant"));
        }
    }

    public class OpenJanitorTask : StaticSerializedTaskGameData<JobTaskGameData> {
        protected override void PrepareGameData() {
            Initialize("Mop the floor", "SchoolGym", "Boss");
            task.AreaName = "JanitorArea";
            SetProcess<OpenJanitorProcess>();
            SetDialogue<JanitorDialogue01>();
        }
    }

    public class JanitorTask01 : StaticSerializedTaskGameData<JobTaskGameData> {
        protected override void PrepareGameData() {
            Initialize("Mop the floor 1", "SchoolGym", "Boss");
            SetProcess<JanitorProcess>();
            SetDialogue<JanitorDialogue01>();
        }
    }

    public class JanitorTask02 : StaticSerializedTaskGameData<JobTaskGameData> {
        protected override void PrepareGameData() {
            Initialize("Mop the floor 2", "SchoolGym", "Boss");
            SetProcess<JanitorProcess>();
            SetDialogue<JanitorDialogue01>();
        }
    }
}
