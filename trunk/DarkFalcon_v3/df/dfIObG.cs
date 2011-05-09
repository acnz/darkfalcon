using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public interface dfIObG : dfIOb
    {
        List<dfCom> GetAll();
    }
}
