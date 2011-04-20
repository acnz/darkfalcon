using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfDados : dfIOb
    {
        dfCom[] _sata;
        dfCom[] _ide;
        public dfCom MasterHD{get;set;}

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
            for (int i = 0; i < lQtd; i++)
            {
                _sata[i] = tl[i];
            }
            for (int i = lQtd; i < Qtd; i++)
            {
                _sata[i] = new dfCom("HD");
            }


            lQtd = _ide.Count();
            tl = new List<dfCom>();
            for (int i = 0; i < lQtd; i++)
            {
                tl.Add(_ide[i]);
            }
            _ide = new dfCom[Qtd2];
            for (int i = 0; i < lQtd; i++)
            {
                _ide[i] = tl[i];
            }
            for (int i = lQtd; i < Qtd2; i++)
            {
                _ide[i] = new dfCom("Leitor");
            }}
            internal string replace(dfCom Obj, dfCom Target)
        {
            dfCom  a = GetAll().Find(i => i == Target);
            if (a != null)
            {
                a = Obj;
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
