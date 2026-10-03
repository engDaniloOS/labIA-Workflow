namespace labIA_Workflow.Models
{
    public static class Estoque
    {
        private static List<Produto> produtos = [];

        public static Produto? GetProdutoByName(string nome)
        {
            return GetProdutos()
                .FirstOrDefault(p => string.Equals(p.Nome, nome, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Produto> GetProdutoUpToPreco(double preco)
        {
            return GetProdutos()
                .Where(p => p.Preco <= preco)
                .ToList();
        }

        public static List<Produto> GetProdutos()
        {
            if (produtos?.Count > 0)
                return produtos;

            produtos = 
            [
                new("mouse", 10, 19.99d),
                new("teclado", 5, 29.99d),
                new("monitor", 0, 1139.99d),
                new("gabinete", 3, 199.99d),
                new("fonte", 7, 149.99d),
                new("placa de video", 2, 2499.99d),
                new("memoria RAM", 15, 89.99d),
                new("hd", 8, 129.99d),
                new("ssd", 12, 249.99d),
                new("cooler", 6, 79.99d)
            ];

            return produtos;
        }

        public class Produto(string nome, int quantidade, double preco)
        {
            public long Id { get; set; } = Random.Shared.NextInt64(1, 1000000);
            public string Nome { get; set; } = nome;
            public int Quantidade { get; set; } = quantidade;
            public double Preco { get; set; } = preco;
        }
    }
}
