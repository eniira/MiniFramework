using System.Collections.Generic;
using System.Linq;

namespace MiniFramework.Models
{
	class CharField
    {
        public int tamanho {get; set;}
        public string verboso {get; set;}
        public bool nulo {get;set;}

        public CharField(int tamanho = 255, string verboso = "", bool nulo = false)
        {
            this.tamanho = tamanho;
            this.verboso = verboso;
            this.nulo = nulo;
        }
    }

    class TextField
    {
        public string verboso {get; set;}
        public bool nulo {get;set;}

        public TextField(string verboso = "", bool nulo = false)
        {

            this.verboso = verboso;
            this.nulo = nulo;
        }
    }

    class DataField
    {
        public string verboso {get; set;}
        public bool nulo {get;set;}

        public DataField(string verboso = "", bool nulo = false)
        {
            this.verboso = verboso;
            this.nulo = nulo;
        }
    }

    class IntegerField
    {
        public string verboso {get; set;}
        public bool nulo {get;set;}

        public IntegerField(string verboso = "", bool nulo = false)
        {
            this.verboso = verboso;
            this.nulo = nulo;
        } 
    }

    class OneToMany
    {
        public Object obj {get; set;}
        public string modoDel {get; set;}
        public bool nulo {get;set;}

        public IntegerField(Object obj, string modoDel = "", bool nulo = false)
        {
            this.obj = obj;
            this.modoDel = modoDel;
            this.nulo = nulo;
        } 
    }
}