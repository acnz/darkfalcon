using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    class dfChecker
    {
        dfChecker() { }
        
        public string v(dfCom a,dfCom b){
            string result = "";
            bool isC = v(a, b,true);
                if (!isC)
                {
                    switch (a.Tipo)
                    {
                        case "Motherboard":
                            switch (b.Tipo)
                            {
                                case "Processador":
                                    break;
                                case "Gabinete":
                                    break;
                                case "Fonte":
                                    break;
                                case "Teclado":
                                    break;
                                case "Mouse":
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
        public bool v(dfCom a, dfCom b,bool retBoolean)
        {
            if (retBoolean)
            {
                bool isC = false;
                foreach (string atag in a.Tags.compat)
                    foreach (string btag in b.Tags.compat)
                    {
                        if (atag == btag) isC = true;
                    }
                return isC;
            }
            else
                return false;
        }
    }
}
