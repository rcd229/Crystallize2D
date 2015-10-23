using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CrystallizeData {
    public class StaticSerializedPhraseResources : StaticSerializedGameData {
        protected override void AddGameData() {
            
        }

        protected override void PrepareGameData() {
            int count = 0;
            int setCount = 0;
            var types = from t in Assembly.GetAssembly(typeof(IPhraseResources)).GetTypes()
                        where typeof(IPhraseResources).IsAssignableFrom(t) && t.IsClass
                        select t;
            foreach (var t in types) {
                var constructor = t.GetConstructor(Type.EmptyTypes);
                if (constructor == null) {
                    Debug.LogError(t + " does not have a default constructor");
                    continue;
                }

                var instance = Activator.CreateInstance(t) as IPhraseResources;
                var key = instance.SetKey;
                setCount++;
                foreach (var p in instance.GetPhraseKeys()) {
                    PhrasePipeline.GetPhrase(key, p, false);
                    count++;
                }

                instance.GetType().GetFieldAndPropertyValues<PhraseSequence>(instance);
            }
            Debug.Log("Found " + setCount + " serialized phrases sets. Phrase count: " + count);
        }
    }
}
