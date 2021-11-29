using System.Collections;
using System.Collections.Generic;
using AhoCorasick;
using System;

namespace Generator;

public interface ITagGenerator
{
    public void Add(string s);
    public void Add(IEnumerable<string> strings);
    public void Build();
    public IEnumerable<string> Find(string text);
}

public class TagGenerator : ITagGenerator
{
    Trie t;

    public TagGenerator() => t = new Trie();


    public void Add(string s) {
        s = s.ToLower();
        t.Add(s);
    }

    public void Build() { 
        t.Build();
    }

    public void Add(IEnumerable<string> strings)
    {
        foreach (string s in strings)
        {
            Add(s);
        }
    }

    public IEnumerable<string> Find(string text) {
        text = text.ToLower();
        return t.Find(text);
    }
}