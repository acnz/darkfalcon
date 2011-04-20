using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
        public class dfPlaca : dfIOb
        {
            dfCom[] _pcie2;
            dfCom[] _pcie1;
            dfCom[] _pci;
            dfCom[] _vga;

            public int nMon
            {
                get
                {
                    int i=0;
                    dfCom current =  new dfCom("?","?","PlaVideo",0.0f,"#0");
                    foreach (dfCom d in getAllP())
                    {
                        if (d.Tipo == "PlaVideo")
                        {
                            if (current == d)
                            {
                                i += int.Parse(d.Tags.qtd[0]);
                            }
                            else
                            {
                                if (int.Parse(d.Tags.qtd[0]) >= int.Parse(current.Tags.qtd[0]))
                                {
                                    current = d;
                                    i = 0;
                                    i += int.Parse(current.Tags.qtd[0]);
                                }
                            }
                        }
                    }
                    return i;
                }
            }
            public int nSom
            {
                get
                {
                    int i = 0;
                    dfCom current = new dfCom("?", "?", "PlaSom", 0.0f, "#0");
                    foreach (dfCom d in getAllP())
                    {
                        if (d.Tipo == "PlaSom")
                        {
                            if (current == d)
                            {
                                i += int.Parse(d.Tags.qtd[0]);
                            }
                            else
                            {
                                if (int.Parse(d.Tags.qtd[0]) >= int.Parse(current.Tags.compat[0]))
                                {
                                    current = d;
                                    i = 0;
                                    i += int.Parse(current.Tags.qtd[0]);
                                }
                            }
                        }
                    }
                    return i;
                }
            }
            public int nRede
            {
                get
                {
                    int i = 0;
                    dfCom current = new dfCom("?", "?", "PlaRede", 0.0f, "#0");
                    foreach (dfCom d in getAllP())
                    {
                        if (d.Tipo == "PlaRede")
                        {
                            if (current == d)
                            {
                                i += int.Parse(d.Tags.qtd[0]);
                            }
                            else
                            {
                                if (int.Parse(d.Tags.qtd[0]) >= int.Parse(current.Tags.qtd[0]))
                                {
                                    current = d;
                                    i = 0;
                                    i += int.Parse(current.Tags.qtd[0]);
                                }
                            }
                        }
                    }
                    return i;
                }
            }

            private List<dfCom> getAllP()
            {
                List<dfCom> tl = new List<dfCom>();
                foreach (dfCom d in _pcie2.ToList())
                    tl.Add(d);
                foreach (dfCom d in _pcie1.ToList())
                    tl.Add(d);
                foreach (dfCom d in _pci.ToList())
                    tl.Add(d);
                foreach (dfCom d in _vga.ToList())
                    tl.Add(d);

                return tl;
            }
            public dfPlaca(int Qtdp2, int Qtdp1, int Qtdp, int Qtdv)
            {
                _pcie2 = new dfCom[Qtdp2];
                _pcie1 = new dfCom[Qtdp1];
                _pci = new dfCom[Qtdp];
                _vga = new dfCom[Qtdv];
                for (int i = 0; i < Qtdp2; i++)
                {
                   _pcie2[i]=new dfCom("Pla");
                }
                for (int i = 0; i < Qtdp1; i++)
                {
                    _pcie1[i] = new dfCom("Pla");
                }
                for (int i = 0; i < Qtdp; i++)
                {
                    _pci[i] = new dfCom("Pla");
                }
                for (int i = 0; i < Qtdv; i++)
                {
                    _vga[i] = new dfCom("Pla");
                }
            }

            public dfCom[] Pcie2
            {
                get { return _pcie2; }
            }
            public dfCom[] Pcie1
            {
                get { return _pcie1; }
            }
            public dfCom[] Pci
            {
                get { return _pci; }
            }
            public dfCom[] Vga
            {
                get { return _vga; }
            }
            public string add(dfCom m)
            {
                if (m.Tipo.Contains("Pla"))
                {
                    string ret="";
                    foreach (string t in m.Tags.compat)
                    {
                        if (t == "pcie2")
                        {
                            List<dfCom> tl = new List<dfCom>();
                            foreach (dfCom d in _pcie2.ToList())
                                tl.Add(d);
                            tl.Add(m);
                            dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                            if (nulled != null) tl.Remove(nulled);
                            if (tl.Count <= _pcie2.Count())
                            {
                                _pcie2 = tl.ToArray();
                                ret = "ok";
                            }
                            else
                            {
                                ret = "Não há slots PciExpress 2.0 Disponíveis!(Max: " + _pcie2.Count() + ")";
                            }
                        }
                        else
                            if (t == "pcie1")
                            {
                                List<dfCom> tl = new List<dfCom>();
                                foreach (dfCom d in _pcie1.ToList())
                                    tl.Add(d);
                                tl.Add(m);
                                dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                                if (nulled != null) tl.Remove(nulled);
                                if (tl.Count <= _pcie1.Count())
                                {
                                    _pcie1 = tl.ToArray();
                                    ret = "ok";
                                }
                                else
                                {
                                    ret = "Não há slots PciExpress Disponíveis!(Max: " + _pcie1.Count() + ")";
                                }
                            }
                            else
                                if (t == "pci")
                                {
                                    List<dfCom> tl = new List<dfCom>();
                                    foreach (dfCom d in _pci.ToList())
                                        tl.Add(d);
                                    tl.Add(m);
                                    dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                                    if (nulled != null) tl.Remove(nulled);
                                    if (tl.Count <= _pci.Count())
                                    {
                                        _pci = tl.ToArray();
                                        ret = "ok";
                                    }
                                    else
                                    {
                                        ret = "Não há slots Pci Disponíveis!(Max: " + _pci.Count() + ")";
                                    }
                                }
                                else
                                    if (t == "vga")
                                    {
                                        List<dfCom> tl = new List<dfCom>();
                                        foreach (dfCom d in _vga.ToList())
                                            tl.Add(d);
                                        tl.Add(m);
                                        dfCom nulled = tl.ToList().Find(item => item.Nome == "?");
                                        if (nulled != null) tl.Remove(nulled);
                                        if (tl.Count <= _vga.Count())
                                        {
                                            _vga = tl.ToArray();
                                            ret = "ok";
                                        }
                                        else
                                        {
                                          ret = "Não há slots Vga Disponíveis!(Max: " + _vga.Count() + ")";
                                        }
                                    }
                      
                    }
                    return ret;
                }
                else
                {
                    return (m.Nome + " não é uma Placa!");
                }
            }


            internal void renew(int p, int p_2, int p_3, int p_4)
            {
                int lQtd = _pcie2.Count();
                List<dfCom> tl = new List<dfCom>();
                for (int i = 0; i < lQtd; i++)
                {
                    tl.Add(_pcie2[i]);
                }
                _pcie2 = new dfCom[p];
                for (int i = 0; i < lQtd; i++)
                {
                    _pcie2[i] = tl[i];
                }
                for (int i = lQtd; i < p; i++)
                {
                    _pcie2[i] = new dfCom("Pla");
                }

                lQtd = _pcie1.Count();
                tl = new List<dfCom>();
                for (int i = 0; i < lQtd; i++)
                {
                    tl.Add(_pcie1[i]);
                }
                _pcie1 = new dfCom[p_2];
                for (int i = 0; i < lQtd; i++)
                {
                    _pcie1[i] = tl[i];
                }
                for (int i = lQtd; i < p_2; i++)
                {
                    _pcie1[i] = new dfCom("Pla");
                }

                lQtd = _pci.Count();
                tl = new List<dfCom>();
                for (int i = 0; i < lQtd; i++)
                {
                    tl.Add(_pci[i]);
                }
                _pci = new dfCom[p_3];
                for (int i = 0; i < lQtd; i++)
                {
                    _pci[i] = tl[i];
                }
                for (int i = lQtd; i < p_3; i++)
                {
                    _pci[i] = new dfCom("Pla");
                }

                lQtd = _vga.Count();
                tl = new List<dfCom>();
                for (int i = 0; i < lQtd; i++)
                {
                    tl.Add(_vga[i]);
                }
                _vga = new dfCom[p_4];
                for (int i = 0; i < lQtd; i++)
                {
                    _vga[i] = tl[i];
                }
                for (int i = lQtd; i < p_4; i++)
                {
                    _vga[i] = new dfCom("Pla");
                }
            }
            internal string replace(dfCom Obj, dfCom Target)
            {
                dfCom a = GetAll().Find(i => i == Target);
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
                foreach (dfCom d in _pcie2)
                    l.Add(d);
                foreach (dfCom d in _pcie1)
                    l.Add(d);
                foreach (dfCom d in _pci)
                    l.Add(d);
                foreach (dfCom d in _vga)
                    l.Add(d);
                return l;
            }
        }
    
}
