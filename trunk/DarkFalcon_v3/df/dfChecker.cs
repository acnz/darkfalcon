using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    class dfChecker
    {
        dfChecker() { }
        
        public string vC(dfCom a,dfCom b){
            string result = "";
            bool isC = vC(a, b,true);
                if (!isC)
                {
                    switch (a.Tipo)
                    {
                        case "Motherboard":
                            switch (b.Tipo)
                            {
                                case "Processador":
                                    break;
                            }
                            break;

                    }
                }
                else
                {
                    result = "Nenhum erro de compatibilidade";
                }
                return result;
        }
        public bool vC(dfCom a, dfCom b,bool retBoolean)
        {
            if (retBoolean)
            {
                bool isC = false;
                foreach (string atag in a.Tags.aval)
                    foreach (string btag in b.Tags.aval)
                    {

                    }
                return isC;
            }
            else
                return false;
        }
    }
}
