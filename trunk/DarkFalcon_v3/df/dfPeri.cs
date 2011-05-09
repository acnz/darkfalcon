using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfPeri : dfIObG
    {
        private dfCom[] _periu;
        private dfCom[] _perip;
        public dfPeri(int QtdUsb, bool hasPS2)
        {
            _periu = new dfCom[QtdUsb];
            for (int i = 0; i < QtdUsb; i++)
            {
                _periu[i] = (new dfCom("?", "$usb", true));
            }
            if(hasPS2)
            {
                _perip = new dfCom[2];
                _perip[0] = new dfCom("Teclado", "$ps2",true);
                _perip[1] = new dfCom("Mouse", "$ps2", true);

            }
        }
        public dfCom[] Peri
        {
            get { return _periu; }
        }
        public dfCom Mouse
        {
            get { return _perip[1]; }
        }
        public dfCom Teclado
        {
            get { return _perip[0]; }
        }
        public string add(dfCom m)
        {
            string result = "";
            if (m.Tipo != "Motherboard" && (m.Tags.compat.Contains("usb") || m.Tags.compat.Contains("ps2")))
            {
                foreach (string t in m.Tags.compat)
                {
                    if (t == "ps2")
                    {
                        if (_perip != null)
                        {
                            if (m.Tipo == "Teclado")
                            {
                                if (_perip[0].Nome == "?")
                                {
                                    _perip[0] = m;
                                    result = "ok";
                                }
                                else
                                {
                                    result = "Você já escolheu um Teclado!(Max: 1)";
                                }
                            }
                            else
                            {
                                result = m.Nome + " não é um Teclado/Mouse!";

                            }
                            if (m.Tipo == "Mouse")
                            {
                                if (_perip[1].Nome == "?")
                                {
                                    _perip[1] = m;
                                    result = "ok";
                                }
                                else
                                {
                                    result = "Você já escolheu um  Mouse!(Max: 1)";
                                }
                            }
                            else
                            {
                                result = m.Nome + " não é um Teclado/Mouse!";

                            }
                        }
                        else
                        {
                            result = "Não há entradas PS/2 Disponíveis!";
                        }
                    }else
                                    if (t == "usb")
                                    {
                                        List<dfCom> tl = new List<dfCom>();
                                        foreach (dfCom d in _periu.ToList())
                                            tl.Add(d);
                                        tl.Add(m);
                                        dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                                        if (nulled != null) tl.Remove(nulled);
                                        if (tl.Count <= _periu.Count())
                                        {
                                            _periu = tl.ToArray();
                                            result = "ok";
                                        }
                                        else
                                        {
                                            result = "Não há mais slots Usb Disponíveis!(Max: " + _periu.Count() + ")";
                                        }
                                    }
                      
                    }
                return result;  
            }
            else
            {
                return (m.Nome + " não é um Periférico!");
            }
            
        }

        internal void renew(int Qtd, bool p_2)
        {
            int lQtd = _periu.Count();
            List<dfCom> tl = new List<dfCom>();
            for (int i = 0; i < lQtd; i++)
            {
                tl.Add(_periu[i]);
            }
            _periu = new dfCom[Qtd];
            for (int i = 0; i < lQtd; i++)
            {
                _periu[i] = tl[i];
            }
            for (int i = lQtd; i < Qtd; i++)
            {
                _periu[i] = new dfCom("?", "$usb");
            }
            if (p_2)
            {
                if (_perip == null)
                    _perip = new dfCom[2];
                if (_perip[1] ==  null)
                     _perip[1]=new dfCom("Mouse", "$ps2");
                if (_perip[0] == null)
                    _perip[0]=new dfCom("Teclado", "$ps2");
                
            }
            else
            {
                _perip = null;
            }}
            internal string replace(dfCom Obj, dfCom Target)
        {
            dfCom[] found = new dfCom[] { };
            int a = -1;
            int index = -1;
            a = _perip.ToList().FindIndex(i => i == Target);
            if (a != -1) {found = _perip;index = a; }
            a = _periu.ToList().FindIndex(i => i == Target);
            if (a != -1) { found = _periu; index = a; }
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
            foreach (dfCom d in _perip)

                l.Add(d);
            foreach (dfCom d in _periu)
                l.Add(d);
            return l;
        }
        
    }
}
