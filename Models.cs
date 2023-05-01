namespace Models
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
            

            Console.WriteLine("aaaa");
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

        public integerField(string verboso = "", bool nulo = false)
        {
            this.verboso = verboso;
            this.nulo = nulo;
        } 
    }
}
