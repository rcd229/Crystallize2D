using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataTableRow {
    public Dictionary<string, int> columnOrder = new Dictionary<string, int>();
    public Dictionary<string, object> values = new Dictionary<string, object>();
    public string username = "";

    public object GetValue(string key) {
        if (values.ContainsKey(key)) { return values[key]; }
        return null;
    }

    public void SetValue(string key, int order, object value) {
        columnOrder[key] = order;
        values[key] = value;
    }

    public string GetUserName() {
        return username;
    }

    public void SetUserName(string name) {
        username = name;
    }

    public void ChangeValue(string key, object value) {
        values[key] = value;
    }

    public IEnumerable<object> GetOrderedValues() {
        var titles = from c in columnOrder
                     orderby c.Value, c.Key
                     select c.Key;
        return from t in titles
               select values[t];
    }
}

public class DataTable {

    public List<DataTableRow> rows = new List<DataTableRow>();

    public DataTableRow GetFirstRowWhere<T>(string column, T value) {
        return (from r in rows
                where r.values.ContainsKey(column) && value.Equals((T)r.values[column])
                select r).FirstOrDefault();
    }

    public void AddRow(DataTableRow row) {
        rows.Add(row);
    }

    public void ChangeValue(string name, string key, object value) {
        foreach (var r in rows) {
            if (r.GetUserName() == name) {
                r.ChangeValue(key, value);
            }
        }
    }

    public object GetValue(string name, string key)
    {
        foreach (var r in rows)
        {
            if (r.GetUserName() == name)
            {
                r.GetValue(key);
            }
        }
        return null;
    }

    public string GetFileString() {
        var titles = GetTitles();
        var output = new StringBuilder();
        output.Append(titles.Aggregate((f, s) => f + "\t" + s));
        foreach (var row in rows) {
            output.Append("\n");
            for (int i = 0; i < titles.Count; i++) {
                output.Append(row.GetValue(titles[i]));
                output.Append("\t");
            }
        }
        return output.ToString();
    }

    List<string> GetTitles() {
        var set = new HashSet<KeyValuePair<string, int>>(from r in rows
                                                         from c in r.columnOrder
                                                         select new KeyValuePair<string, int>(c.Key, c.Value));
        return (from kv in set
                orderby kv.Value, kv.Key
                select kv.Key)
                .Distinct()
                .ToList();
    }

}
