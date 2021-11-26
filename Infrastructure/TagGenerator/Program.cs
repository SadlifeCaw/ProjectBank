using Generator;

class Program {
    public static void Main(string[] args)
    {

        ITagGenerator trie = new TagGenerator();

        trie.Add("kom");
        trie.Add("ko");
        trie.Add("læge");

        trie.Build();
      
        String s = "Hej Jeg er blevet sparket af en ko, og ko jeg Ko KO har brug for en læge";

        s = s.ToLower(); 
        
        foreach (string word in trie.Find(s))
        {
            Console.WriteLine(word);
        }
    }
}