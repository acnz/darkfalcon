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
        private dfCom _pro;
        private dfCom _gab;
        private dfCom _fonte;
        private dfCom _tec;
        private dfCom _mou;
     #endregion
        #region Publicos
        public dfMobo Motherboard
        {
            get { return _mobo; }
            set {
                if (value.Tipo == "Motherboard")
                    _mobo = value;
                }
        }
        public dfCom Processador
        {
            get { return _pro; }
            set
            {
                if (value.Tipo == "Processador")
                    _pro = value;
            }
        }
        public dfMem Memoria
        {
            get { return _mem; }
            set
            {
                    _mem = value;
            }
        }
        public dfCom Fonte
        {
            get { return _fonte; }
            set
            {
                if (value.Tipo == "Fonte")
                    _fonte = value;
            }
        }
        public dfCom HD
        {
            get { return _da.MasterHD; }
            set
            {
                if (value.Tipo == "HD")
                    _da.MasterHD = value;
            }
        }
        public dfCom Gabinete
        {
            get { return _gab; }
            set
            {
                if (value.Tipo == "Gabinete")
                    _gab = value;
                else
                    _gab = new dfCom("Gabinete");
            }
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
            
                _mem = new dfMem(_mobo.mem);
            
                _fonte = new dfCom("Fonte");
        
                _da = new dfDados(new dfCom("HD"),_mobo.ssata,_mobo.side);

                _monitor = new dfMon(1);
        
                _gab =new dfCom("Gabinete");

                _pl = new dfPlaca(_mobo.spcie2, _mobo.spcie1, _mobo.spci, _mobo.svga);

                _tec = new dfCom("Teclado");
                _mou = new dfCom("Mouse");
            }
            else
            {

            }
        }
        #endregion
        #region Funcs
        public List<dfIOb> getACom()
        {
            List<dfIOb> l = new List<dfIOb>();
         return l;
        }
        public List<dfCom> getVCom()
        {
            List<dfCom> l = new List<dfCom>();
            foreach (dfCom c in getACom())
                if (c != null)
                   l.Add(c);
            return l;
        }
        #endregion 
    }
}
