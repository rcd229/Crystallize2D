using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util.Serialization;

public class SurveyData {
    public string name;
    public int minutesPlayed = 0;
    public List<string> responses = new List<string>();
    public int answered = 0;
}

public class ReadSurveyData : MonoBehaviour {

    const int UserRow = 5;
    const int TimeRow = 6;
    const int DemographicStartColumn = 8;
    const int DemographicCount = 10;
    const int FirstQuizRow = 18;
    const int QuizColumnCount = 30;
    const int LikeGameColumn = 48;
    const int WordsFirstColumn = 49;
    const int WordsColumnCount = 14;

    // Use this for initialization
    void Start() {
        DataTable preTable;
        DataTable postTable;
        ReadData(out preTable, out postTable);
    }

    public static void ReadData(out DataTable preTable, out DataTable postTable) {
        preTable = new DataTable();
        postTable = new DataTable();

        List<string[]> rows = new List<string[]>();
        List<SurveyData> datas = new List<SurveyData>();
        HashSet<string> users = new HashSet<string>();

        List<string> demographicItems = new List<string>();
        List<string> gameDescriptionWords = new List<string>();

        using (var reader = new StreamReader(Directory.GetParent(ServerData.DirectoryPath).Parent + "/Crystallize_release_feedback.csv")) {
            string line;
            reader.ReadLine();
            var titles = reader.ReadLine().Split('\t');

            for (int i = DemographicStartColumn; i < DemographicStartColumn + DemographicCount; i++) {
                demographicItems.Add(titles[i].Split('-')[1]);
            }

            for (int i = WordsFirstColumn; i < WordsFirstColumn + WordsColumnCount; i++) {
                gameDescriptionWords.Add(titles[i].Split('-')[1]);
            }

            while ((line = reader.ReadLine()) != null) {
                var userData = new SurveyData();
                datas.Add(userData);
                var row = line.Split('\t');

                rows.Add(row);
                users.Add(row[UserRow]);
                userData.name = row[UserRow];

                var isPost = false;
                var time = row[TimeRow];
                if (!time.IsEmptyOrNull() && time != "0") {
                    var minutesPlayed = int.Parse(row[TimeRow]);
                    userData.minutesPlayed = minutesPlayed;
                    if (minutesPlayed > 5) {
                        isPost = true;
                    }
                }

                for (int i = FirstQuizRow; i < FirstQuizRow + QuizColumnCount; i++) {
                    var response = row[i].Replace("?", "")
                        .Replace("-", "")
                        .Replace("don't know", "")
                        .Replace("i don't know", "");
                    userData.responses.Add(response);
                    if (!response.IsEmptyOrNull()) {
                        userData.answered++;
                    }
                }

                var rowData = new DataTableRow();
                column = -1;
                rowData.SetValue("User", NextColumn(), userData.name);
                rowData.SetValue("Play time", NextColumn(), userData.minutesPlayed);
                rowData.SetValue("Answered", NextColumn(), userData.answered);

                for (int i = DemographicStartColumn; i < DemographicStartColumn + DemographicCount; i++) {
                    rowData.SetValue(demographicItems[i - DemographicStartColumn], NextColumn(), row[i]);
                }

                if (isPost) {
                    var preRow = preTable.GetFirstRowWhere("User", userData.name);
                    int preAnswered = 0;
                    int changeInAnswered = 0;
                    if (preRow != null) {
                        preAnswered = (int)preRow.GetValue("Answered");
                        changeInAnswered = userData.answered - (int)preRow.GetValue("Answered");
                    }
                    rowData.SetValue("Pre-answered", NextColumn(), preAnswered);
                    rowData.SetValue("Change in answered", NextColumn(), changeInAnswered);
                    rowData.SetValue("Liked game", NextColumn(), row[LikeGameColumn]);
                    for (int i = WordsFirstColumn; i < WordsFirstColumn + WordsColumnCount; i++) {
                        rowData.SetValue(gameDescriptionWords[i - WordsFirstColumn], NextColumn(), row[i]);
                    }
                    postTable.AddRow(rowData);
                } else {
                    preTable.AddRow(rowData);
                }
            }
        }

        Debug.Log("pre: " + preTable.rows.Count);

        var s = "";
        s += "User pairs\t" + (rows.Count - users.Count).ToString();
        s += "\nPost-play\t" + datas.Where(d => d.minutesPlayed >= 5).Count();
        Serializer.SaveToFile(Directory.GetParent(ServerData.DirectoryPath).Parent + "/survey_out.txt", s);

        s = "";
        foreach (var d in datas) {
            s += "\n" + d.name + "\t" + d.minutesPlayed + "\t" + d.answered;
            foreach (var resp in d.responses) {
                s += "\t" + resp;
            }
        }
        Serializer.SaveToFile(Directory.GetParent(ServerData.DirectoryPath).Parent + "/survey_table.txt", s);

        Serializer.SaveToFile(Directory.GetParent(ServerData.DirectoryPath).Parent + "/survey_user_pre.txt", preTable.GetFileString());
        Serializer.SaveToFile(Directory.GetParent(ServerData.DirectoryPath).Parent + "/survey_user_post.txt", postTable.GetFileString());
    }

    static int column = -1;
    static int NextColumn() {
        column++;
        return column;
    }
}
