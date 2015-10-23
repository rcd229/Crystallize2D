using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class PersonFinder01 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.TalentSeeker1, "Talent seeker 1", 0, 40);

			job.formalLevel = JobFormalLevel.Formal;
            foreach (var s in FindPersonProcess.AllStrings()) {
                AddLine(s);
            }
            AddLine(FindPersonProcess.OkayLetsGo);

            AddTask<FindPersonTask, FindIntroDialogue01>("StreetSession", "FinderClient");
            
            job.TaskSelector = new MasterToContinueTaskSelectorGameData();
            job.AddRequirement(GetPhrase(FindPersonProcess.Person));
            job.Money = 1000;
            job.HelpText = "You need to learn the required words first. (Try doing Tourist 1)";

            job.PromotionTask = job.Tasks[0];
            job.PromotionReq = 0;
            job.PromotionMistakes = 1;
        }
    }

    public class PersonFinder02 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.TalentSeeker2, "Talent seeker 2", 1, 40);

			job.formalLevel = JobFormalLevel.Formal;

            foreach (var s in FindPersonProcess.AllStrings()) {
                AddLine(s);
            }
            AddLine(FindPersonProcess.OkayLetsGo);

            AddTask<FindPersonTask, FindIntroDialogue01>("StreetSession", "FinderClient");

            job.TaskSelector = new MasterToContinueTaskSelectorGameData();
            job.AddRequirement(JobID.TalentSeeker1);
            job.AddRequirement(GetPhrase("boy"));
            job.AddRequirement(GetPhrase("girl"));
            job.Money = 1500;
            job.HelpText = "You need to learn the required words first. (Try doing Talent Seeker 1)";
        }
    }

    public class PersonFinder03 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.TalentSeeker3, "Talent seeker 3", 2, 40);
			job.formalLevel = JobFormalLevel.Formal;

            foreach (var s in FindPersonProcess.AllStrings()) {
                AddLine(s);
            }
            AddLine(FindPersonProcess.OkayLetsGo);

            AddTask<FindPersonTask, FindIntroDialogue01>("StreetSession", "FinderClient");

            job.TaskSelector = new MasterToContinueTaskSelectorGameData();
            job.AddRequirement(JobID.TalentSeeker2);
            job.AddRequirement(GetPhrase("hair"));
            job.AddRequirement(GetPhrase("blond"));
            job.AddRequirement(GetPhrase("brown"));
            job.AddRequirement(GetPhrase("black"));
            job.Money = 2000;
            job.HelpText = "You need to learn the required words first. (Try doing Talent Seeker 2)";
        }
    }

    class FindIntroDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddLine(FindPersonProcess.PleaseHelpMe, 0);
        }
    }

}
