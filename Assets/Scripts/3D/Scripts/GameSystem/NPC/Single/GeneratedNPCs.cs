using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DialogueBuilder;

[SerializedNPCs]
public class GeneratedNPCs : HasDialogueBase {

    public static readonly GeneratedNPC[] NPCs = new GeneratedNPC[]{
        Get(0, "hello", "hello"),
        Get(0, "hello", "hello", "this is a nice place"),
        Get(0, "good morning", "good morning"),
        Get(0, "good morning", "good morning", "the weather is nice"),
        Get(0, "goodbye", "goodbye"),
        Get(0, "goodbye", "see you tomorrow"),
        Get(0, "what is your hobby?", "I read books"),
        Get(0, "what is your hobby?", "I play sports"),
        Get(0, "what is your hobby?", "I play games"),
        Get(0, "what is your hobby?", "I do flower arranging"),
        Get(0, "What's your name?", "I'm [name]."),
        Get(0, "What's your name?", "I am [name]."),
        Get(0, "Are you a student?", "Yes I'm a student")
    };

    public static readonly GeneratedNPC IntroductionNPC = new GeneratedNPC(
        "Introduction1", new Guid("ee2e90d90e1142baa05ef0e224b5cb09"), 10,
        Line("hello"),
        Branch(
            Prompted("hello", EnglishLine("..."), Confidence(-2), Line("I'm [name]")),
            Prompted("what's your name?", Line("I'm [name]"))
            ),
        Line("What's your name?"),
        Branch(
            Prompted("?", EnglishLine("...")),
            Prompted("I'm [name].", Line("Nice to meet you."), Line("please remember me"))
            ),
        Line("goodbye")
        );

    public static readonly GeneratedNPC ContextQuestionNPC = new GeneratedNPC(
        "ContextResponseNPC", new Guid("5d897181d06e4ef3abc90971ef5c9916"), 0,
        Line("hello"),
        Branch(
            Prompted("What's your name?", Line("I'm [name].")),
            Prompted("Where are you from?", Line("I'm from [place].")),
            Prompted("How old are you?", Line("I'm [age].")),
            Prompted("What's your job?", Line("I am a [occupation].")),
            Prompted("What's your hobby?", Line("I like to do [hobby]."))
        )
        );


    static GeneratedNPC Get(int level, string prompt, params string[] responses) {
        return new GeneratedNPC(level, prompt, responses);
    }

}
