using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfMem : dfIObG
    {
        dfCom[] _mems;
        public dfCom MasterMem
        {
            get
            {
                dfCom d = GetAll().Find(i => i.Nome != "?" && i.Tipo == "Memoria");
                if (d != null) return d; else return new dfCom("Memoria");
            }
        }
        public dfMem(int Qtd)
        {
            _mems = new dfCom[Qtd];
            for (int i = 0; i < Qtd; i++)
            {
                _mems[i] = new dfCom("Memoria");
            }
        }
        public dfCom[] Mems
        {
            get { return _mems; }
        }
        public string add(dfCom m)
        {
            if (m.Tipo == "Memoria")
            {
                List<dfCom> tl = new List<dfCom>();
                foreach (dfCom d in _mems.ToList())
                    tl.Add(d);
                tl.Add(m);
                dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                if (nulled != null) tl.Remove(nulled);
                if (tl.Count <= _mems.Count())
                {
                    _mems = tl.ToArray();
                    return ("ok");
                }
                else
                {
                    return ("Não há mais slots de memória Disponíveis!(Max: " + _mems.Count() + ")");
                }
            }
            else
            {
                return (m.Nome + " não é uma Memória!");
            }
        }


        internal void renew(int Qtd)
        {
            int lQtd = _mems.Count();
            List<dfCom> tl = new List<dfCom>();
            for (int i = 0; i < lQtd; i++)
            {
                tl.Add(_mems[i]);
            }
            _mems = new dfCom[Qtd];
            if (lQtd <= Qtd)
            {
                for (int i = 0; i < lQtd; i++)
                {
                    _mems[i] = tl[i];
                }
                for (int i = lQtd; i < Qtd; i++)
                {
                    _mems[i] = new dfCom("Memoria");
                }
            }
            else
            {
                for (int i = 0; i < Qtd; i++)
                {
                    _mems[i] = tl[i];
                }
            }
        }

        internal string replace(dfCom Obj, dfCom Target)
        {
            int index = -1;
            index = _mems.ToList().FindIndex(i => i == Target);
            if (index != -1)
            {
                _mems[index] = Obj;
                return "ok";
            }
            else
                return "fail";

        }

        public List<dfCom> GetAll()
        {
            List<dfCom> l = new List<dfCom>();
            foreach (dfCom d in _mems)
                l.Add(d);
            return l;
        }
    }
}
