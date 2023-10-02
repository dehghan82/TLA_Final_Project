using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Q2
{
    public static void Main()
    {
        // string[] mm = File.ReadAllLines(@"E:\AD01022\New folder (2)\a.txt");
        List<State> states = Console.ReadLine().Trim('}').Trim('{').Split(',').Select(x => new State(x)).ToList();
        char[] alphabet = Console.ReadLine().Trim('}').Trim('{').Split(',').Select(x => x[0]).ToArray();
        char[] stack_alphabet = Console.ReadLine().Trim('}').Trim('{').Split(',').Select(x => x[0]).ToArray();
        List<State> final = Console.ReadLine().Trim('}').Trim('{').Split(',').Select(a => states.Where(x => x.Name == a).First()).ToList();
        int n = int.Parse( Console.ReadLine());
        for (int i = 0; i < n; i++)
        {
            //(q0,a,#),(a,q0)
            var q = Console.ReadLine().Replace(")","").Replace("(","").Split(',');
            // states.Where(x => x.Name == q[0].Trim('(')).First().Transition.Add(
            //     q[1][0], new Tuple<char, char, State>(q[2][0], q[3][1], states.Where(x => x.Name == q[4].Trim(')')).First())
            // );
            State ss = states.Where(x => x.Name == q[0].Trim('(')).First();
            var m = new Tuple<char, char>(q[1][0], q[2][0]);
            if(!ss.tran.ContainsKey(m))
                ss.tran.Add(m, new List<Tuple<string, State>>());
            ss.tran[m].Add(new Tuple<string, State>(q[3], states.Where(x => x.Name == q[4].Trim(')')).First()));


        }
        PDA pda = new PDA(states, alphabet, stack_alphabet, final);
        Stack<char> stack = new Stack<char>();
        stack.Push('$');
        string s = Console.ReadLine();
        s = Regex.Replace(s, "#", "");
        // pda.Acceptence(s, stack, 0, states[0]);
        // if (pda.check)
        //     System.Console.WriteLine("Accepted");
        // else
        //     System.Console.WriteLine("Rejected");
        if (pda.Accpet(stack, 0, states[0], s))
            System.Console.WriteLine("Accepted");
        else
            System.Console.WriteLine("Rejected");
    }

}
public class PDA
{
    public PDA(List<State> states, char[] alphabet, char[] stack_alphabet, List<State> final_)
    {
        this.states = states;
        this.alphabet = alphabet;
        this.stack_alphabet = stack_alphabet;
        final_states = final_;
        check = false;
    }

    public List<State> states { get; set; }
    public char[] alphabet { get; set; }
    public char[] stack_alphabet { get; set; }
    public List<State> final_states { get; set; }
    public bool check;
    public Stack<char> stack1;
    public Stack<char> stack2;
    public Stack<char> stack3;
    public Stack<char> stack4;

    public void Acceptence(string s, Stack<char> stack, int i, State po)
    {
        if (check)
            return;
        State root = po;
        while (i < s.Count())
        {
            if (check)
                return;
            if (root.Transition.ContainsKey('#'))
            {
                Stack<char> s1 = new Stack<char>(stack.Reverse());
                var e = root.Transition['#'];
                char p = '#';
                if (s1.Peek() == e.Item1)
                {
                    p = s1.Pop();
                    if (e.Item2 != '#')
                        s1.Push(e.Item2);
                    Acceptence(s, s1, i, e.Item3);
                }
                else if (e.Item1 == '#')
                {
                    if (e.Item2 != '#')
                        s1.Push(e.Item2);
                    Acceptence(s, s1, i, e.Item3);
                }
            }
            if (check)
                return;
            if (root.Transition.ContainsKey(s[i]))
            {
                if (check)
                    return;
                var e = root.Transition[s[i]];
                char p = '#';
                if (stack.Peek() == e.Item1)
                {
                    p = stack.Pop();
                    if (e.Item2 != '#')
                        stack.Push(e.Item2);
                    root = e.Item3;
                    i++;
                }
                else if (e.Item1 == '#')
                {
                    if (e.Item2 != '#')
                        stack.Push(e.Item2);
                    i++;
                }
                else
                    return;
            }
            else
                return;
        }
        if (root.Transition.ContainsKey('#'))
        {
            Stack<char> s1 = new Stack<char>(stack.Reverse());
            var e = root.Transition['#'];
            char p = '#';
            if (s1.Peek() == e.Item1)
            {
                p = s1.Pop();
                if (e.Item2 != '#')
                    s1.Push(e.Item2);
                root = e.Item3;
            }
        }
        if (final_states.Contains(root))
            check = true;
    }
    public bool Accpet(Stack<char> stack, int i, State root, string s)
    {
        if (i == s.Count())
            if (this.final_states.Contains(root))
                return true;
        if (stack.Count() == 0)
            return false;
        char entry1 = '#';
        char entry2 = stack.Peek();
        char entry3 = ' ';
        if(i<s.Length)
            entry3 = s[i];
        if (root.tran.ContainsKey(new Tuple<char, char>(entry1, entry1)))
        {
            var stack1 = new Stack<char>(stack.Reverse());
            var version1 = new Tuple<char, char>(entry1, entry1);
            foreach (var item in root.tran[version1])
            {
                if (item.Item1 != "#")
                foreach (var it in item.Item1.Reverse())
                        stack1.Push(it);            
                if (Accpet(stack1, i, item.Item2, s))
                    return true;
            }
        }
        if (root.tran.ContainsKey(new Tuple<char, char>(entry1, entry2)))
        {
            var stack2 = new Stack<char>(stack.Reverse());
            var version2 = new Tuple<char, char>(entry1, entry2);
            stack2.Pop();
            foreach (var item in root.tran[version2])
            {
                if (item.Item1 != "#")
                foreach (var it in item.Item1.Reverse())
                        stack2.Push(it);            
                if (Accpet(stack2, i, item.Item2, s))
                    return true;
            }
        }
        if (root.tran.ContainsKey(new Tuple<char, char>(entry3, entry1)))
        {
            var stack3 = new Stack<char>(stack.Reverse());
            var version3 = new Tuple<char, char>(entry3, entry1);
            foreach (var item in root.tran[version3])
            {
                if (item.Item1 != "#")
                foreach (var it in item.Item1.Reverse())
                        stack3.Push(it);            
                if (Accpet(stack3, i+1, item.Item2, s))
                    return true;
            }
        }
        if (root.tran.ContainsKey(new Tuple<char, char>(entry3, entry2)))
        {
            var stack4 = new Stack<char>(stack.Reverse());
            var version4 = new Tuple<char, char>(entry3, entry2);
                stack4.Pop();
            foreach (var item in root.tran[version4])
            {
                if (item.Item1 != "#")
                foreach (var it in item.Item1.Reverse())
                        stack4.Push(it);            
                if (Accpet(stack4, i+1, item.Item2, s))
                    return true;
            }
        }
        return false;
    }
}
public class State
{
    public State(string name)
    {
        Name = name;
        Transition = new Dictionary<char, Tuple<char, char, State>>();
        tran = new Dictionary<Tuple<char, char>, List<Tuple<string, State>>>();
    }
    public Dictionary<char, Tuple<char, char, State>> Transition { get; set; }
    public Dictionary<Tuple<char, char>, List<Tuple<string, State>>> tran { get; set; }

    public string Name { get; set; }

}
