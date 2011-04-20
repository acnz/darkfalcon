using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfMon : dfIOb
    {
        dfCom[] _mons;
        public dfMon(int Qtd)
        {
            _mons = new dfCom[Qtd];
            for(int i = 0; i<Qtd;i++){
                _mons[i] = new dfCom("Monitor");
            }
        }
        public dfCom[] Mons
        {
            get { return _mons; }
        }
        public string add(dfCom m)
        {
            if (m.Tipo == "Monitor")
            {
                List<dfCom> tl = new List<dfCom>();
                foreach (dfCom d in _mons.ToList())
                    tl.Add(d);
                tl.Add(m);
                dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                if (nulled != null) tl.Remove(nulled);
                if (tl.Count <= _mons.Count())
                {
                    _mons = tl.ToArray();
                    return ("ok");
                }
                else
                {
                    return ("Não há mais saidas de vídeo Disponíveis!(Max: " + _mons.Count() + ")");
                }
            }
            else
            {
                return (m.Nome + " não é uma Monitor!");
            }
        }


        internal void renew(int Qtd)
        {
            int lQtd = _mons.Count();
            List<dfCom> tl = new List<dfCom>();
            for (int i = 0; i < lQtd; i++)
            {
                tl.Add(_mons[i]);
            } 
            _mons = new dfCom[Qtd];
            for (int i = 0; i < lQtd; i++)
            {
                _mons[i] = tl[i];
            }
            for (int i = lQtd; i < Qtd; i++)
            {
                _mons[i] = new dfCom("Monitor");
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
            foreach (dfCom d in _mons)
                l.Add(d);
            return l;
        }
        
    }
}
