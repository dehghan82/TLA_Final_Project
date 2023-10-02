using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Project2
{
    public class PHASE3
    {
        public static void Main(string[] args)
        {
            PDA p = new PDA();
            System.Console.WriteLine(p.Tocfg());
        }
    }
    public class PDA
    {
        private static string lamb(string inp, bool empty = false)
        {
            if (empty)
            {
                return (inp == "#") ? "" : inp;
            }
            else
            {
                return (inp == "#") ? "#" : inp;
            }
        }
        public string[] alphabets_element;
        public string[] stack_element;
        public List<Transition> transitions;
        public List<string> states;
        public List<string> final_states;
        public string initial_state;

        public PDA()
        {
            this.states = Console.ReadLine().Trim('{').Trim('}').Split(',').ToList();
            this.initial_state = states[0];
            this.alphabets_element = Console.ReadLine().Trim('{').Trim('}').Split(',').ToArray();
            this.stack_element = Console.ReadLine().Trim('{').Trim('}').Split(',').ToArray();
            this.final_states = Console.ReadLine().Trim('{').Trim('}').Split(',').ToList();
            int tcount = int.Parse(Console.ReadLine());
            this.transitions = new List<Transition>();
            //Modify the NPDA so that it empties the stack and has a unique final state
            if (final_states.Count() != 0)
            {
                states.Add($"q{states.Count()}");
                foreach (var item in final_states)
                    transitions.Add(new Transition(item, states.Last(), "#", "#", "#"));
                foreach (var item in stack_element)
                {
                    transitions.Add(new Transition(states.Last(), states.Last(), "#", item, "#"));
                }
                final_states.Clear();
                states.Add("qf");
                final_states.Add(states.Last());
                transitions.Add(new Transition(states[states.Count() - 2], states.Last(), "#", "$", "#"));
            }
            for (int i = 0; i < tcount; i++)
            {
                var tran = Console.ReadLine().Replace(")", "").Replace("(", "").Split(',');
                //modify so write stack be # or 2 length string
                if (tran[3].Length == 1 && tran[3] != "#")
                {
                    states.Add($"q{states.Count()}");
                    transitions.Add(new Transition(tran[0], states.Last(), tran[1], tran[2], "~" + tran[3]));
                    transitions.Add(new Transition(states.Last(), tran[4], "#", "~", "#"));
                }
                string source = tran[0];
                string des = tran[4];
                string sta = tran[3];
                if (tran[3].Length > 2)
                {
                    while (sta.Length != 2)
                    {
                        states.Add($"q{states.Count()}");
                        transitions.Add(new Transition(states.Last(), des, "#", "~", sta[0].ToString() + "~"));
                        des = states.Last();
                        sta = sta.Substring(1);
                    }
                    transitions.Add(new Transition(source,des,tran[1],tran[2],sta));
                }
                if (tran[3].Length == 2 || tran[3] == "#")
                { transitions.Add(new Transition(tran[0], tran[4], tran[1], tran[2], tran[3])); }
            }

        }
        //return its --NOT-- simplified context free grammer
        public string Tocfg()
        {
            List<string> res = new List<string>();
            foreach (var item in this.transitions)
            {
                if (item.stack_write == "" || item.stack_write == "#")
                    res.Add(String.Format("({0}{1}{2}) -> {3}", item.source, item.stack_read, item.destination, item.input));
                else if (item.stack_write.Length == 2)
                {
                    foreach (var s1 in this.states)
                    {
                        foreach (var s2 in this.states)
                        {
                            res.Add(String.Format("({0}{1}{2}) -> {3}({4}{5}{6})({7}{8}{9})", item.source, item.stack_read, s1, PDA.lamb(item.input, true),
                            this.initial_state, item.stack_write[0], s2, s2, item.stack_write[1], s1));
                        }
                    }
                }
                else
                    throw new Exception("transistion not right");
            }
            return string.Join("\n",res);
        }
    }
    
    public class Transition
    {
        public string source;
        public string destination;
        public string input;
        public string stack_read;
        public string stack_write;

        public Transition(string source, string destination, string input, string stack_read, string stack_write)
        {
            this.source = source;
            this.destination = destination;
            this.input = input;
            this.stack_read = stack_read;
            this.stack_write = stack_write;
        }
    }

}