using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfPC
    {
        #region Fields
        private dfMobo _mobo;
        private dfMem _mem;
        private dfPlaca _pl;
        private dfDados _da;
        private dfMon _monitor;
        private dfPeri _per;
        private dfCom _pro;
        private dfCom _gab;
        private dfCom _fonte;
     #endregion
        #region Publicos
        public dfMobo Motherboard
        {
            get { return _mobo; }
        }
        public dfCom Processador
        {
            get { return _pro; }
        }
        public dfMem Memoria
        {
            get { return _mem; }
        }
        public dfCom Fonte
        {
            get { return _fonte; }
        }
        public dfCom HD
        {
            get { return _da.MasterHD; }
        }
        public dfCom Gabinete
        {
            get { return _gab; }
        }
        public int numMon
        {
            get { return _pl.nMon; }
        }
        public dfCom Mouse
        {
            get { return _per.Mouse; }
        }
        public dfCom Teclado
        {
            get { return _per.Teclado; }
        }

        #endregion
        #region Construtres
        public dfPC()
        {
        }
        public dfPC(bool init)
        {
            if (init)
            {
                _mobo = new dfMobo(true);
            
                _pro = new dfCom("Processador");
            
                _mem = new dfMem(_mobo.smem);
            
                _fonte = new dfCom("Fonte");
        
                _da = new dfDados(new dfCom("HD"),_mobo.ssata,_mobo.side);

                _monitor = new dfMon(1);
        
                _gab =new dfCom("Gabinete");

                _pl = new dfPlaca(_mobo.spcie2, _mobo.spcie1, _mobo.spci, _mobo.svga);

                _per = new dfPeri(_mobo.susb, _mobo.Tags.compat.Contains("ps2"));
            }
            else
            {

            }
        }
        #endregion
        #region Funcs
        public List<dfCom> GetAllCom()
        {
            List<dfCom> l = new List<dfCom>();
            l.Add(_mobo);
            l.Add(_pro);
            l.AddRange(_mem.GetAll());
            l.AddRange(_da.GetAll());
            l.Add(_fonte);
            l.AddRange(_per.GetAll());
            l.AddRange(_monitor.GetAll());
            l.Add(_gab);
            l.AddRange(_pl.GetAll());
            
         return l;
        }
        public List<dfCom> getSCom()
        {
            List<dfCom> l = new List<dfCom>();
            l.Add(_mobo);
            l.Add(_pro);
            l.Add(_fonte);
            l.Add(_gab);
            l.Add(_per.Mouse);
            l.Add(_per.Teclado);
            return l;
        }
        public List<dfIObG> getMCom()
        {
            List<dfIObG> l = new List<dfIObG>();
            l.Add(_mem);
            l.Add(_da);
            l.Add(_monitor);
            l.Add(_pl);
            return l;
        }

        public string replace(dfCom Obj,dfCom Target)
        {
                switch (Obj.Tipo)
                {
                    case "Motherboard":
                        _mobo = new dfMobo(Obj);
                        _mem.renew(_mobo.smem);
                        _pl.renew(_mobo.spcie2, _mobo.spcie1, _mobo.spci, _mobo.svga);
                        _da.renew(_mobo.ssata, _mobo.side);
                        _monitor.renew(1);
                        _per.renew(_mobo.susb, _mobo.Tags.compat.Contains("ps2"));
                        return "ok";
                    case "Processador":
                        _pro = Obj;
                        return "ok";
                    case "Gabinete":
                        _gab = Obj;
                        return "ok";
                    case "Fonte":
                        _fonte = Obj;
                        return "ok";
                    default:
                        if (_mem.replace(Obj, Target) == "ok")
                        {
                            return "ok";

                        }else{
                            if (Obj.Tipo == "Memoria")
                            return "Não foi possivel substitir "+Target.Nome+ " por "+ Obj.Nome;
                        }
                       
                            if (_per.replace(Obj, Target) == "ok")
                            {
                                return "ok";
                            }
                            else
                            {
                                if (Obj.Tags.compat.Contains("usb") || Obj.Tags.compat.Contains("ps2"))
                                return "Não foi possivel substitir " + Target.Nome + " por " + Obj.Nome;
                            }
                        if (_monitor.replace(Obj, Target) == "ok")
                        {
                            return "ok";
                        }
                        else
                        {
                            if (Obj.Tipo == "Monitor")
                            return "Não foi possivel substitir " + Target.Nome + " por " + Obj.Nome;
                        }
                        if (_da.replace(Obj, Target) == "ok")
                        {
                            return "ok";
                        }
                        else
                        {
                            if (Obj.Tipo == "HD" || Obj.Tipo == "Leitor")
                            return "Não foi possivel substitir " + Target.Nome + " por " + Obj.Nome;
                        }
                        if (_pl.replace(Obj, Target) == "ok")
                        {
                            if (Obj.Tipo == "PlaVideo")
                            {
                                _monitor.renew(_pl.nMon);
                            }
                            return "ok";
                        }
                        else
                        {
                            if (Obj.Tipo.Contains("Pla"))
                            return "Não foi possivel substitir " + Target.Nome + " por " + Obj.Nome;
                        }
                        return ("Não é um Componente aceito.");
                }

            
        }
        public string add(dfCom c)
        {
            switch (c.Tipo)
            {
                case "Motherboard":
                    if (Motherboard.Nome == "?")
                    {
                        _mobo = new dfMobo(c);
                        _mem.renew(_mobo.smem);
                        _pl.renew(_mobo.spcie2, _mobo.spcie1, _mobo.spci, _mobo.svga);
                        _da.renew(_mobo.ssata, _mobo.side);
                        _monitor.renew(1);
                        _per.renew(_mobo.susb, _mobo.Tags.compat.Contains("ps2"));
                        return "ok";
                    }
                    else
                    {
                        return ("Você já escolheu uma Motherboard!");
                    }
                case "Processador":
                    if (Processador.Nome == "?")
                    {
                        _pro = c;
                        return "ok";
                    }
                    else
                    {
                        return ("Você já escolheu um Processador!");
                    }
                case "Gabinete":
                    if (Gabinete.Nome == "?")
                    {
                        _gab = c;
                        return "ok";
                    }
                    else
                    {
                        return ("Você já escolheu um Gabinete!");
                    }
                case "Fonte":
                    if (Fonte.Nome == "?")
                    {
                        _fonte = c;
                        return "ok";
                    }
                    else
                    {
                        return ("Você já escolheu uma Fonte!");
                    }
                default:
                    string e = "";
                    if ((e = _mem.add(c)) == "ok")
                    {
                        return "ok";
                    }
                    else
                    {
                        if (c.Tipo == "Memoria")
                            return e;
                    }
                    if ((e = _per.add(c)) == "ok")
                    {
                        return "ok";
                    }
                    else
                    {
                        if (c.Tags.compat.Contains("usb") || c.Tags.compat.Contains("ps2"))
                            return e;
                    }
                    if ((e = _monitor.add(c)) == "ok")
                    {
                        return "ok";
                    }
                    else
                    {
                        if (c.Tipo == "Monitor")
                            return e;
                    }
                    if ((e = _da.add(c)) == "ok")
                    {
                        return "ok";
                    }
                    else
                    {
                        if (c.Tipo == "HD" || c.Tipo == "Leitor")
                            return e;
                    }
                    if ((e = _pl.add(c)) == "ok")
                    {
                        if (c.Tipo == "PlaVideo")
                        {
                            _monitor.renew(_pl.nMon);
                        }
                        return "ok";
                    }
                    else
                    {
                        if (c.Tipo.Contains("Pla"))
                            return e;
                    }
                    return ("Não é um Componente aceito.");
            }
        }

        #endregion 
    }
}
