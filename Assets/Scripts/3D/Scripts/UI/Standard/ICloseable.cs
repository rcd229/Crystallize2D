using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public static class ClosableExtensions {

    public static bool IsOpen(this ICloseable closable) {
        if (closable != null) {
            if (closable is Component) {
                return ((Component)closable).gameObject;
            }
        }
        return true;
    }

    public static void CloseIfNotNull(this ICloseable closable) {
        if (closable != null) {
            if (closable is Component) {
                if ((Component)closable) {
                    closable.Close();
                }
            } else {
                closable.Close();
            }
        }
    }
}

public interface ICloseable {
    void Close();
}
