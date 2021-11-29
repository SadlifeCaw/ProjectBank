using Xunit;
using System.Linq;

namespace Generator.Tests;

public class TagGeneratorTests
{

    [Fact]
    public void Basic_Input_Hello_World_Recive_Hello_World_From_Text()
    {
        string text = "hello and welcome to this beautiful world!";

        ITagGenerator trie = new TagGenerator();
        trie.Add("hello");
        trie.Add("world");
        trie.Build();

        string[] matches = trie.Find(text).ToArray();

        Assert.Equal(2, matches.Length);
        Assert.Equal("hello", matches[0]);
        Assert.Equal("world", matches[1]);
    }

    [Fact]
    public void Find_Add_In_Capital_Letters_Find_Input_With_Lowercase_Letters()
    {
        string text = "I always types in caps";

        ITagGenerator trie = new TagGenerator();
        trie.Add("CAPS");
        trie.Build();

        string[] matches = trie.Find(text).ToArray();

        Assert.Equal("caps", matches[0]);
    }

    [Fact]
    public void Find_Add_In_LowerCase_Find_With_Capital_Letters()
    {
        string text = "I always types in CAPS";

        ITagGenerator trie = new TagGenerator();
        trie.Add("caps");
        trie.Build();

        string[] matches = trie.Find(text).ToArray();

        Assert.Equal("caps", matches[0]);
    }

    [Fact]
    public void Add_An_Array()
    {
        string text = "one two three four";

        ITagGenerator trie = new TagGenerator();
        trie.Add(new[] {"three", "four"});
        trie.Build();

        string[] matches = trie.Find(text).ToArray();

        Assert.Equal("three", matches[0]);
        Assert.Equal("four", matches[1]);
    }

}