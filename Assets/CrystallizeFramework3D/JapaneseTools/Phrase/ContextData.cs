using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ContextData {

    public List<ContextDataElement> Elements { get; set; }

    public ContextData() {
        Elements = new List<ContextDataElement>();
    }

    public ContextData(ContextData original)
        : this() {
        foreach (var e in original.Elements) {
            Set(e.Name, e.Data);
        }
    }

    public ContextDataElement Get(string id) {
        return (from e in Elements where e.Name.ToLower() == id.ToLower() select e).FirstOrDefault();
    }

    public void Set(string id, PhraseSequence value) {
        var e = Get(id);
        if (e != null) {
            e.Data = value;
        } else {
            e = new ContextDataElement(id);
            e.Data = value;
            Elements.Add(e);
        }
    }

    public ContextData OverrideWith(ContextData context) {
        var newContext = new ContextData(this);
        if (context == null) {
            return newContext;
        }

        foreach (var k in context.Elements) {
            newContext.Set(k.Name, k.Data);
        }
        return newContext;
    }

}
