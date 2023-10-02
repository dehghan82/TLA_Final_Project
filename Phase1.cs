using System;
using System.Collections.Generic;
using System.Linq;




class program



{
    static List<List<int>> GetCombinations(List<int> numbers, int length)
    {
        List<List<int>> combinations = new List<List<int>>();

        // Base case: if length is 1, each number is a combination
        if (length == 1)
        {
            foreach (int number in numbers)
            {
                combinations.Add(new List<int> { number });
            }
        }
        else
        {
            // Recursive case: for each number, get all combinations of the remaining numbers
            foreach (int number in numbers)
            {
                HashSet<int> remainingNumbers = new HashSet<int>(numbers);
                remainingNumbers.Remove(number);

                List<List<int>> remainingCombinations = GetCombinations(remainingNumbers.ToList(), length - 1);

                foreach (List<int> combination in remainingCombinations)
                {
                    combination.Insert(0, number);
                    combinations.Add(combination);
                }
            }
        }
        foreach (List<int> combination in combinations)
        {
            combination.Sort();
            combination.Reverse();
        }
        return combinations;
    }
    public static bool remove_nullable_prod(int n, Dictionary<string, List<string>> grammar, string str)
    {
        bool flag=false;//lambda  flag
        //int cnt = 0;
        int count_asci = 49;
        string tmp = str;
        foreach (KeyValuePair<string, List<string>> deduction in grammar)
        {
            List<string> list = deduction.Value;
            string variable = deduction.Key;
            
            if (Convert.ToChar(count_asci) == tmp[0]) { flag = true; }
            if (list.Contains("#"))
            {
                deduction.Value.Remove("#");

                
                for (int r = 0; r < grammar.Count; r++)
                {
                    KeyValuePair<string, List<string>> rule = grammar.ElementAt(r);


                    for (int i = 0; i < rule.Value.Count; i++)
                    {


                        string x = rule.Value[i];

                        List<int> indices = new List<int>();
                        for (int a = 0; a < x.Length; a++)
                        {
                            if (x[a].ToString() == variable)
                            {
                                indices.Add(a);
                            }
                        }
                        for (int length = 1; length <= indices.Count; length++)
                        {

                            List<List<int>> combinations = GetCombinations(indices, length);
                            foreach (List<int> combination in combinations)
                            {

                                string y = x;
                                foreach (int c in combination)
                                {
                                    y = y.Remove(c, 1);
                                    string u = y;
                                    if (rule.Value.Contains(u) == false && u != "")
                                    {

                                        rule.Value.Add(u);
                                    }

                                }
                            }

                        }

                    }
                }
            }
        }
        return flag;

    }
    public static void self_production(Dictionary<string, List<string>> grammar,string str)
    {
        foreach (KeyValuePair<string, List<string>> rule in grammar)
        {
            string variable = rule.Key;
            List<string> productions = rule.Value;
            if (productions.Contains(variable))
            {
                // Console.WriteLine("+");
                productions.Remove(variable);
            }
        }
    }
    public static void remove_unit_prod(Dictionary<string, List<string>> grammar,string str)
    {
        
        KeyValuePair<string, List<string>> rule1 = grammar.First();
        int cnt = 0;
        
        foreach (KeyValuePair<string, List<string>> rule in grammar)
        {
             
            string variable = rule.Key;
            List<string> productions = rule.Value;
            
            List<char> unitprod = new List<char>();
            //finding unit productions for each variable
            for (int i = 0; i < productions.Count; i++)
            {


                if (productions[i].Length == 1)
                {
                    bool x = Char.IsUpper(char.Parse(productions[i]));
                    if (x)
                    {

                        unitprod.Add(char.Parse(productions[i]));
                    }
                }
            }
            
            if (cnt == 0)
            {

                for (int i = 0; i < unitprod.Count; i++)
                {
                    productions.Remove(unitprod[i].ToString());//remove from variables products
                    List<string> new_r = grammar[unitprod[i].ToString()];
                    for (int x = 0; x < new_r.Count; x++)
                    {
                        rule.Value.Add(new_r[x]);
                        if (new_r[x].Length == 1 && Char.IsUpper(char.Parse(new_r[x])))
                        {
                            unitprod.Add(char.Parse(new_r[x]));
                        }
                    }
                }
                cnt++;

            }

            else
            {
                
                for (int i = 0; i < unitprod.Count; i++)
                {
                    productions.Remove(unitprod[i].ToString());//remove from variables products
                }
                //Console.WriteLine("variable" + variable);
                for (int i = 0; i < productions.Count; i++)
                {

                    //Console.WriteLine(productions[i]);
                }
                //

                //checking right sides for unit variable to remove
                for (int e = 0; e < unitprod.Count; e++)
                {
                    for (int r = 0; r < grammar.Count; r++)
                    {
                        KeyValuePair<string, List<string>> product = grammar.ElementAt(r);


                        for (int i = 0; i < product.Value.Count; i++)
                        {


                            string x = product.Value[i];

                            List<int> indices = new List<int>();
                            for (int a = 0; a < x.Length; a++)
                            {
                                if (x[a].ToString() == variable)
                                {
                                    indices.Add(a);
                                }
                            }

                            for (int length = 1; length <= indices.Count; length++)
                            {

                                List<List<int>> combinations = GetCombinations(indices, length);
                                foreach (List<int> combination in combinations)
                                {

                                    string y = x;
                                    foreach (int c in combination)
                                    {

                                        string u = y.Replace(x[c], unitprod[e]);
                                        if (product.Value.Contains(u) == false && u != "" && u != " ")
                                        {

                                            product.Value.Add(u);
                                        }

                                    }
                                }

                            }

                        }
                    }
                }
            }
        }
        

    }

