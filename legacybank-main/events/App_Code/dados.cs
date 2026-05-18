using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for dados
/// </summary>
public class dadosMcc
{
    public class MccInfo
    {
        public string resource { get; set; }
        public string uri { get; set; }
        public items[] items { get; set; }

        public bool has_more { get; set; } //true
        public int limit { get; set; } //100
        public int total_pages { get; set; } //3
        public int page { get; set; } //1
        public int offset { get; set; } //0
        public int total { get; set; } //273
        public int query_count { get; set; } //273

    }
    public class items
    {
        public string id { get; set; }
        public string code { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string resouce { get; set; }
    }
}

public class dadosCEP
{
    public class CEPInfo
    {
      public string cep { get; set; }
      public string logradouro { get; set; }
      public string complemento { get; set; }
      public string bairro { get; set; }
      public string localidade { get; set; }
      public string uf { get; set; }
    }
}