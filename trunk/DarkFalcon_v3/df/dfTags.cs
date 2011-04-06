using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon_v3.df
{
    struct dfTags
    {
        List<string> info;
        List<string> compat;
        List<string> aval;

        public dfTags(string t)
        {
            info = new List<string>();
            compat = new List<string>();
            aval = new List<string>();
            t = t.ToLower();
            List<string> r = t.Split(' ').ToList();

            foreach (string s in r)
            {
                string f;
                f = s.Replace('-',' ');

                if (f.StartsWith("$"))
                {
                    compat.Add(f);
                }
                else if (f.StartsWith("@"))
                {
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
