using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
public class Country
{
    private static List<string> _Countrys;
    static Country()
    {
        _Countrys = new List<string>(200);
        _Countrys = Resources.Load<TextAsset>("Data/Contrys")
            .text.Split('\n').
            OfType<string>()
            .Select((str) => str.Remove(str.Length - 1))
            .ToList();
    }

    public static string getCountry(string filter = "")
    {
        while(true)
        {   
            string tmp = _Countrys[Random.Range(0, _Countrys.Count)];
            if (!tmp.Equals(filter))
                return tmp;
        }
    }
}
