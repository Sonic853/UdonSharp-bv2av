
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AV2BV : UdonSharpBehaviour
{
    public string av;
    public string bv;
    private string[] table = { "f", "Z", "o", "d", "R", "9", "X", "Q", "D", "S", "U", "m", "2", "1", "y", "C", "k", "r", "6", "z", "B", "q", "i", "v", "e", "Y", "a", "h", "8", "b", "t", "4", "x", "s", "W", "p", "H", "n", "J", "E", "7", "j", "L", "5", "V", "G", "3", "g", "u", "M", "T", "K", "N", "P", "A", "w", "c", "F" };
    private long xor = 177451812;
    private long add = 8728348608;
    private int a58 = 58;
    private int[] s = { 11, 10, 3, 8, 4, 6 };
    private string BVTemplate = "BV1  4 1 7  ";
    void Start()
    {
        Debug.Log("av10492=" + av2bv("AV10492"));
        Debug.Log("BV17R4y157V8=" + bv2av("BV17R4y157V8"));
    }
    string av2bv(string AV)
    {
        av = AV.ToLower();
        long avlong = 0;
        if (av.Substring(0, 2) == "av")
        {
            long.TryParse(av.Substring(2), out avlong);
        }
        else
        {
            long.TryParse(av, out avlong);
        }
        if (avlong == 0) return "error";
        avlong = (avlong ^ xor) + add;
        string BV = BVTemplate;
        for (int i = 0; i < s.Length; i++)
        {
            BV = BV.Remove(s[i], 1);
            double val = avlong / Math.Pow(a58, i) % a58;
            BV = BV.Insert(s[i], table[Mathf.FloorToInt(float.Parse(val.ToString()))]);
        }
        bv = BV;
        return bv;
    }
    string bv2av(string BV)
    {
        bv = BV;
        if (bv.Substring(0, 3) != "BV1") return "error";
        double avdouble = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int i1 = tableIndexOf(bv.Substring(s[i], 1));
            if (i1 == -1) return "error";
            avdouble += i1 * Math.Pow(a58, i);
        }
        avdouble = avdouble - add;
        long avlong = 0;
        long.TryParse(avdouble.ToString(), out avlong);
        av = "av" + (avlong ^ xor).ToString();
        return av;
    }
    int tableIndexOf(string s)
    {
        if (s.Length == 0 || s.Length > 1) return -1;
        for (int i = 0; i < table.Length; i++)
        {
            if (table[i] == s) return i;
        }
        return -1;
    }
}
