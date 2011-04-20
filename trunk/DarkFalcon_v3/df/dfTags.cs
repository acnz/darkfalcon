using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
   public class dfTags
    {
       public List<string> info { get; set; }
        public List<string> compat { get; set; }
        public List<string> aval{get;set;}
        public List<string> qtd { get; set; }
        public dfTags(string t)
        {
            info = new List<string>();
            compat = new List<string>();
            aval = new List<string>();
            qtd = new List<string>();
            t = t.ToLower();
            List<string> r = t.Split(' ').ToList();

            foreach (string s in r)
            {
                string f;
                f = s.Replace('-',' ');

                if (f.StartsWith("$"))
                {
                    f = f.Replace("$","");
                    compat.Add(f);
                }
                if (f.StartsWith("#"))
                {
                    f = f.Replace("#", "");
                    qtd.Add(f);
                }
                else if (f.StartsWith("@"))
                {
                    f = f.Replace("@", "");
                    aval.Add(f);
                }
                else
                {
                    info.Add(f);
                }

            }
        }
    }
}
