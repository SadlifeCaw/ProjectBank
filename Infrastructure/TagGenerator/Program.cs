using Generator;

class Program {
    public static void Main(string[] args)
    {

        ITagGenerator trie = new TagGenerator();

        trie.Add("KOM");

        trie.Build();
      
        String s = "Hej kom med mig";
        
        foreach (string word in trie.Find(s))
        {
            Console.WriteLine(word);
        }
    }
}