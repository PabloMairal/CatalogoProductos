﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogo
{
    public class CustomEventArgs : EventArgs
    {
        public Articulo Articulo { get; set; }
    }
}
