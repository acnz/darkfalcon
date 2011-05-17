using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfDados : dfIObG
    {
        dfCom[] _sata;
        dfCom[] _ide;
        public dfCom MasterHD
        {
            get
            {
                dfCom  d = GetAll().Find(i => i.Nome != "?" && i.Tipo == "HD");
                if (d != null) return d; else return new dfCom("HD");
            }
        }

        public dfDados(dfCom hd0,int Qtdsata, int Qtdide)
            {
                _sata = new dfCom[Qtdsata];
                _ide = new dfCom[Qtdide];
    
                for (int i = 0; i < Qtdsata; i++)
                {
                    _sata[i] = new dfCom("HD");
                }
                for (int i = 0; i < Qtdide; i++)
                {
                    _ide[i] = new dfCom("Leitor");
                }
            }
        public dfCom[] Sata
        {
            get { return _sata; }
        }
        public dfCom[] Ide
        {
            get { return _ide; }
        }
        public string add(dfCom m)
        {
            if (m.Tipo == "HD" || m.Tipo.Contains("Leitor"))
            {
                string ret = "";
                foreach (string t in m.Tags.compat)
                {
                    if (t == "sata")
                    {
                        List<dfCom> tl = new List<dfCom>();
                        foreach (dfCom d in _sata.ToList())
                            tl.Add(d);
                        tl.Add(m);
                        dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                        if (nulled != null) tl.Remove(nulled);
                        if (tl.Count <= _sata.Count())
                        {
                            _sata = tl.ToArray();
                            ret = "ok";
                        }
                        else
                        {
                                ret = "Não há slots Sata Disponíveis!(Max: " + _sata.Count() + ")";
                        }
                    }
                    else
                        if (t == "ide")
                        {
                            List<dfCom> tl = new List<dfCom>();
                            foreach (dfCom d in _ide.ToList())
                                tl.Add(d);
                             tl.Add(m);
                             dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                             if (nulled != null) tl.Remove(nulled);
                            if (tl.Count <= _ide.Count())
                            {
                                _ide = tl.ToArray();
                                ret = "ok";
                            }
                            else
                            {
                                ret = "Não há slots IDE Disponíveis!(Max: " + _ide.Count() + ")";
                            }
                        }
                        else{
                                    ret = "A Motherboard não possui o tipo de slot necessário!";
                                }

                }
                return ret;
            }
            else
            {
                return (m.Nome + " não é uma HD/Leitor de dados!");
            }
        }


        internal void renew(int Qtd, int Qtd2)
        {
            int lQtd = _sata.Count();
            List<dfCom> tl = new List<dfCom>();
            for (int i = 0; i < lQtd; i++)
            {
                tl.Add(_sata[i]);
            }
            _sata = new dfCom[Qtd];
            if (lQtd <= Qtd)
            {
                for (int i = 0; i < lQtd; i++)
                {
                    _sata[i] = tl[i];
                }
                for (int i = lQtd; i < Qtd; i++)
                {
                    _sata[i] = new dfCom("HD");
                }
            }
            else
            {
                for (int i = 0; i < Qtd; i++)
                {
                    _sata[i] = tl[i];
                }
            }

            lQtd = _ide.Count();
            tl = new List<dfCom>();
            for (int i = 0; i < lQtd; i++)
            {
                tl.Add(_ide[i]);
            }
            _ide = new dfCom[Qtd2];
            if (lQtd <= Qtd2)
            {
                for (int i = 0; i < lQtd; i++)
                {
                    _ide[i] = tl[i];
                }
                for (int i = lQtd; i < Qtd2; i++)
                {
                    _ide[i] = new dfCom("Leitor");
                }
            }
            else
            {
                for (int i = 0; i < Qtd2; i++)
                {
                    _ide[i] = tl[i];
                }
            }
        }
            internal string replace(dfCom Obj, dfCom Target)
        {
            dfCom[] found = new dfCom[] { };

            int a = -1;
            int index = -1;
               a = _sata.ToList().FindIndex(i => i == Target);
               if (a != -1) { found = _sata; index = a; }
               a = _ide.ToList().FindIndex(i => i == Target);
               if (a != -1){ found = _ide;index = a; }
               if (index != -1)
            {
                found[index] = Obj;
                return "ok";
            }
            else
                return "fail";

        }

        public List<dfCom> GetAll()
        {
            List<dfCom> l = new List<dfCom>();
            foreach (dfCom d in _ide)
                l.Add(d);
            foreach (dfCom d in _sata)
                l.Add(d);
            return l;
        }
        
    }
}