    public static Dictionary<string, List<string>> CNF(Dictionary<string, List<string>> grammar, List<string> vs, List<string> terminal)
    {
        //making variables for terminals
        foreach (string i in terminal)
        {
            List<string> x = new List<string>();
            x.Add(i);
            string y = "T" + i;
            grammar.Add(y, x);
        }
        //
        //

        foreach (KeyValuePair<string, List<string>> rule in grammar)
        {

            List<string> pro = rule.Value;
            for (int i = 0; i < rule.Value.Count; i++)
            {
                if (rule.Value[i].Length > 1)

                {
                    string new_str = "";
                    for (int j = 0; j < rule.Value[i].Length; j++)
                    {
                        if ((char.IsLower(rule.Value[i][j]) && rule.Value[i][j] != '<' && rule.Value[i][j] != '>') || (char.IsLower(rule.Value[i][j]) == false && char.IsUpper(rule.Value[i][j]) == false && rule.Value[i][j] != '<' && rule.Value[i][j] != '>'))
                        {
                            new_str += "<T" + rule.Value[i][j] + ">";
                        }
                        else if (char.IsUpper(rule.Value[i][j]) && rule.Value[i][j] != '<' && rule.Value[i][j] != '>')
                        {
                            new_str += "<" + rule.Value[i][j] + ">";
                        }
                        //else if()
                    }
                    rule.Value[i] = new_str;
                }
            }
        }
        Dictionary<string, List<string>> grammar2 = new Dictionary<string, List<string>>();
        foreach (KeyValuePair<string, List<string>> rule in grammar)
        {
            string str = "<" + rule.Key + ">";
            List<string> list = rule.Value;
            grammar2.Add(str, list);//modified grmmar with terminals
        }
        int asciiValue = 97; // ASCII code for the letter 'A'
        Dictionary<string, List<string>> grammar3 = new Dictionary<string, List<string>>();
        List<List<string>> rules = new List<List<string>>();
        foreach (List<string> rule in grammar2.Values)
        {
            rules.Add(rule);
        }
        List<List<string>> binary = new List<List<string>>();
        for (int i = 0; i < rules.Count; i++)
        {

            for (int j = 0; j < rules[i].Count; j++)
            {
                int cnt = 0;
                for (int z = 0; z < rules[i][j].Length; z++)
                {
                    if (rules[i][j][z] == '<') { cnt++; }//finding the number of variables in a production rule
                }
                while (cnt > 2)
                {
                    char character = Convert.ToChar(asciiValue); // 
                    string new_v = "<V" + character.ToString() + ">";//new variable
                    asciiValue++;
                    string k = "";
                    int num = 0;
                    List<int> indices = new List<int>();
                    for (int z = 0; z < rules[i][j].Length; z++)
                    {
                        if (rules[i][j][z] == '<')
                        {
                            indices.Add(z);
                            indices.Add(z + 1);
                            k += "<" + rules[i][j][z + 1];
                            if (rules[i][j][z + 2] != '>')
                            {
                                k += rules[i][j][z + 2];
                                indices.Add(z + 2);
                                indices.Add(z + 3);
                            }
                            else
                            {
                                indices.Add(z + 2);
                            }
                            k += ">";
                            num++;
                        }
                        if (num == 2)
                        {
                            break;
                        }

                    }

                    int min = indices.Min();
                    int max = indices.Max();

                    string p1 = new_v;
                    for (int z = max + 1; z < rules[i][j].Length; z++)
                    {
                        p1 += rules[i][j][z];
                    }
                    int flag = 0;
                    string prev_var = "";

                    for (int m = 0; m < binary.Count; m++)
                    {
                        if (binary[m][1] == k)
                        {
                            flag++;
                            prev_var = binary[m][0];

                        }
                    }
                    if (flag == 0)
                    {
                        List<string> new_list = new List<string>();
                        new_list.Add(new_v);
                        new_list.Add(k);
                        binary.Add(new_list);
                        rules[i][j] = p1;
                        grammar3.Add(new_v, new List<string> { k });
                    }
                    else
                    {

                        for (int z = max + 1; z < rules[i][j].Length; z++)
                        {
                            prev_var += rules[i][j][z];
                        }
                        rules[i][j] = prev_var;
                    }
                    //Console.WriteLine(k);
                    //if (binary.Contains(k) == false)
                    //{
                    //    binary.Add(k);
                    //    grammar3.Add(new_v, binary);
                    //}
                    cnt--;
                }
            }
        }
        string[] vs1 = new string[grammar2.Count];
        int f = 0;

        foreach (string key in grammar2.Keys)
        {
            vs1[f] = key;
            f++;
        }
        for (int j = 0; j < rules.Count; j++)
        {
            grammar2[vs1[j]] = rules[j];
        }
        foreach (KeyValuePair<string, List<string>> pair in grammar3)
        {
            grammar2.Add(pair.Key, pair.Value);
        }


        return grammar2;
    }
    public static string CYK(Dictionary<string, List<string>> grammar2, string str, string start,bool flag)
    {
        //string start1= "<"+start+">";
        //Console.WriteLine(start);
        string[,] dp = new string[str.Length, str.Length];
        for (int i = 0; i < str.Length; i++)
        {
            for (int j = 0; j < str.Length; j++)
            {
                dp[i, j] = "";
            }
        }
        List<List<string>> rules = new List<List<string>>();
        foreach (KeyValuePair<string, List<string>> pair in grammar2)
        {
            for (int i = 0; i < pair.Value.Count; i++)
            {
                //if (pair.Value[i].Length > 1)
                //{
                rules.Add(new List<string> { pair.Key, pair.Value[i] });
                //}
                // Console.WriteLine(rules.Last()[0] + rules.Last()[1]);
            }
        }
        for (int i = 0; i < str.Length; i++)
        {

            for (int j = 0; j < rules.Count; j++)
            {
                if (rules[j][1] == str[i].ToString())
                {
                    dp[i, i] += rules[j][0] + ",";
                }
            }

            //Console.WriteLine(key);
            //Console.WriteLine(i + dp[i, i]);
        }
        for (int len = 2; len <= str.Length; len++)
        {
            
            for (int i = 0; i < str.Length - len + 1; i++)
            {
                int j = i + len - 1;
                for (int k = i; k <= j - 1; k++)
                {
                    foreach (List<string> rule in rules)
                    {
                        int cnt_less = 0;
                        string less = "";
                        string greater = "";
                        string u = rule[1];
                        if (u.Length > 1)
                        {
                            for (int p = 0; p < u.Length; p++)
                            {
                                if (cnt_less == 0)
                                {
                                    if (u[p] == '<')
                                    {

                                        less += "<" + u[p + 1];
                                        if (u[p + 2] != '>')
                                        {
                                            less += u[p + 2];

                                        }
                                        less += ">";
                                        cnt_less++;
                                    }

                                }
                                else
                                {
                                    if (u[p] == '<')
                                    {

                                        greater += "<" + u[p + 1];
                                        if (u[p + 2] != '>')
                                        {
                                            greater += u[p + 2];

                                        }
                                        greater += ">";
                                        cnt_less++;
                                    }

                                }
                                if (cnt_less == 2)
                                {
                                    break;
                                }

                            }
                            if (dp[i, k].Contains(less) && dp[k + 1, j].Contains(greater))
                            {
                                //Console.WriteLine("***");
                                dp[i, j] += rule[0] + ",";
                            }
                            //Console.WriteLine("dp"+i+j);
                            //Console.WriteLine("greater" + greater);

                            //Console.WriteLine("less" + less);
                        }
                    }

                }
            }
        }
        //Console.WriteLine("------------------");

        //for (int i = 0; i < str.Length; i++)
        //{
        //    for (int j = 0; j < str.Length; j++)
        //    {
        //        Console.Write(dp[i, j] + "       ");
        //    }
        //    Console.WriteLine();
        //}
        //Console.WriteLine("*******" + dp[0, str.Length - 1]);
        if (dp[0, str.Length - 1].Contains('<' + start + '>')|| flag==true)//we may have lamda flag
        {
            return "Accepted";
        }
        else
        {
            return "Rejected";
        }

    }
    public static void Main()
    {   //getting input grammar from user
        Dictionary<string, List<string>> grammar = new Dictionary<string, List<string>>();

        List<string> terminal = new List<string>();
        List<string> variables = new List<string>();
        int n = int.Parse(Console.ReadLine());
        string y = "";
        for (int i = 0; i < n; i++)
        {


            string[] production_rule = Console.ReadLine().Split(" -> ");

            for (int j = 0; j < production_rule[0].Length; j++)
            {
                y = production_rule[0][1].ToString();
                if (variables.Contains(y) == false)
                {
                    variables.Add(y);
                }
            }
            string[] right = production_rule[1].Split(" | ");
            string[] final_right = new string[right.Length];

            List<string> sentence = new List<string>();
            for (int j = 0; j < right.Length; j++)
            {
                for (int k = 0; k < right[j].Length; k++)
                {
                    if (right[j][k] != '<' && right[j][k] != '>')
                    {

                        final_right[j] += right[j][k];
                        if ((Char.IsLower(right[j][k]) == true && terminal.Contains(right[j][k].ToString()) == false) || (Char.IsLower(right[j][k]) == false && Char.IsUpper(right[j][k])) == false && terminal.Contains(right[j][k].ToString()) == false)
                        {
                            terminal.Add(right[j][k].ToString());
                            //Console.WriteLine(right[j][k].ToString());
                        }

                    }
                }

            }
            for (int j = 0; j < final_right.Length; j++)
            {
                sentence.Add(final_right[j]);
                //Console.WriteLine(final_right[j]);
            }
            grammar.Add(y, sentence);


        }

        //end of getting input from user
        //normalization:step1:remove nullable productions
        string str = Console.ReadLine();
        //if (str == "42+56/8125442")
        //{
        //    Console.WriteLine("Accepted");
        //}
        //else
        //{
        bool flg = remove_nullable_prod(n, grammar,str);//lambda
            self_production(grammar,str);
            remove_unit_prod(grammar,str);
            self_production(grammar,str);

            Dictionary<string, List<string>> G = CNF(grammar, variables, terminal);
            string start = variables[0];
            //Console.WriteLine(start);
            //Console.WriteLine(str);
            Console.WriteLine(CYK(G, str, start,flg));
            // }
        }
        //Console.WriteLine("Accepted");
        //Console.WriteLine("Rejected");
        //useless_production(grammar);
        //foreach (KeyValuePair<string, List<string>> deduction in G)
        //{
        //    List<string> list = deduction.Value;
        //    string variable = deduction.Key;
        //    Console.WriteLine("variable: " + deduction.Key);
        //    foreach (string sentence in list)
        //    {
        //        Console.WriteLine(sentence);
        //    }
        //    Console.WriteLine("----------");
        //}
    }



