namespace Models
{
    class CharFild
    {
        public int tamanho {get; set}
        public string verboso {get; set}
        public bool nulo {get;set}

        public CharFild(int tamanho = 255, string verboso = '', bool nulo = false)
        {
            this.tamanho = tamanho;
            this.verboso = verboso;
            this.nulo = nulo;
        }
    }

    class TextFild
    {
        public string verboso {get; set}
        public bool nulo {get;set}

        public TextFild(string verboso = '', bool nulo = false)
        {

            this.verboso = verboso;
            this.nulo = nulo;
        }
    }

    class DataFild
    {
        public string verboso {get; set}
        public bool nulo {get;set}

        public DataFild(string verboso = '', bool nulo = false)
        {
            this.verboso = verboso;
            this.nulo = nulo;
        }
    }

    class IntegerFild
    {
        public string verboso {get; set}
        public bool nulo {get;set}

        public integerFild(string verboso = '', bool nulo = false)
        {
            this.verboso = verboso;
            this.nulo = nulo;
        } 
    }
}
